// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using NUglify.Helpers;

namespace NUglify.Html
{
    /// <summary>
    /// Simplified HTML5 Parser that should handle tag omission correctly (e.g: p, li...etc.)
    /// </summary>
    public class HtmlParser
    {
        private readonly string text;
        private readonly StringBuilder tempBuilder;

        private readonly List<HtmlElement> stack;
        private char c;
        private int position;
        private int line;
        private int column;
        private bool nextLine;
        private bool isEof;
        private readonly string sourceFileName;
        private SourceLocation startTagLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParser"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public HtmlParser(string text, string sourceFileName = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            this.sourceFileName = sourceFileName;
            this.text = text;
            tempBuilder = new StringBuilder();
            Errors = new List<UglifyError>();
            position = -1;
            column = -1;
            stack = new List<HtmlElement>();
        }

        public bool HasErrors { get; private set; }

        public List<UglifyError> Errors { get; }

        private HtmlElement CurrentParent => stack[stack.Count - 1];

        public HtmlDocument Parse()
        {
            stack.Clear();
            stack.Add(new HtmlDocument());

            c = NextChar();
            while (true)
            {
                if (c == 0)
                {
                    break;
                }

                if (c == '<')
                {
                    TryProcessTag();
                }
                else
                {
                    AppendText(c);
                    c = NextChar();
                }
            }

            // Close all remaining tags
            CloseTags(1, GetSourceLocation(), "root");

            return (HtmlDocument) stack[0];
        }

        private void TryProcessTag()
        {
            // Save start tag position
            startTagLocation = GetSourceLocation();

            c = NextChar();
            if (c == '!')
            {
                c = NextChar();
                if (c == '-') // Try to parse comment <!-- .... -->
                {
                    TryProcessComment();
                }
                else if (c == 'D' || c == 'd') // Try to parse directive <!DOCTYPE (case insensitive)
                {
                    TryProcessDoctype();
                }
                else if (c == '[')
                {
                    TryProcessCDATA();
                }
                else
                {
                    // We don't output an error in this case. TODO: Is this correct? Check HTML specs to find out the correct behavior
                    // Error($"Invalid character '{c}' found while parsing <! special tag");
                    AppendText(startTagLocation, position - 1);
                }
            }
            else if (c == '/')
            {
                c = NextChar();
                if (c.IsTagChar())
                {
                    // Parse end-tag
                    TryProcessEndTag();
                }
                else
                {
                    Error($"Invalid character '{c}' following end tag </");
                    AppendText(startTagLocation, position - 1);
                }
            }
            else if (c.IsTagChar() || c == '?')
            {
                TryProcessStartTag();
            }
            else
            {
                Error($"Invalid character '{c}' following start tag <");
                AppendText(startTagLocation, position - 1);
            }
        }

        private void TryProcessDoctype()
        {
            // We are noy fully parsing a DOCTYPE so we just expect that 
            // there won't be any ">" inside a double quote string
            tempBuilder.Clear();
            if (TryParse("DOCTYPE", false, tempBuilder))
            {
                if (char.IsWhiteSpace(c) || c == '>')
                {
                    while (true)
                    {
                        if (c == 0)
                        {
                            goto onerror;
                        }
                        if (c == '>')
                        {
                            c = NextChar();
                            break;
                        }

                        c = NextChar();
                    }

                    var tag = new HtmlDOCTYPE
                    {
                        Location = startTagLocation,
                        Slice = new StringSlice(text, startTagLocation.Position, position - 1)
                    };
                    CurrentParent.AppendChild(tag);
                    return;
                }
            }

            onerror:

            Error(c == 0
                ? $"Invalid EOF found while parsing <!DOCTYPE"
                : $"Invalid character '{c}' found while parsing <!DOCTYPE");

            AppendText(startTagLocation, position - 1);
        }

        private void TryProcessCDATA()
        {
            tempBuilder.Clear();
            if (TryParse("[CDATA[", true, tempBuilder))
            {
                var contentPosition = GetSourceLocation();
                int endPosition = position - 1;
                while (true)
                {
                    if (c == ']')
                    {
                        c = NextChar();
                        if (c == ']')
                        {
                            c = NextChar();
                            if (c == '>')
                            {
                                c = NextChar();
                                break;
                            }
                        }
                    }
                    else if (c == 0)
                    {
                        goto onerror;
                    }

                    endPosition = position;
                    c = NextChar();
                }

                tempBuilder.Clear();
                var tag = new HtmlCDATA()
                {
                    Location = startTagLocation,
                    Slice = new StringSlice(text, contentPosition.Position, endPosition)
                };
                CurrentParent.AppendChild(tag);
                return;
            }

            onerror:
            tempBuilder.Clear();

            Error(c == 0
                ? $"Invalid EOF found while parsing <![CDATA["
                : $"Invalid character '{c}' found while parsing <![CDATA[");

            GetTextNode(startTagLocation).Append(text, startTagLocation.Position, position - 1);
        }

        private void TryProcessStartTag()
        {
            // https://www.w3.org/TR/html-markup/syntax.html#syntax-elements
            // start tags consist of the following parts, in exactly the following order:
            //
            //    A "<" character.
            //    The element’s tag name.
            //       tag names are used within element start tags and end tags to give the element’s name. 
            //       HTML elements all have names that only use characters in the range 0–9, a–z, and A–Z.
            //    Optionally, one or more attributes, each of which must be preceded by one or more space characters.
            //    Optionally, one or more space characters.
            //    Optionally, a "/" character, which may be present only if the element is a void element.
            //    A ">" character.
            tempBuilder.Clear();

            var isProcessingInstruction = false;
            if (c == '?')
            {
                isProcessingInstruction = true;
            }
            else
            {
                tempBuilder.Append(c);
            }

            while(true)
            {
                c = NextChar();
                // TODO: not entirely correct for <? as we should only test for Alpha for the first char
                if (c.IsTagChar() || c == '-') // Allow - character (unlike html)
                {
                    tempBuilder.Append(c);
                }
                else
                {
                    break;
                }
            }

            var tag = new HtmlElement(tempBuilder.ToString())
            {
                Location = startTagLocation,
                Kind = isProcessingInstruction ? ElementKind.ProcessingInstruction : ElementKind.StartWithEnd,
            };
            tag.Descriptor = HtmlTagDescriptor.Find(tag.Name);

            tempBuilder.Clear();

            bool hasAttribute = false;

            bool isValid = false;
            var errorContext = string.Empty;
            while (true)
            {
                var hasWhitespaces = false;

                // Skip any whitespaces
                while (c.IsSpace())
                {
                    c = NextChar();
                    hasWhitespaces = true;
                }

                switch (c)
                {
                    case '\0':
                        goto exit;
                    case '@':
                        // Try to continue parsing the tag even if we have an error
                        // We may be able to recover from it
                        var postText = (string.IsNullOrEmpty(errorContext) ? string.Empty : " " + errorContext);
                        Error($"Invalid character '{c}' found while parsing <{tag.Name}>{postText}");
                        // Fake a whitespace instead
                        c = !isProcessingInstruction && c == '>' ? '>' : ' ';
                        errorContext = null;
                        break;
                    case '?':
                        if (isProcessingInstruction)
                        {
                            c = NextChar();
                            if (c == '>')
                            {
                                c = NextChar();
                                isValid = true;
                                goto exit;
                            }
                        }
                        goto case '@';
                    case '>':
                        if (!isProcessingInstruction)
                        {
                            c = NextChar();
                            isValid = true;
                            goto exit;
                        }
                        goto case '@';
                    case '/':
                        c = NextChar();
                        if (c == '>' && !isProcessingInstruction)
                        {
                            tag.Kind = ElementKind.SelfClosing;
                            c = NextChar();
                            isValid = true;
                            goto exit;
                        }
                        goto case '@';
                    case '=':
                        if (!hasAttribute)
                        {
                            goto case '@';
                        }

                        // Skip any spaces after
                        while (true)
                        {
                            c = NextChar();
                            if (!c.IsSpace())
                            {
                                break;
                            }
                        }

                        tempBuilder.Clear();

                        var attrIndex = tag.Attributes.Count - 1;
                        var attr = tag.Attributes[attrIndex];

                        // Parse a quoted string
                        if (c == '\'' || c == '\"')
                        {
                            var openingStringChar = c;
                            while (true)
                            {
                                c = NextChar();
                                if (c == '\0')
                                {
                                    goto exit;
                                }
                                if (c != openingStringChar)
                                {
                                    tempBuilder.Append(c);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            c = NextChar();
                        }
                        else
                        {
                            // Parse until we match a space or a special html character
                            int matchCount = 0;
                            while (true)
                            {
                                if (c == '\0')
                                {
                                    goto exit;
                                }
                                if (c == ' ' || c == '\n' || c == '"' || c == '\'' || c == '=' || c == '<' || c == '>' || c == '`')
                                {
                                    break;
                                }
                                matchCount++;
                                tempBuilder.Append(c);
                                c = NextChar();
                            }

                            // We need at least one char after '='
                            if (matchCount == 0)
                            {
                                errorContext = $"and after attribute [{attr.Name}]. Expecting valid character after '='";
                                goto case '@';
                            }
                        }

                        attr.Value = tempBuilder.ToString();
                        tag.Attributes[attrIndex] = attr;
                        tempBuilder.Clear();

                        hasAttribute = false;
                        break;
                    default:
                        // Parse the attribute name
                        if (!hasWhitespaces || !(c.IsAlpha() || c == '_' || c == ':'))
                        {
                            goto case '@';
                        }

                        tempBuilder.Clear();
                        tempBuilder.Append(c);

                        while (true)
                        {
                            c = NextChar();
                            if (c.IsAlphaNumeric() || c == '_' || c == ':' || c == '.' || c == '-')
                            {
                                tempBuilder.Append(c);
                            }
                            else
                            {
                                break;
                            }
                        }

                        hasAttribute = true;
                        if (tag.Attributes == null)
                        {
                            tag.Attributes = new List<HtmlAttribute>();
                        }
                        tag.Attributes.Add(new HtmlAttribute(tempBuilder.ToString(), null));

                        tempBuilder.Clear();
                        break;
                }
            }

            exit:

            if (isValid)
            {
                // TODO: Process stack and check if we need to close them
                while (true)
                {
                    var parent = CurrentParent;
                    var parentDescriptor = HtmlTagDescriptor.Find(parent.Name);

                    // If the parent has an AcceptContent == Transparent
                    // We need to find a higher parent that is not transparent and use its ContentKind
                    var nonTransparentParent = parent;
                    var nonTransparentDescriptor = parentDescriptor;
                    while (nonTransparentDescriptor != null && nonTransparentDescriptor.AcceptContent == ContentKind.Transparent)
                    {
                        nonTransparentParent = nonTransparentParent.Parent;
                        nonTransparentDescriptor = HtmlTagDescriptor.Find(nonTransparentParent.Name);
                    }

                    var acceptContentKind = parentDescriptor?.AcceptContent ?? ContentKind.Any;
                    var parentIsTransparent = parentDescriptor != null && parentDescriptor.AcceptContent == ContentKind.Transparent;
                    if (parentIsTransparent && nonTransparentDescriptor != null)
                    {
                        acceptContentKind = nonTransparentDescriptor.AcceptContent;
                    }

                    // - If the parent has no descriptor, we assume that it is a non-HTML tag but it accepts children
                    // - If parent has a descriptor and is not closable by a new tag
                    // - If parent is supporting omission tag but is not closed by current opening tag
                    var isParentClosableByNewTag = parentDescriptor != null && parentDescriptor.EndKind == TagEndKind.Omission;
                    if (parentDescriptor == null || !isParentClosableByNewTag || !parentDescriptor.ShouldCloseParentOnTag(parent, tag))
                    {
                        if (parentDescriptor != null && !isParentClosableByNewTag)
                        {
                            // Check if the parent accepts the tag
                            // we will emit a warning just in case
                            if (!parentDescriptor.TryAcceptContent(parent, parentDescriptor, acceptContentKind, tag, tag.Descriptor))
                            {
                                Warning(tag.Location, $"The tag <{tag.Name}> is not a valid tag within the parent tag <{parent.Name}>");
                            }
                        }

                        parent.AppendChild(tag);

                        if ((tag.Descriptor == null || tag.Descriptor.AcceptContent != ContentKind.None || tag.Descriptor.AcceptContentTags != null) && tag.Kind != ElementKind.SelfClosing)
                        {
                            stack.Add(tag);
                        }
                        break;
                    }

                    // This should not happen, so throw an error if we got there
                    if (stack.Count == 1)
                    {
                        Error(tag.Location, $"The tag <{tag.Name}> is not a valid tag within the parent tag <{parent.Name}>");
                        break;
                    }

                    PopStack();
                }

                // The content of SCRIPT and STYLE are considered as kind of "CDATA"
                // and are expecting to mach either a </script> or </style>
                // so we parse the content immediately here
                if (tag.Kind != ElementKind.SelfClosing && (tag.Name.Equals("script", StringComparison.OrdinalIgnoreCase) || tag.Name.Equals("style", StringComparison.OrdinalIgnoreCase)))
                {
                    ParseScriptOrStyleContent(tag);

                    // Remove the <script> or <style> element from the stack as we have parse it
                    PopStack();
                }
            }
            else
            {
                var tagName = tag.Name;
                if (isProcessingInstruction)
                {
                    tagName = "?" + tagName + "?";
                }

                Error(c == 0
                    ? $"Invalid EOF found while parsing <{tagName}>"
                    : $"Invalid character '{c}' found while parsing <{tagName}>");

                AppendText(startTagLocation, position - 1);
            }
        }

        private void PopStack()
        {
            if (stack.Count == 1)
            {
                Error("Cannot pop HTML root element");
                return;
            }

            // Remove the script from the stack
            stack.RemoveAt(stack.Count - 1);
        }

        private void ParseScriptOrStyleContent(HtmlElement tag)
        {
            int endPosition;
            var contentPosition = GetSourceLocation();
            while (true)
            {
                if (c == 0)
                {
                    Error($"Invalid EOF found while parsing content of tag [{CurrentParent}]");
                    AppendText(contentPosition, position - 1);
                    return;
                }

                endPosition = position;
                if (c == '<')
                {
                    c = NextChar();
                    if (c == '/')
                    {
                        tempBuilder.Clear();

                        c = NextChar();
                        if (TryParse(tag.Name, false, tempBuilder))
                        {
                            while (c.IsSpace())
                            {
                                c = NextChar();
                            }

                            if (c == '>')
                            {
                                c = NextChar();
                                break;
                            }

                            Warning($"Invalid end of tag <{tag.Name}>. Expecting a '>'");
                        }
                    }
                }
                c = NextChar();
            }

            if (endPosition - 1 >= contentPosition.Position)
            {
                CurrentParent.AppendChild(new HtmlRaw()
                {
                    Location = contentPosition,
                    Slice = new StringSlice(text, contentPosition.Position, endPosition - 1)
                });
            }
        }

        private void TryProcessEndTag()
        {
            var endTagSpan = GetSourceLocation();

            tempBuilder.Clear();
            tempBuilder.Append(c);

            while (true)
            {
                c = NextChar();
                if (c.IsAlphaNumeric() || c == '_' || c == ':' || c == '.' || c == '-')
                {
                    tempBuilder.Append(c);
                }
                else
                {
                    break;
                }
            }

            // Skip any spaces after
            while (c.IsSpace())
            {
                c = NextChar();
            }

            var tagName = tempBuilder.ToString().ToLowerInvariant();

            if (c == '>')
            {
                c = NextChar();

                int indexOfOpenTag;
                for (indexOfOpenTag = stack.Count - 1; indexOfOpenTag >= 0; indexOfOpenTag--)
                {
                    if (string.Equals(stack[indexOfOpenTag].Name, tagName, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                // Opening tag found?
                if (indexOfOpenTag >= 0)
                {
                    CloseTags(indexOfOpenTag, startTagLocation, tagName);
                }
                else
                {
                    // If we have a closing tag without an opening tag
                    // Log a warning but keep the tag (that should be an error, but we assume we can recover from it)
                    var invalidTag = new HtmlElement(tagName)
                    {
                        Location = startTagLocation,
                        Kind = ElementKind.EndWithoutStart
                    };
                    invalidTag.Descriptor = HtmlTagDescriptor.Find(invalidTag.Name);

                    CurrentParent.AppendChild(invalidTag);

                    Warning(startTagLocation, $"Unable to find opening tag for closing tag </{tagName}>");
                }
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing </{tagName}>"
                    : $"Invalid character '{c}' found while parsing </{tagName}>");

                AppendText(startTagLocation, position - 1);
            }

            tempBuilder.Clear();
        }

        private void CloseTags(int indexOfOpenTag, SourceLocation span, string parentTag)
        {
            // Else we will close all intermediate tag
            bool isFirstTag = true;
            for (int i = stack.Count - 1; i >= indexOfOpenTag; i--)
            {
                var element = stack[i];
                var elementDesc = HtmlTagDescriptor.Find(element.Name);
                if (i > indexOfOpenTag
                    && (elementDesc == null || (elementDesc.AcceptContent != ContentKind.None && (elementDesc.EndKind != TagEndKind.Omission || !isFirstTag))))
                {
                    Warning(span, $"Unbalanced tag [{element.Name}] within tag [{parentTag}] requiring a closing tag. Force closing it");
                }

                stack.RemoveAt(i);
                isFirstTag = false;
            }
        }

        private void TryProcessComment()
        {
            c = NextChar();
            if (c != '-')
            {
                AppendText(startTagLocation, position - 1);
                return;
            }

            var commentPosition = position + 1;

            while (true)
            {
                c = NextChar();

                if (c == '-')
                {
                    c = NextChar();
                    if (c == '-')
                    {
                        c = NextChar();
                        if (c == '>')
                        {
                            // Don't eat last char, as it will be processed by main loop
                            c = NextChar();

                            var comment = new HtmlComment()
                            {
                                Location = startTagLocation,
                                Slice = new StringSlice(text, commentPosition, position - 4)
                            };
                            CurrentParent.AppendChild(comment);
                            return;
                        }

                        Error(c == 0
                            ? $"Invalid EOF found while parsing <!--"
                            : $"Invalid character '{c}' found while parsing <!--");

                        AppendText(startTagLocation, position - 1);
                        return;
                    }
                }

                if (c == '\0')
                {
                    Error($"Invalid EOF found while parsing comment");

                    AppendText(startTagLocation, position - 1);
                    return;
                }
            }
        }

        private void AppendText(char character)
        {
            GetTextNode(GetSourceLocation()).Append(text, position, character);
        }

        private void AppendText(SourceLocation from, int to)
        {
            GetTextNode(from).Append(text, from.Position, to);
        }

        private bool TryParse(string text, bool isCaseSensitive, StringBuilder builderArg)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (isCaseSensitive)
                {
                    if (c != text[i])
                    {
                        return false;
                    }
                }
                else if (char.ToUpperInvariant(c) != char.ToUpperInvariant(text[i]))
                {
                    return false;
                }
                if (c != 0)
                {
                    builderArg.Append(c);
                }
                c = NextChar();
            }
            return true;
        }

        private HtmlText GetTextNode(SourceLocation from)
        {
            var textNode = CurrentParent.LastChild as HtmlText;
            if (textNode == null)
            {
                textNode = new HtmlText() {Location = from};
                CurrentParent.AppendChild(textNode);
            }
            return textNode;
        }

        private SourceLocation GetSourceLocation()
        {
            return new SourceLocation() {Line = line + 1, Column = column + 1, Position = position};
        }

        private void Error(string message)
        {
            Error(GetSourceLocation(), message);
        }

        private void Error(SourceLocation location, string message)
        {
            Errors.Add(new UglifyError()
            {
                File = sourceFileName,
                IsError = true,
                StartColumn = location.Column,
                EndColumn = location.Column,
                StartLine = location.Line,
                EndLine = location.Line,
                Message = message
            });
            HasErrors = true;
        }

        private void Warning(string message)
        {
            Warning(GetSourceLocation(), message);
        }

        private void Warning(SourceLocation location, string message)
        {
            Errors.Add(new UglifyError()
            {
                File = sourceFileName,
                IsError = false,
                StartColumn = location.Column,
                EndColumn = location.Column,
                StartLine = location.Line,
                EndLine = location.Line,
                Message = message
            });
        }

        private char NextChar()
        {
            if (isEof)
            {
                return (char)0;
            }

            if (nextLine)
            {
                column = -1;
                line++;
                nextLine = false;
            }
            column++;

            position++;
            if (position  == text.Length)
            {
                isEof = true;
                c = (char)0;
            }
            else
            {
                c = text[position];
                if (c == '\n')
                {
                    nextLine = true;
                }
            }
            return c;
        }
    }
}
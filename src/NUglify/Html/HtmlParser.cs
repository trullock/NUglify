// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace NUglify.Html
{
    public class HtmlParser
    {
        private readonly string text;
        private readonly StringBuilder tempBuilder;

        private readonly List<HtmlElement> stack;
        private HtmlNode node;
        private char c;
        private int position;
        private int line;
        private int column;
        private bool nextLine;
        private bool isEof;
        private readonly string sourceFileName;
        private int startTagPosition;


        public HtmlParser(string text, string sourceFileName = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            this.sourceFileName = sourceFileName;
            this.text = text;
            tempBuilder = new StringBuilder();
            Messages = new List<UglifyError>();
            position = -1;
            column = -1;
            stack = new List<HtmlElement>();
        }

        public bool HasErrors { get; private set; }

        public List<UglifyError> Messages { get; }

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
                    AppendText(position, c);
                    c = NextChar();
                }
            }

            return HasErrors ? null : (HtmlDocument) stack[0];
        }

        private void TryProcessTag()
        {
            // Save start tag position
            startTagPosition = position;

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
                    AppendText(startTagPosition, position - 1);
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
                    // We don't output an error in this case. TODO: Is this correct? Check HTML specs to find out the correct behavior
                    // Error($"Invalid character '{c}' found while parsing </ end tag");
                    AppendText(startTagPosition, position - 1);
                }
            }
            else if (c.IsTagChar() || c == '?')
            {
                TryProcessStartTag();
            }
            else
            {
                // We don't output an error in this case. TODO: Is this correct? Check HTML specs to find out the correct behavior
                // Error($"Invalid character '{c}' found while parsing <");
                AppendText(startTagPosition, position - 1);
            }
        }

        private void TryProcessDoctype()
        {
            tempBuilder.Clear();
            if (TryParse("DOCTYPE", false, tempBuilder))
            {
                if (char.IsWhiteSpace(c) || c == '>')
                {
                    while (true)
                    {
                        if (c == 0)
                        {
                            Error($"Invalid EOF found while parsing <!DOCTYPE");
                            return;
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
                        Slice = new StringSlice(text, startTagPosition, position - 1)
                    };

                    node = tag;
                    CurrentParent.AppendChild(node);
                }
                else
                {
                    Error(c == 0
                        ? $"Invalid EOF found while parsing <!DOCTYPE"
                        : $"Invalid character '{c}' found while parsing <!DOCTYPE");
                }
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing <!DOCTYPE"
                    : $"Invalid character '{c}' found while parsing <!DOCTYPE");
            }
        }

        private void TryProcessCDATA()
        {
            int i = 0;
            tempBuilder.Clear();
            if (TryParse("[CDATA[", true, tempBuilder))
            {
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
                        Error($"Invalid EOF found while parsing CDATA");
                        tempBuilder.Clear();
                        return;
                    }
                    c = NextChar();
                }

                var tag = new HtmlCDATA()
                {
                    Slice = new StringSlice(text, startTagPosition, position - 4)
                };
                node = tag;
                CurrentParent.AppendChild(node);
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing <![CDATA["
                    : $"Invalid character '{c}' found while parsing <![CDATA[");
            }
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

            var tag = new HtmlElement()
            {
                Name = tempBuilder.ToString().ToLowerInvariant(),
                IsProcessingInstruction = isProcessingInstruction
            };

            bool hasAttribute = false;

            bool isValid = false;
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
                    case '?':
                        if (isProcessingInstruction)
                        {
                            c = NextChar();
                            if (c == '>')
                            {
                                c = NextChar();
                                isValid = true;
                            }
                        }
                        goto exit;
                    case '>':
                        if (!isProcessingInstruction)
                        {
                            c = NextChar();
                            isValid = true;
                        }
                        goto exit;
                    case '/':
                        c = NextChar();
                        if (c == '>' && !isProcessingInstruction)
                        {
                            tag.IsClosed = true;
                            c = NextChar();
                            isValid = true;
                        }
                        goto exit;
                    case '=':
                        if (!hasAttribute)
                        {
                            goto exit;
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
                                goto exit;
                            }
                        }

                        var attrIndex = tag.Attributes.Count - 1;
                        var attr = tag.Attributes[attrIndex];
                        attr.Value = tempBuilder.ToString();
                        tag.Attributes[attrIndex] = attr;
                        tempBuilder.Clear();

                        hasAttribute = false;
                        continue;
                    default:
                        if (!hasWhitespaces)
                        {
                            goto exit;
                        }

                        // Parse the attribute name
                        if (!(c.IsAlpha() || c == '_' || c == ':'))
                        {
                            goto exit;
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
                node = null;
                while (true)
                {
                    var parent = CurrentParent;
                    var parentDescriptor = HtmlTagDescriptor.Find(parent.Name);
                    var childDescriptor = HtmlTagDescriptor.Find(tag.Name);

                    var nonTransparentParent = parent;
                    var nonTransparentDescriptor = parentDescriptor;
                    while (nonTransparentDescriptor != null && nonTransparentDescriptor.AcceptContent == ContentKind.Transparent)
                    {
                        nonTransparentParent = parent.Parent;
                        nonTransparentDescriptor = HtmlTagDescriptor.Find(nonTransparentParent.Name);
                    }

                    var acceptContentKind = parentDescriptor != null ? parentDescriptor.AcceptContent : ContentKind.Any;
                    if (nonTransparentDescriptor != null)
                    {
                        acceptContentKind = nonTransparentDescriptor.AcceptContent;
                    }

                    if (parentDescriptor == null || parentDescriptor.TryAcceptContent(parent, parentDescriptor, acceptContentKind, tag, childDescriptor))
                    {
                        parent.AppendChild(tag);

                        if (childDescriptor == null || ((childDescriptor.AcceptContent & ContentKind.None) == 0 || childDescriptor.AcceptContentTags != null) && !tag.IsClosed)
                        {
                            stack.Add(tag);
                        }
                        break;
                    }

                    stack.RemoveAt(stack.Count - 1);
                }

                // The content of SCRIPT and STYLE are considered as CDATA
                // and are expecting to mach either a </script> or </style>
                if (!tag.IsClosed && ((tag.Name == "script") || (tag.Name == "style")))
                {
                    ParseScriptOrStyleContent();
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
            }
        }

        private void ParseScriptOrStyleContent()
        {
            int startPositionContent = position;
            int endPosition;
            while (true)
            {
                if (c == 0)
                {
                    Error($"Invalid EOF found while parsing content of tag [{CurrentParent}]");
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
                        bool tagFound = false;
                        if (TryParse("SCRIPT", false, tempBuilder))
                        {
                            tagFound = true;
                        }
                        else if (tempBuilder.Length == 1) // if script was partially match, we should have only the 's'
                        {
                            if (TryParse("TYLE", false, tempBuilder))
                            {
                                tagFound = true;
                            }
                        }

                        if (tagFound)
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
                        }
                    }
                }
                c = NextChar();
            }

            node = null;

            if (endPosition - 1 >= startPositionContent)
            {
                CurrentParent.AppendChild(new HtmlRaw()
                {
                    Slice = new StringSlice(text, startPositionContent, endPosition - 1)
                });
            }

            // Remove the script from the stack
            stack.RemoveAt(stack.Count - 1);
        }

        private void TryProcessEndTag()
        {
            var endTagSpan = GetSourceSpan();

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
                node = null;
                // TODO: Process stack and check if we need to close them
                c = NextChar();

                int indexOfOpenTag;
                for (indexOfOpenTag = stack.Count - 1; indexOfOpenTag >= 0; indexOfOpenTag--)
                {
                    if (string.Equals(stack[indexOfOpenTag].Name, tagName, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                if (indexOfOpenTag < 0)
                {
                    Warning(endTagSpan, $"Unable to find opening tag for closing tag </{tagName}>. Tag will be discarded");
                }
                else
                {
                    for (int i = stack.Count - 1; i >= indexOfOpenTag; i--)
                    {
                        var element = stack[i];
                        var elementDesc = HtmlTagDescriptor.Find(element.Name);
                        if (elementDesc != null && elementDesc.AcceptContent != ContentKind.None && i > indexOfOpenTag)
                        {
                            Warning(endTagSpan, $"Unbalanced tag [{element.Name}] within tag [{tagName}] requiring a closing tag. Force closing it");
                        }

                        stack.RemoveAt(i);
                    }
                }
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing </{tagName}>"
                    : $"Invalid character '{c}' found while parsing </{tagName}>");
            }

            tempBuilder.Clear();
        }

        private void TryProcessComment()
        {
            c = NextChar();
            if (c != '-')
            {
                AppendText(startTagPosition, position - 1);
                return;
            }

            var commentPosition = position;

            var commentNode = new HtmlComment();
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
                                Slice = new StringSlice(text, commentPosition, position - 4)
                            };
                            node = null;
                            CurrentParent.AppendChild(comment);
                            return;
                        }

                        Error(c == 0
                            ? $"Invalid EOF found while parsing <!--"
                            : $"Invalid character '{c}' found while parsing <!--");

                        return;
                    }
                }

                if (c == '\0')
                {
                    Error($"Invalid EOF found while parsing comment");
                    return;
                }
            }
        }

        private void AppendText(int from, char character)
        {
            GetTextNode().Append(text, from, character);
        }

        private void AppendText(int from, int to)
        {
            GetTextNode().Append(text, from, to);
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

        private HtmlText GetTextNode()
        {
            var textNode = node as HtmlText;
            if (textNode == null)
            {
                node = textNode = new HtmlText();
                CurrentParent.AppendChild(node);
            }
            return textNode;
        }

        private SourceSpan GetSourceSpan()
        {
            return new SourceSpan() {Line = line + 1, Column = column + 1, Position = position};
        }

        private void Error(string message)
        {
            Error(GetSourceSpan(), message);
        }

        private void Error(SourceSpan span, string message)
        {
            Messages.Add(new UglifyError()
            {
                File = sourceFileName,
                IsError = true,
                StartColumn = span.Column,
                EndColumn = span.Column,
                StartLine = span.Line,
                EndLine = span.Line,
                Message = message
            });
            HasErrors = true;
        }

        private void Warning(string message)
        {
            Warning(GetSourceSpan(), message);
        }

        private void Warning(SourceSpan span, string message)
        {
            Messages.Add(new UglifyError()
            {
                File = sourceFileName,
                IsError = false,
                StartColumn = span.Column,
                EndColumn = span.Column,
                StartLine = span.Line,
                EndLine = span.Line,
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

            if (position + 1 == text.Length)
            {
                isEof = true;
                c = (char)0;
            }
            else
            {
                position++;
                c = text[position];
                if (c == '\n')
                {
                    nextLine = true;
                }
            }
            return c;
        }

        private struct SourceSpan
        {
            public int Position;

            public int Line;

            public int Column;
        }
    }
}
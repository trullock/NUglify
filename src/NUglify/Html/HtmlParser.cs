// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NUglify.Html
{
    public class HtmlParser
    {
        private readonly TextReader reader;
        private readonly StringBuilder builder;
        private readonly StringBuilder nameBuilder;
        private readonly StringBuilder valueBuilder;

        private List<HtmlNode> nodes;
        private HtmlNode node;
        private char c;
        private int line;
        private int column;
        private bool nextLine;

        public HtmlParser(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            this.reader = reader;
            builder = new StringBuilder();
            nameBuilder = new StringBuilder();
            valueBuilder = new StringBuilder();
            Errors = new List<UglifyError>();
        }

        public bool HasError => Errors.Count > 0;

        public List<UglifyError> Errors { get; }

        public List<HtmlNode> Parse()
        {
            nodes = new List<HtmlNode>();
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
                    GetNode<HtmlTextNode>().Text.Append(c);
                    c = NextChar();
                }
            }

            return nodes;
        }

        private void TryProcessTag()
        {
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
                    Error($"Invalid character '{c}' found while parsing <! special tag");
                    GetNode<HtmlTextNode>().Text.Append("<!");
                }
            }
            else if (c == '/')
            {
                c = NextChar();
                if (IsTagChar(c))
                {
                    // Parse end-tag
                    TryProcessEndTag();
                }
                else
                {
                    Error($"Invalid character '{c}' found while parsing </ end tag");
                    GetNode<HtmlTextNode>().Text.Append("</");
                }
            }
            else if (IsTagChar(c) || c == '?')
            {
                TryProcessStartTag();
            }
            else
            {
                Error($"Invalid character '{c}' found while parsing <");
                GetNode<HtmlTextNode>().Text.Append('<');
            }
        }

        private void TryProcessDoctype()
        {
            builder.Clear();
            if (TryParse("DOCTYPE", false, builder))
            {
                var tag = new HtmlDOCTYPENode();
                tag.Text.Append("<!DOCTYPE");

                if (char.IsWhiteSpace(c) || c == '>')
                {
                    while (true)
                    {
                        if (c == 0)
                        {
                            Error($"Invalid EOF found while parsing <!DOCTYPE");

                            GetNode<HtmlTextNode>().Text.Append(tag.Text);
                            return;
                        }
                        tag.Text.Append(c);
                        if (c == '>')
                        {
                            c = NextChar();
                            break;
                        }

                        c = NextChar();
                    }
                    node = tag;
                    nodes.Add(tag);
                }
                else
                {
                    Error($"Invalid character '{c}' found while parsing <!DOCTYPE");

                    GetNode<HtmlTextNode>().Text.Append(tag.Text);
                }
            }
            else
            {
                Error($"Invalid character '{c}' found while parsing <!DOCTYPE");

                GetNode<HtmlTextNode>().Text.Append("<!").Append(builder);
            }
        }

        private void TryProcessCDATA()
        {
            int i = 0;
            builder.Clear();
            if (TryParse("[CDATA[", true, builder))
            {
                var tag = new HtmlCDATANode();
                tag.Text.Clear();
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
                            tag.Text.Append("]]");
                        }
                        else
                        {
                            tag.Text.Append(']');
                        }
                    }
                    else if (c == 0)
                    {
                        Error($"Invalid EOF found while parsing CDATA");

                        GetNode<HtmlTextNode>().Text.Append("<![CDATA[").Append(tag.Text);
                        return;
                    }
                    tag.Text.Append(c);
                    c = NextChar();
                }
                node = tag;
                nodes.Add(tag);
            }
            else
            {
                Error($"Invalid character '{c}' found while parsing <![CDATA[");

                GetNode<HtmlTextNode>().Text.Append("<!").Append(builder);
                builder.Clear();
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
            builder.Clear();
            nameBuilder.Clear();

            var isProcessingInstruction = false;
            if (c == '?')
            {
                isProcessingInstruction = true;
            }
            else
            {
                nameBuilder.Append(c);
            }

            builder.Append(c);
            while(true)
            {
                c = NextChar();
                if (IsTagChar(c) || c == '-') // Allow - character (unlike html)
                {
                    builder.Append(c);
                    nameBuilder.Append(c);
                }
                else
                {
                    break;
                }
            }
            // If we don't have a space, or a closing >, this is not a valid start tag
            // we just early exit

            if (!(IsSpace(c) || c == '>'))
            {
                Error($"Invalid character '{c}' found while parsing tag");

                GetNode<HtmlTextNode>().Text.Append('<');
                GetNode<HtmlTextNode>().Text.Append(builder);
                nameBuilder.Clear();
                return;
            }

            var tag = new HtmlTagNode()
            {
                Name = nameBuilder.ToString(),
                IsProcessingInstruction = isProcessingInstruction
            };
            HtmlAttribute currentAttribute = null;

            bool isValid = false;
            while (true)
            {
                var hasWhitespaces = false;

                // Skip any whitespaces
                while (IsSpace(c))
                {
                    builder.Append(c);

                    c = NextChar();
                    hasWhitespaces = true;
                }

                switch (c)
                {
                    case '\0':
                        goto exit;
                    case '?':
                        builder.Append(c);
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
                        if (c == '>')
                        {
                            c = NextChar();
                            isValid = true;
                        }
                        else
                        {
                            builder.Append('/');
                        }
                        goto exit;
                    case '=':
                        builder.Append('=');
                        if (currentAttribute == null)
                        {
                            goto exit;
                        }

                        // Skip any spaces after
                        while (true)
                        {
                            c = NextChar();
                            if (IsSpace(c))
                            {
                                builder.Append(c);
                            }
                            else
                            {
                                break;
                            }
                        }

                        valueBuilder.Clear();

                        // Parse a quoted string
                        if (c == '\'' || c == '\"')
                        {
                            builder.Append(c);
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
                                    valueBuilder.Append(c);
                                    builder.Append(c);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            builder.Append(c);

                            currentAttribute.Value = valueBuilder.ToString();
                            valueBuilder.Clear();

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
                                builder.Append(c);
                                valueBuilder.Append(c);
                                c = NextChar();
                            }

                            // We need at least one char after '='
                            if (matchCount == 0)
                            {
                                goto exit;
                            }

                            currentAttribute.Value = valueBuilder.ToString();
                            valueBuilder.Clear();
                        }

                        currentAttribute = null;
                        continue;
                    default:
                        if (!hasWhitespaces)
                        {
                            goto exit;
                        }

                        // Parse the attribute name
                        if (!(IsAlpha(c) || c == '_' || c == ':'))
                        {
                            goto exit;
                        }

                        nameBuilder.Clear();
                        nameBuilder.Append(c);
                        builder.Append(c);

                        while (true)
                        {
                            c = NextChar();
                            if (IsAlphaNumeric(c) || c == '_' || c == ':' || c == '.' || c == '-')
                            {
                                nameBuilder.Append(c);
                                builder.Append(c);
                            }
                            else
                            {
                                break;
                            }
                        }

                        currentAttribute = new HtmlAttribute
                        {
                            Name = nameBuilder.ToString()
                        };
                        tag.Attributes.Add(currentAttribute);

                        nameBuilder.Clear();
                        break;
                }
            }

            exit:

            if (isValid)
            {
                nodes.Add(tag);
                node = tag;

                if (string.Equals(tag.Name, "script", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(tag.Name, "style", StringComparison.OrdinalIgnoreCase))
                {
                    ParseUntilEndTag(tag);
                }
            }
            else
            {
                Error($"Invalid character '{c}' found while parsing tag");

                GetNode<HtmlTextNode>().Text.Append('<');
                GetNode<HtmlTextNode>().Text.Append(builder);
            }
        }

        private void ParseUntilEndTag(HtmlTagNode tag)
        {
            var temp = new StringBuilder();
            builder.Clear();
            while (true)
            {
                if (c == 0)
                {
                    Error($"Invalid EOF found while parsing content of tag [{tag.Name}]");
                    return;
                }

                if (c == '<')
                {
                    c = NextChar();
                    if (c == '/')
                    {
                        c= NextChar();
                        if (c == 's' || c == 'S')
                        {
                            temp.Clear();
                            bool tagFound = false;
                            if (TryParse("script", false, temp))
                            {
                                tagFound = true;
                            }
                            else if (temp.Length == 1)
                            {
                                if (TryParse("tyle", false, temp))
                                {
                                    tagFound = true;
                                }
                            }

                            if (tagFound)
                            {
                                while (IsSpace(c))
                                {
                                    temp.Append(c);
                                    c = NextChar();
                                }

                                if (c == '>')
                                {
                                    c = NextChar();
                                    break;
                                }
                            }

                            builder.Append("</");
                            builder.Append(temp);
                        }
                        else
                        {
                            builder.Append("</");
                        }
                    }
                    else
                    {
                        builder.Append('<');
                    }
                }

                builder.Append(c);
                c = NextChar();
            }

            tag.Content = builder.ToString();
        }

        private void TryProcessEndTag()
        {
            nameBuilder.Clear();
            nameBuilder.Append(c);
            builder.Clear();
            builder.Append(c);

            while (true)
            {
                c = NextChar();
                if (IsAlphaNumeric(c) || c == '_' || c == ':' || c == '.' || c == '-')
                {
                    nameBuilder.Append(c);
                    builder.Append(c);
                }
                else
                {
                    break;
                }
            }

            // Skip any spaces after
            while (IsSpace(c))
            {
                builder.Append(c);
                c = NextChar();
            }

            if (c == '>')
            {
                var tag = new HtmlEndTagNode() {Name = nameBuilder.ToString()};
                node = tag;
                nodes.Add(tag);
                c = NextChar();
            }
            else
            {
                Error($"Invalid character '{c}' found while parsing end tag");

                GetNode<HtmlTextNode>().Text.Append("</");
                GetNode<HtmlTextNode>().Text.Append(builder);
            }

            nameBuilder.Clear();
        }

        private void TryProcessComment()
        {
            c = NextChar();
            if (c != '-')
            {
                GetNode<HtmlTextNode>().Text.Append("<!-");
                return;
            }

            var commentNode = GetNode<HtmlCommentNode>();
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
                            return;
                        }

                        Error($"Invalid character '{c}' found while parsing comment");
                        commentNode.Text.Append("--");
                    }
                    else
                    {
                        Error($"Invalid character '{c}' found while parsing comment");
                        commentNode.Text.Append('-');
                    }
                }

                if (c == '\0')
                {
                    Error($"Invalid EOF '{c}' found while parsing comment");
                    break;
                }

                commentNode.Text.Append(c);
            }
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

        private T GetNode<T>() where T : HtmlNode, new()
        {
            var textNode = node as T;
            if (textNode == null)
            {
                node = textNode = new T();
                nodes.Add(node);
            }
            return textNode;
        }

        private void Error(string message)
        {
            Errors.Add(new UglifyError()
            {
                StartColumn = column,
                EndColumn = column,
                StartLine = line,
                EndLine = line,
                Message = message
            });
        }

        private char NextChar()
        {
            if (nextLine)
            {
                column = 0;
                line++;
                nextLine = false;
            }

            var nextChar = reader.Read();
            if (nextChar < 0)
            {
                nextChar = 0;
            }
            var nc = (char)nextChar;
            if (nc != 0)
            {
                if (nc == '\n')
                {
                    nextLine = true;
                }
                else
                {
                    column++;
                }
            }
            return nc;
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsTagChar(char c)
        {
            return IsAlphaNumeric(c);
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsAlphaUpper(char c)
        {
            return (c >= 'A' && c <= 'Z');
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsAlphaNumeric(char c)
        {
            return (c >= '0' && c <= '9') || IsAlpha(c);
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\f' || c == '\n';
        }
    }
}
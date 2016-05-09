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
        private bool isEof;
        private readonly string sourceFileName;

        public HtmlParser(TextReader reader, string sourceFileName = null)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            this.sourceFileName = sourceFileName;
            this.reader = reader;
            builder = new StringBuilder();
            nameBuilder = new StringBuilder();
            valueBuilder = new StringBuilder();
            Errors = new List<UglifyError>();
            column = -1;
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
                    // We don't output an error in this case. TODO: Is this correct? Find what is the correct behavior in such a case
                    // Error($"Invalid character '{c}' found while parsing <! special tag");
                    GetNode<HtmlTextNode>().Text.Append("<!");
                }
            }
            else if (c == '/')
            {
                c = NextChar();
                if (IsAlpha(c))
                {
                    // Parse end-tag
                    TryProcessEndTag();
                }
                else
                {
                    // We don't output an error in this case. TODO: Is this correct? Find what is the correct behavior in such a case
                    // Error($"Invalid character '{c}' found while parsing </ end tag");
                    GetNode<HtmlTextNode>().Text.Append("</");
                }
            }
            else if (IsAlpha(c) || c == '?')
            {
                TryProcessStartTag();
            }
            else
            {
                // We don't output an error in this case. TODO: Is this correct? Find what is the correct behavior in such a case
                // Error($"Invalid character '{c}' found while parsing <");
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
                    Error(c == 0
                        ? $"Invalid EOF found while parsing <!DOCTYPE"
                        : $"Invalid character '{c}' found while parsing <!DOCTYPE");

                    GetNode<HtmlTextNode>().Text.Append(tag.Text);
                }
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing <!DOCTYPE"
                    : $"Invalid character '{c}' found while parsing <!DOCTYPE");

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
                Error(c == 0
                    ? $"Invalid EOF found while parsing <![CDATA["
                    : $"Invalid character '{c}' found while parsing <![CDATA[");

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
                // TODO: not entirely correct for <? as we should only test for Alpha for the first char
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

            var tag = new HtmlTagNode()
            {
                Name = nameBuilder.ToString().ToLowerInvariant(),
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
                        if (c == '>' && !isProcessingInstruction)
                        {
                            tag.IsClosed = true;
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

                // The content of SCRIPT and STYLE are considered as CDATA
                // and are expecting to mach either a </script> or </style>
                if (!tag.IsClosed && ((tag.Name == "script") || (tag.Name == "style")))
                {
                    ParseCDATATagContent(tag);
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

                GetNode<HtmlTextNode>().Text.Append('<');
                GetNode<HtmlTextNode>().Text.Append(builder);
            }
        }

        private void ParseCDATATagContent(HtmlTagNode tag)
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
                        temp.Clear();

                        c = NextChar();
                        bool tagFound = false;
                        if (TryParse("SCRIPT", false, temp))
                        {
                            tagFound = true;
                        }
                        else if (temp.Length == 1) // if script was partially match, we should have only the 's'
                        {
                            if (TryParse("TYLE", false, temp))
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

            var tagName = nameBuilder.ToString().ToLowerInvariant();

            if (c == '>')
            {
                var tag = new HtmlEndTagNode() {Name = tagName };
                node = tag;
                nodes.Add(tag);
                c = NextChar();
            }
            else
            {
                Error(c == 0
                    ? $"Invalid EOF found while parsing </{tagName}>"
                    : $"Invalid character '{c}' found while parsing </{tagName}>");

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

            var commentNode = new HtmlCommentNode();
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

                            node = commentNode;
                            nodes.Add(node);
                            return;
                        }

                        commentNode.Text.Append("--");
                        GetNode<HtmlTextNode>().Text.Append("<!--").Append(commentNode.Text);
                        Error(c == 0
                            ? $"Invalid EOF found while parsing <!--"
                            : $"Invalid character '{c}' found while parsing <!--");

                        return;
                    }
                    commentNode.Text.Append("-");
                }

                if (c == '\0')
                {
                    GetNode<HtmlTextNode>().Text.Append("<!--").Append(commentNode.Text);
                    Error($"Invalid EOF found while parsing comment");
                    return;
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
                File = sourceFileName,
                IsError = true,
                StartColumn = column + 1,
                EndColumn = column + 1,
                StartLine = line + 1,
                EndLine = line + 1,
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

            var nextChar = reader.Read();
            if (nextChar < 0)
            {
                isEof = true;
                nextChar = 0;
            }
            var nc = (char)nextChar;
            if (nc == '\n')
            {
                nextLine = true;
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
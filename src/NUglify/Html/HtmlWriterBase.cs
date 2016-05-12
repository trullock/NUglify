// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;

namespace NUglify.Html
{
    public abstract class HtmlWriterBase
    {
        protected HtmlWriterBase()
        {
        }

        public void Write(HtmlNode node)
        {
            if (node is HtmlText)
            {
                Write((HtmlText)node);
            }
            else if (node is HtmlElement)
            {
                if (node is HtmlDocument)
                {
                    WriteChildren(node);
                }
                else
                {
                    Write((HtmlElement) node);
                }
            }
            else if (node is HtmlRaw)
            {
                Write((HtmlRaw)node);
            }
            else if (node is HtmlCDATA)
            {
                Write((HtmlCDATA)node);
            }
            else if (node is HtmlComment)
            {
                Write((HtmlComment)node);
            }
            else if (node is HtmlDOCTYPE)
            {
                Write((HtmlDOCTYPE)node);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported tag node [{node?.GetType()}]");
            }
        }

        protected virtual void Write(HtmlElement node)
        {
            WriteStartTag(node);
            if (node.Kind == ElementKind.StartEnd)
            {
                WriteChildren(node);
                WriteEndTag(node);
            }
        }

        protected virtual void WriteStartTag(HtmlElement node)
        {
            Write("<");
            switch (node.Kind)
            {
                case ElementKind.InvalidEnd:
                    Write('/');
                    break;
                case ElementKind.ProcessingInstruction:
                    Write("?");
                    break;
            }
            Write(node.Name);

            if (node.Attributes != null)
            {
                foreach (var attribute in node.Attributes)
                {
                    Write(" ");
                    Write(attribute.Name);
                    if (attribute.Value != null)
                    {
                        Write("=");
                        Write("\"");
                        Write(attribute.Value);
                        Write("\"");
                    }
                }
            }

            if (node.Kind == ElementKind.ProcessingInstruction)
            {
                Write("?");
            }
            Write(">");
        }

        protected virtual void WriteEndTag(HtmlElement node)
        {
            Write("</");
            Write(node.Name);
            Write(">");
        }

        protected virtual void Write(HtmlText node)
        {
            Write(node.Slice.ToString());
        }

        protected virtual void Write(HtmlRaw node)
        {
            Write(node.Slice.ToString());
        }

        protected virtual void Write(HtmlCDATA node)
        {
            Write("<![CDATA[");
            Write(node.Slice.ToString());
            Write("]]>");
        }

        protected virtual void Write(HtmlComment node)
        {
            Write("<!--");
            Write(node.Slice.ToString());
            Write("-->");
        }

        protected virtual void Write(HtmlDOCTYPE node)
        {
            // It is parsed as a raw type
            Write(node.Slice.ToString());
        }

        protected abstract void Write(string text);

        protected abstract void Write(char c);

        protected virtual void WriteChildren(HtmlNode node)
        {
            foreach (var child in node.Children)
            {
                Write(child);
            }
        }
    }
}
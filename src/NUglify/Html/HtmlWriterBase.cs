// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;

namespace NUglify.Html
{
    public abstract class HtmlWriterBase
    {
        protected int XmlNamespaceLevel { get; private set; }

        protected HtmlWriterBase()
        {
        }
        protected int Depth { get; private set; }

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
            bool isInXml = node.Descriptor != null && (node.Descriptor.Category & ContentKind.Xml) != 0;
            var hasClosing = (node.Kind & ElementKind.Closing) != 0;

            var shouldClose = hasClosing ||
                                   (node.Descriptor != null && node.Descriptor.EndKind != TagEndKind.Required);

            if ((node.Kind & (ElementKind.Opening | ElementKind.SelfClosing | ElementKind.ProcessingInstruction)) != 0)
            {
                WriteStartTag(node);
                if (shouldClose)
                {
                    Depth++;
                }
            }

            if (isInXml)
            {
                XmlNamespaceLevel++;
            }

            WriteChildren(node);

            if (isInXml)
            {
                XmlNamespaceLevel--;
            }

            if (shouldClose)
            {
                Depth--;
            }

            if (hasClosing)
            {
                WriteEndTag(node);
            }
        }

        protected virtual void WriteStartTag(HtmlElement node)
        {
            Write("<");
            var isProcessing = (node.Kind & ElementKind.ProcessingInstruction) != 0;
            if (isProcessing)
            {
                Write("?");
            }
            Write(node.Name);

            if (node.Attributes != null)
            {
                var count = node.Attributes.Count;
                for (int i = 0; i < count; i++)
                {
                    var attribute = node.Attributes[i];
                    Write(" ");
                    WriteAttribute(node, attribute, i + 1 == count);
                }
            }

            if (isProcessing)
            {
                Write("?");
            }

            if ((node.Kind & ElementKind.SelfClosing) != 0 && XmlNamespaceLevel > 0)
            {
                Write(" /");
            }

            Write(">");
        }

        protected virtual void WriteEndTag(HtmlElement node)
        {
            Write("</");
            Write(node.Name);
            Write(">");
        }

        protected virtual void WriteAttribute(HtmlElement element, HtmlAttribute attribute, bool isLast)
        {
            Write(attribute.Name);
            if (attribute.Value != null)
            {
                Write("=");
                WriteAttributeValue(element, attribute, isLast);
            }
        }

        protected virtual void WriteAttributeValue(HtmlElement element, HtmlAttribute attribute, bool isLast)
        {
            Write("\"");
            Write(attribute.Value);
            Write("\"");
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
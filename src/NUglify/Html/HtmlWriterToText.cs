// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.IO;

namespace NUglify.Html
{
    /// <summary>
    /// Class responsible from extracting only text nodes from an HTML document, used by <see cref="Uglify.HtmlToText"/> function.
    /// </summary>
    public class HtmlWriterToText : HtmlWriterBase
    {
        private bool outputEnabled;
        private readonly HtmlToTextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlWriterToText"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">if writer is null</exception>
        public HtmlWriterToText(TextWriter writer, HtmlToTextOptions options)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            Writer = writer;
            this.options = options;
        }

        public TextWriter Writer { get; }

        private bool ShouldKeepStructure => (options & HtmlToTextOptions.KeepStructure) != 0;

        private bool ShouldKeepFormatting => (options & HtmlToTextOptions.KeepFormatting) != 0;

        private bool ShouldKeepHtmlEscape => (options & HtmlToTextOptions.KeepHtmlEscape) != 0;

        protected override void Write(string text)
        {
            if (outputEnabled)
            {
                if (!ShouldKeepStructure)
                {
                    text = text.Replace("\r\n", " ")
                        .Replace('\r', ' ')
                        .Replace('\n', ' ')
                        .Replace('\t', ' ')
                        .Replace('\f', ' ');
                }

                if (ShouldKeepFormatting || !ShouldKeepHtmlEscape)
                {
                    text = text.Replace("&lt;", "<");
                    text = text.Replace("&amp;", "&");
                }

                Writer.Write(text);
            }
        }

        protected override void Write(char c)
        {
            if (outputEnabled)
            {
                Writer.Write(ShouldKeepStructure ? c : c.IsSpace() ? ' ' : c);
            }
        }

        protected override void Write(HtmlCDATA node)
        {
        }

        protected override void Write(HtmlComment node)
        {
        }

        protected override void Write(HtmlDOCTYPE node)
        {
        }

        protected override void WriteStartTag(HtmlElement node)
        {
            if (node.Name == "body")
            {
                outputEnabled = true;
            }
            else if (ShouldKeepFormatting && node.Descriptor != null && (node.Descriptor.Category & ContentKind.Phrasing) != 0)
            {
                base.WriteStartTag(node);
            }
        }

        protected override void Write(HtmlRaw node)
        {
        }

        protected override void WriteEndTag(HtmlElement node)
        {
            if ((node.Descriptor == null || (node.Descriptor.Category & ContentKind.Phrasing) == 0 ||
                node.Name == "li"))
            {
                Write('\n');
            }

            if (node.Name == "body")
            {
                outputEnabled = false;
            }
            else if (ShouldKeepFormatting && node.Descriptor != null && (node.Descriptor.Category & ContentKind.Phrasing) != 0)
            {
                base.WriteEndTag(node);
            }
        }
    }
}
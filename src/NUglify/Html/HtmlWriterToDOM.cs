// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace NUglify.Html
{
    public class HtmlWriterToDOM : HtmlWriterBase
    {
        private readonly StringBuilder builder;
        private int level;

        public HtmlWriterToDOM()
        {
            this.builder = new StringBuilder();
            DOMDumpList = new List<string>();
        }

        public List<string> DOMDumpList { get; }
        
        protected override void WriteStartTag(HtmlElement node)
        {
            Start("[tag");
            base.WriteStartTag(node);
            FlushDOM();
        }

        protected override void WriteEndTag(HtmlElement node)
        {
            Start("]tag");
            base.WriteEndTag(node);
            FlushDOM();
        }

        protected override void Write(HtmlText node)
        {
            Start("#txt");
            base.Write(node);
            FlushDOM();
        }

        protected override void Write(HtmlRaw node)
        {
            Start("#raw");
            base.Write(node);
            FlushDOM();
        }

        protected override void Write(HtmlCDATA node)
        {
            Start("!cda");
            base.Write(node);
            FlushDOM();
        }

        protected override void Write(HtmlComment node)
        {
            Start("!com");
            base.Write(node);
            FlushDOM();
        }

        protected override void Write(HtmlDOCTYPE node)
        {
            Start("!doc");
            base.Write(node);
            FlushDOM();
        }

        protected void Start(string text)
        {
            builder.Clear();
            builder.Append($"[{level:0000}] ");
            builder.Append(text).Append(": ");
        }

        protected void FlushDOM()
        {
            DOMDumpList.Add(builder.ToString());
            builder.Clear();
        }

        protected override void WriteChildren(HtmlNode node)
        {
            level++;
            base.WriteChildren(node);
            level--;
        }

        protected override void Write(string text)
        {
            builder.Append(text);
        }

        protected override void Write(char c)
        {
            builder.Append(c);
        }
    }
}
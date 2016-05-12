// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.IO;

namespace NUglify.Html
{
    public class HtmlWriterToText : HtmlWriterBase
    {
        public HtmlWriterToText(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            Writer = writer;
        }


        public TextWriter Writer { get; }

        protected override void Write(string text)
        {
            Writer.Write(text);
        }

        protected override void Write(char c)
        {
            Writer.Write(c);
        }
    }
}
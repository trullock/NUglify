// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Collections.Generic;

namespace NUglify.Html
{
    public static class HtmlNodeExtensions
    {
        public static List<string> DumpDom(this HtmlNode node)
        {
            var domWriter = new HtmlWriterToDOM();
            domWriter.Write(node);
            return domWriter.DOMDumpList;
        }

        public static bool IsNonEmptyText(this HtmlNode node)
        {
            return node is HtmlText && !((HtmlText) node).Slice.IsEmptyOrWhiteSpace();
        }
    }
}
// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NUglify.Html
{
    // TODO: This is not yet a real minifier but just an integration test with HtmlParser
    public class HtmlMinifier
    {
        private readonly string html;
        private readonly string sourceFileName;
        private StringBuilder builder;
        private List<HtmlNode> nodes;
        private int noTextStrip;

        public HtmlMinifier(string html, string sourceFileName = null)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            this.html = html;
            this.sourceFileName = sourceFileName;
        }

        public UgliflyResult Minify()
        {

            var parser = new HtmlParser(html, sourceFileName);

            //nodes = parser.Parse();
            if (parser.HasErrors)
            {
                nodes = null;
                return new UgliflyResult(html, parser.Errors);
            }

            builder = new StringBuilder();
            ProcessNodes();

            return new UgliflyResult(builder.ToString(), null);
        }

        private void ProcessNodes()
        {
            //foreach (var node in nodes)
            //{
            //    var textNode = node as HtmlText;
            //    if (textNode != null)
            //    {
            //        if (noTextStrip > 0)
            //        {
            //            textNode.OutputTo(builder);
            //        }
            //        else
            //        {
            //            var text = textNode.Slice.ToString();
            //            if (!IsEmptyOrWhitespace(text))
            //            {
            //                builder.Append(text);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (node is HtmlDOCTYPE || node is HtmlCDATA)
            //        {
            //            node.OutputTo(builder);
            //        }
            //        else if (node is HtmlComment)
            //        {
            //            // SKIP
            //        }
            //        else if (node is HtmlTagNode)
            //        {
            //            node.OutputTo(builder);
            //            var tag = (HtmlTagNode) node;
            //            if (tag.Name == "pre" || tag.Name == "code")
            //            {
            //                noTextStrip++;
            //            }
            //        }
            //        else if (node is HtmlEndTagNode)
            //        {
            //            node.OutputTo(builder);
            //            var tag = (HtmlEndTagNode)node;
            //            if (tag.Name == "pre" || tag.Name == "code")
            //            {
            //                noTextStrip--;
            //                // Check unbalanced pre/code
            //            }
            //        }
            //    }
            //}
        }

        private static bool IsEmptyOrWhitespace(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
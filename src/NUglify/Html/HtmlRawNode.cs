// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// HTML base class for raw nodes.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public abstract class HtmlRawNode : HtmlNode
    {
        protected HtmlRawNode()
        {
            Text = new StringBuilder();
        }

        public StringBuilder Text { get; }
    }
}
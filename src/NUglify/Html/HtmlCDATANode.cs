// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// A HTML CDATA block
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlRawNode" />
    public class HtmlCDATANode : HtmlRawNode
    {
        public override string ToString()
        {
            return $"html-CDATA: <![CDATA[${Text}]]>";
        }

        public override void OutputTo(StringBuilder builder)
        {
            builder.Append("<![CDATA[").Append(Text).Append("]]>");
        }
    }
}
// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
namespace NUglify.Html
{
    /// <summary>
    /// An HTML text node
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlRawNode" />
    public class HtmlTextNode : HtmlRawNode
    {
        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
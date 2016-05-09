// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
namespace NUglify.Html
{
    /// <summary>
    /// A HTML closing tag.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public class HtmlEndTagNode : HtmlNode
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"html-end-tag: </{Name}>";
        }
    }
}
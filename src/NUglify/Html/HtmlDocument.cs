// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
namespace NUglify.Html
{
    /// <summary>
    /// A root document element.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlElement" />
    public class HtmlDocument : HtmlElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDocument"/> class.
        /// </summary>
        public HtmlDocument() : base("$root")
        {
        }
    }
}
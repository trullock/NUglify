// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// A DOCTYPE HTML tag.
    /// </summary>
    /// <seealso cref="HtmlTextBase" />
    public class HtmlDOCTYPE : HtmlTextBase
    {
        public override string ToString()
        {
            return $"html-DOCTYPE: {Slice}";
        }
    }
}
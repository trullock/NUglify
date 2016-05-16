// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// A HTML comment node.
    /// </summary>
    /// <seealso cref="HtmlTextBase" />
    public class HtmlComment : HtmlTextBase
    {
        public override string ToString()
        {
            return $"html-comment: <!--{Slice}-->";
        }
    }
}
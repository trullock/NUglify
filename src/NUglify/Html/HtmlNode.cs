// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// Base class for an HTML node.
    /// </summary>
    public abstract class HtmlNode
    {
        public abstract void OutputTo(StringBuilder builder);

    }
}
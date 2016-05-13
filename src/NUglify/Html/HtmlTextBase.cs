// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Helpers;

namespace NUglify.Html
{
    /// <summary>
    /// HTML base class for text nodes.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public abstract class HtmlTextBase : HtmlNode
    {
        public StringSlice Slice;
    }

    /// <summary>
    /// HTML raw content.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public class HtmlRaw : HtmlTextBase
    {
    }
}
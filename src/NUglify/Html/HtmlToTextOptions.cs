// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;

namespace NUglify.Html
{
    /// <summary>
    /// Settings for extracting text from an HTML document using the <see cref="Uglify.HtmlToText"/> function.
    /// </summary>
    [Flags]
    public enum HtmlToTextOptions
    {
        /// <summary>
        /// The extracted text will not contain structure (newlines), formatting (HTML tags) or Html escape (&amp;amp; for &amp;, &amp;lt; for &lt;)
        /// </summary>
        None = 0,

        /// <summary>
        /// Keeps the structure of the document by emitting newline when necessary
        /// </summary>
        KeepStructure = 1,

        /// <summary>
        /// Keeps HTML formatting tags (e.g &lt;em&gt;)
        /// </summary>
        KeepFormatting = 2,

        /// <summary>
        /// Keeps HTML escape for special characters (&amp;amp; for &amp;, &amp;lt; for &lt;)
        /// </summary>
        KeepHtmlEscape = 4
    }
}
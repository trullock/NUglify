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
        None = 0,

        KeepStructure = 1,

        KeepFormatting = 2,

        KeepHtmlEscape = 4
    }
}
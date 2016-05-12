// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
namespace NUglify.Html
{
    public enum ElementKind
    {
        /// <summary>
        /// An element with a start and end tag &lt;tag&gt;...&lt;/tag&gt;
        /// </summary>
        StartEnd,

        /// <summary>
        /// An element with a single end tag (invalid element) &lt;/tag&gt;
        /// </summary>
        InvalidEnd,

        /// <summary>
        /// The self closing element &lt;tag/&gt;
        /// </summary>
        SelfClosing,

        /// <summary>
        /// The XML ? processing instruction
        /// </summary>
        ProcessingInstruction,
    }
}
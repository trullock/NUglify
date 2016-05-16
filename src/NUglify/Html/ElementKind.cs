// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;

namespace NUglify.Html
{
    [Flags]
    public enum ElementKind
    {
        None = 0,

        /// <summary>
        /// Emit the opening tag of this element
        /// </summary>
        Opening = 1,

        /// <summary>
        /// Emit the closing tag of this element
        /// </summary>
        Closing = 2,

        OpeningClosing = Opening | Closing,

        /// <summary>
        /// Emit a self closing tag of this element (cannot be used with Start or Closing)
        /// </summary>
        SelfClosing = 4,

        /// <summary>
        /// The XML ? processing instruction
        /// </summary>
        ProcessingInstruction = 8,
    }
}
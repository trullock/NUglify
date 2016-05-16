// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System;

namespace NUglify.Html
{
    [Flags]
    public enum ContentKind
    {
        None = 0,
        Flow = 1 << 0,
        Sectioning = 1 << 1,
        Palpable = 1 << 2,
        Phrasing = 1 << 3,
        Metadata = 1 << 4,
        SectioningRoot = 1 << 5,
        Interactive = 1 << 6,

        Listed = 1 << 7,
        Labelable = 1 << 8,
        Submittable = 1 << 9,
        FormAssociated = 1 << 10,

        Embedded = 1 << 11,
        Transparent = 1 << 12,
        ScriptSupporting = 1 << 13,

        Resettable = 1 << 14,
        Heading = 1 << 15,

        ScriptText = 1 << 16,

        Text = 1 << 17,

        Xml = 1 << 18,


        Any = -1,
    }
}
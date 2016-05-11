// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Runtime.CompilerServices;

namespace NUglify.Html
{
    public static class CharHelper
    {
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsTagChar(this char c)
        {
            return IsAlphaNumeric(c);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool IsAlpha(this char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool IsAlphaUpper(this char c)
        {
            return (c >= 'A' && c <= 'Z');
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool IsAlphaNumeric(this char c)
        {
            return (c >= '0' && c <= '9') || IsAlpha(c);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool IsSpace(this char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\f' || c == '\n';
        }
    }
}
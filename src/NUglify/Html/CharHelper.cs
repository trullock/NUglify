// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Text;
using NUglify.Helpers;

namespace NUglify.Html
{
    public static class CharHelper
    {
        public static string CollapseWhitespaces(string text)
        {
            if (text == null)
            {
                return null;
            }

            StringBuilder sb = null;

            bool previousSpace = false;
            int previousIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c.IsSpace())
                {
                    if (previousSpace)
                    {
                        if (sb == null)
                        {
                            sb = StringBuilderPool.Acquire();
                        }
                        if (previousIndex < i)
                        {
                            sb.Append(text, previousIndex, i - previousIndex);
                        }
                        previousIndex = i + 1;
                    }
                    else if (c != ' ')
                    {
                        if (sb == null)
                        {
                            sb = StringBuilderPool.Acquire();
                        }
                        if (previousIndex < i)
                        {
                            sb.Append(text, previousIndex, i - previousIndex);
                        }
                        sb.Append(' ');
                        previousIndex = i + 1;
                    }

                    previousSpace = true;
                }
                else
                {
                    previousSpace = false;
                }
            }

            if (sb == null)
            {
                return text;
            }

            if (previousIndex < text.Length)
            {
                sb.Append(text, previousIndex, text.Length - previousIndex);
            }
            var result = sb.ToString();
            sb.Release();
            return result;
        }

        public static bool IsAttributeNameChar(this char c)
        {
            // Attribute names must consist of one or more characters other than 
            // the space characters, 
            // U +0000 NULL, 
            // U +0022 QUOTATION MARK ("), 
            // U +0027 APOSTROPHE ('), 
            // U +003E GREATER-THAN SIGN (>), 
            // U +002F SOLIDUS (/), 
            // and U+003D EQUALS SIGN (=) characters, 
            // the control characters, 
            // and any characters that are not defined by Unicode. 

            return !c.IsSpace() && c != 0 && c != '"' && c != '\'' && c != '>' && c != '/' && c != '=' &&
                   !char.IsControl(c);
        }

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
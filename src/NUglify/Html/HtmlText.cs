// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using NUglify.Helpers;

namespace NUglify.Html
{
    /// <summary>
    /// An HTML text node
    /// </summary>
    /// <seealso cref="HtmlTextBase" />
    public class HtmlText : HtmlTextBase
    {
        public HtmlText()
        {
        }

        public override string ToString()
        {
            return $"html-text: {Slice}";
        }

        public void Append(string text, int position, char c)
        {
            if (Slice.Text == null)
            {
                Slice = new StringSlice(text) {Start = position, End = position};
            }
            else
            {
                if (position != Slice.End + 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(position), $"Position [{position}] is not consecutive to the previous position [{Slice.End}]");
                }
                Slice.End = position;
            }
        }

        public void Append(string text, int from, int to)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (from < 0) throw new ArgumentOutOfRangeException(nameof(from), "From position cannot be null");

            // For to, limit them (this is a safeguard, should not happen, but we don't want to crash if this is the case) 
            if (to >= text.Length)
            {
                to = text.Length - 1;
            }

            if (Slice.Text == null)
            {
                Slice = new StringSlice(text) {Start = from, End = to};
            }
            else
            {
                if (from != Slice.End + 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(from),
                        $"Position [{from}] is not consecutive to the previous position [{Slice.End}]");
                }
                Slice.End = to;
            }
        }
    }
}
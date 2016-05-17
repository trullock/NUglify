// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using NUglify.Html;

namespace NUglify.Helpers
{
    public struct StringSlice
    {
        public StringSlice(string text) : this()
        {
            ChangeText(text);
        }

        public StringSlice(string text, int start, int end)
        {
            Text = text;
            Start = start;
            End = end;
        }

        public string Text { get; private set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Length => End - Start + 1;

        public int IndexOf(char c)
        {
            if (Text == null)
            {
                return -1;
            }
            for (int i = Start; i <= End; i++)
            {
                if (Text[i] == c)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool StartsWith(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            for (int i = 0; i < text.Length; i++)
            {
                var start = Start + i;
                if (start > End || Text[start] != text[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool StartsWithIgnoreCase(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            for (int i = 0; i < text.Length; i++)
            {
                var start = Start + i;
                if (start > End || char.ToLowerInvariant(Text[start]) != char.ToLowerInvariant(text[i]))
                {
                    return false;
                }
            }
            return true;
        }


        public bool EndsWith(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            for (int i = 0; i < text.Length; i++)
            {
                var start = End - i;
                if (start < Start || Text[start] != text[text.Length - i - 1])
                {
                    return false;
                }
            }
            return true;
        }



        public void ChangeText(string text)
        {
            Text = text;
            Start = text == null ? 1 : 0;
            End = text != null ? text.Length - 1 : 0;
        }

        public override string ToString()
        {
            return Text != null ? (Start <= End ? Text.Substring(Start, End - Start + 1) : string.Empty) : string.Empty;
        }

        public bool StartsBySpace()
        {
            return Start <= End && Text != null && Text[Start].IsSpace();
        }

        public void TrimStart()
        {
            if (Text == null)
            {
                return;
            }

            for (; Start <= End; Start++)
            {
                if (!Text[Start].IsSpace())
                {
                    break;
                }
            }
        }

        public void CollapseSpaces()
        {
            if (Text == null)
            {
                return;
            }

            // TODO: this method could be optimized by checking if 
            // it is really necessary to perform a StringBuilder
            // if there are no spaces to collapse
            var builder = StringBuilderPool.Acquire();
            bool previousWasSpace = false;
            for (int i = Start; i <= End; i++)
            {
                var c = Text[i];
                if (c.IsSpace())
                {
                    if (!previousWasSpace)
                    {
                        builder.Append(' ');
                    }
                    previousWasSpace = true;
                }
                else
                {
                    builder.Append(c);
                    previousWasSpace = false;
                }
            }

            ChangeText(builder.ToString());

            builder.Release();
        }

        public void TrimEnd()
        {
            if (Text == null)
            {
                return;
            }

            for (; End >= Start; End--)
            {
                if (!Text[End].IsSpace())
                {
                    break;
                }
            }
        }

        public bool HasTrailingSpaces()
        {
            if (Text == null)
            {
                return false;
            }
            return Start <= End && Text[End].IsSpace();
        }

        public bool IsEmpty()
        {
            if (Text == null)
            {
                return true;
            }

            return Start > End;
        }

        public bool IsEmptyOrWhiteSpace()
        {
            if (Text == null)
            {
                return true;
            }

            for (int i = Start; i <= End; i++)
            {
                if (!Text[i].IsSpace())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
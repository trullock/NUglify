// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

namespace NUglify.Html
{
    public struct StringSlice
    {
        public StringSlice(string text)
        {
            Text = text;
            Start = text == null ? 1 : 0;
            End = text != null ? text.Length - 1 : 0;
        }

        public StringSlice(string text, int start, int end)
        {
            Text = text;
            Start = start;
            End = end;
        }

        public string Text { get; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Length => End - Start + 1;

        public override string ToString()
        {
            return Text != null ? (Start <= End ? Text.Substring(Start, End - Start + 1) : string.Empty) : string.Empty;
        }
    }
}
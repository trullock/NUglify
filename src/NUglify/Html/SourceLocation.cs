// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

namespace NUglify.Html
{
    /// <summary>
    /// Location of a text in a HTML source file.
    /// </summary>
    public struct SourceLocation
    {
	    /// <summary>
        /// The source file name (may be null)
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// The absolute position from the beginning of the file
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// The line position (start at 1)
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// The character position (start at 1) (warning, does not count tabs)
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceLocation"/> struct.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="position">The position.</param>
        /// <param name="line">The line.</param>
        /// <param name="column">The column.</param>
        public SourceLocation(string file, int position, int line, int column)
        {
	        File = file;
	        Position = position;
	        Line = line;
	        Column = column;
        }
    }
}
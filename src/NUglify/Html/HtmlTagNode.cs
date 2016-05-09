// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System.Collections.Generic;
using System.Text;

namespace NUglify.Html
{
    /// <summary>
    /// A HTML open tag (or processing instruction if <see cref="IsProcessingInstruction"/> is set.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public class HtmlTagNode : HtmlNode
    {
        public HtmlTagNode()
        {
            Attributes = new List<HtmlAttribute>();
        }

        public string Name { get; set; }

        public bool IsProcessingInstruction { get; set; }

        public bool IsClosed { get; set; }

        public List<HtmlAttribute> Attributes { get; }

        public string Content { get; set; }

        public override string ToString()
        {
            return $"<{(IsProcessingInstruction ? "?" : string.Empty)}{Name}{(Attributes.Count > 0?" ..." : string.Empty)}>";
        }
    }
}
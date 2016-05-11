// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;

namespace NUglify.Html
{
    /// <summary>
    /// A HTML open tag (or processing instruction if <see cref="IsProcessingInstruction"/> is set.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public class HtmlElement : HtmlNode
    {
        public HtmlElement()
        {
            Attributes = null;
        }

        public string Name { get; set; }

        public bool IsProcessingInstruction { get; set; }

        public bool IsClosed { get; set; }

        public List<HtmlAttribute> Attributes { get; set; }

        public void AddAttribute(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (Attributes == null)
            {
                Attributes = new List<HtmlAttribute>();
            }
            Attributes.Add(new HtmlAttribute(name, value));
        }

        public override string ToString()
        {
            return $"html-tag: <{(IsProcessingInstruction ? "?" : string.Empty)}{Name}{(Attributes.Count > 0?" ..." : string.Empty)}>";
        }
    }
}
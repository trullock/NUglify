// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NUglify.Html
{
    /// <summary>
    /// A HTML/XML open/close/self-closing tag.
    /// </summary>
    /// <seealso cref="NUglify.Html.HtmlNode" />
    public class HtmlElement : HtmlNode
    {
        public HtmlElement(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("A tag name cannot be null or empty", nameof(name));
            Name = name;
            Attributes = null;
        }

        public string Name { get; set; }

        public HtmlTagDescriptor Descriptor { get; set; }

        public ElementKind Kind { get; set; }

        public List<HtmlAttribute> Attributes { get; set; }

        public HtmlAttribute FindAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (Attributes == null)
            {
                return null;
            }
            foreach (var attr in Attributes)
            {
                if (name.Equals(attr.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return attr;
                }
            }
            return null;
        }

        public void RemoveAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (Attributes == null)
            {
                return;
            }
            for (int i = Attributes.Count - 1; i >= 0; i--)
            {
                if (name.Equals(Attributes[i].Name, StringComparison.OrdinalIgnoreCase))
                {
                    Attributes.RemoveAt(i);
                }
            }
        }


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
            return $"html-tag: <{(Kind == ElementKind.ProcessingInstruction ? "?" : string.Empty)}{Name}{(Attributes != null && Attributes.Count > 0?" ..." : string.Empty)}>";
        }
    }
}
// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
namespace NUglify.Html
{
    /// <summary>
    /// A HTML attribute.
    /// </summary>
    public class HtmlAttribute
    {
        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value != null ? $"{Name}={Value}" : Name;
        }
    }
}
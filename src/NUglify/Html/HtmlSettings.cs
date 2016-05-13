// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Collections.Generic;
using NUglify.Css;
using NUglify.Helpers;
using NUglify.JavaScript;

namespace NUglify.Html
{
    /// <summary>
    /// Settings for Html minification.
    /// </summary>
    public class HtmlSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlSettings"/> class.
        /// </summary>
        public HtmlSettings()
        {
            AttributesCaseSensitive = false;
            CollapseWhitespaces = true;
            RemoveComments = true;
            RemoveOptionalTags = true;
            RemoveInvalidClosingTags = true;
            RemoveEmptyAttributes = true;
            MinifyJs = true;
            JsSettings = new CodeSettings();
            MinifyCss = true;
            CssSettings = new CssSettings();

            InlineTagsPreservingSpacesAround = new[]
            {
                "a",
                "abbr",
                "acronym",
                "b",
                "bdi",
                "bdo",
                "big",
                "button",
                "cite",
                "code",
                "del",
                "dfn",
                "em",
                "font",
                "i",
                "ins",
                "kbd",
                "label",
                "mark",
                "math",
                "nobr",
                "q",
                "rp",
                "rt",
                "s",
                "samp",
                "small",
                "span",
                "strike",
                "strong",
                "sub",
                "sup",
                "svg",
                "time",
                "tt",
                "u",
                "var",
            }.ToDictionaryBool(false);

            TagsWithNonCollapsableWhitespaces = new[]
            {
                "pre",
                "textarea",
            }.ToDictionaryBool(false);

        }

        /// <summary>
        /// Gets or sets a value indicating whether to treat attributes as case sensitive. Default is <c>false</c>
        /// </summary>
        public bool AttributesCaseSensitive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to collapse whitespaces. Default is <c>true</c>
        /// </summary>
        public bool CollapseWhitespaces { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to remove comments. Default is <c>true</c>
        /// </summary>
        public bool RemoveComments { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to remove optional tags (e.g: &lt;/p&gt; or &lt;/li&gt;). Default is <c>true</c>
        /// </summary>
        public bool RemoveOptionalTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove invalid closing tags (tags with only a end tag and a missing start tag)
        /// </summary>
        public bool RemoveInvalidClosingTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove empty attributes with whitespace only characters.
        /// </summary>
        public bool RemoveEmptyAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether minify js inside &lt;script&gt; tags. Default is <c>true</c> using <see cref="Uglify.Js(string,NUglify.JavaScript.CodeSettings)"/>
        /// </summary>
        public bool MinifyJs { get; set; }

        /// <summary>
        /// Gets or sets the minify js settings.
        /// </summary>
        public JavaScript.CodeSettings JsSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether minify css inside &lt;style&gt; tags. Default is <c>true</c> using <see cref="Uglify.Css(string,NUglify.Css.CssSettings,NUglify.JavaScript.CodeSettings)"/>
        /// </summary>
        public bool MinifyCss { get; set; }

        /// <summary>
        /// Gets or sets the minify css settings.
        /// </summary>
        public Css.CssSettings CssSettings{ get; set; }

        /// <summary>
        /// Gets the inline tags preserving spaces around (default: a, abbr, acronym, b, bdi, 
        /// bdo, big, button, cite, code, del, dfn, em, font, i, ins, kbd, label, 
        /// mark, math, nobr, q, rp, rt, s, samp, small, span, strike, strong, sub, sup, 
        /// svg, time, tt, u, var)
        /// </summary>
        public Dictionary<string, bool> InlineTagsPreservingSpacesAround { get; }


        /// <summary>
        /// Gets the tags with non collapsable whitespaces (default: pre, textarea)
        /// </summary>
        public Dictionary<string, bool> TagsWithNonCollapsableWhitespaces { get; }
    }
}
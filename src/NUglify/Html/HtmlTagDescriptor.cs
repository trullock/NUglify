// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using NUglify.Helpers;

namespace NUglify.Html
{
    public class HtmlTagDescriptor
    {
        public delegate bool AcceptContentDelegate(HtmlElement parent, HtmlElement child, ref HtmlTagDescriptor childDescriptor);

        /// <summary>
        /// A delegate to check whether a end tag can be omitted.
        /// </summary>
        /// <param name="tag">The tag for which we want to test if a end tag can be omitted.</param>
        /// <param name="nextSibling">The next sibling or null if it is the last tag of its parent.</param>
        /// <returns><c>true</c> if the end tag can be omitted; <c>false</c> otherwise</returns>
        public delegate bool CanOmitEndTagDelegate(HtmlElement tag, HtmlElement nextSibling);

        public HtmlTagDescriptor(ContentKind category, string[] acceptTags, ContentKind acceptContent = ContentKind.None, TagEndKind endKind = TagEndKind.Required, CanOmitEndTagDelegate canOmitEndTag = null)
        {
            Category = category;
            AcceptContent = acceptContent;
            AcceptContentTags = acceptTags?.ToDictionaryBool(false);
            EndKind = endKind;
            CanOmitEndTag = canOmitEndTag;
        }

        public HtmlTagDescriptor(ContentKind category, ContentKind acceptContent, TagEndKind endKind = TagEndKind.Required, CanOmitEndTagDelegate canOmitEndTag = null)
        {
            Category = category;
            AcceptContent = acceptContent;
            EndKind = endKind;
            CanOmitEndTag = canOmitEndTag;
        }

        public ContentKind Category { get; }

        public ContentKind AcceptContent { get; }

        public Dictionary<string, bool> AcceptContentTags { get; }

        public TagEndKind EndKind { get; }

        public CanOmitEndTagDelegate CanOmitEndTag { get; }

        public bool TryAcceptContent(HtmlElement parent, HtmlTagDescriptor parentDescriptor, ContentKind parentAcceptContentType, HtmlElement child, HtmlTagDescriptor childDescriptor)
        {
            if (parentDescriptor == null || childDescriptor == null)
            {
                return true;
            }

            return (childDescriptor.Category & parentAcceptContentType) != 0
                   ||
                   (parentDescriptor.AcceptContentTags != null &&
                    parentDescriptor.AcceptContentTags.ContainsKey(child.Name));
        }

        public static HtmlTagDescriptor Find(string tag)
		{
			HtmlTagDescriptor tagDesc;
			TagDescriptors.TryGetValue(tag, out tagDesc);
			return tagDesc;
		}

        private static readonly Dictionary<string, HtmlTagDescriptor> TagDescriptors = new Dictionary<string, HtmlTagDescriptor>(StringComparer.OrdinalIgnoreCase)
        {
            ["a"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Palpable, ContentKind.Transparent),
            ["abbr"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["address"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["area"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["article"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Sectioning | ContentKind.Palpable, ContentKind.Flow),
            ["aside"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Sectioning | ContentKind.Palpable, ContentKind.Flow),
            ["audio"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.Palpable, new[] { "source", "track" }, ContentKind.Transparent),
            ["b"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["base"] = new HtmlTagDescriptor(ContentKind.Metadata, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["bdi"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["bdo"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["blockquote"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.SectioningRoot | ContentKind.Palpable, ContentKind.Flow),
            ["body"] = new HtmlTagDescriptor(ContentKind.SectioningRoot, ContentKind.Flow),
            ["br"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["button"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Listed | ContentKind.Labelable | ContentKind.Submittable | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.Phrasing),
            ["canvas"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Palpable, ContentKind.Transparent),
            ["caption"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Flow, TagEndKind.Omission, CanOmitIfFollowedByCommentOrSpace),
            ["cite"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["code"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["col"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["colgroup"] = new HtmlTagDescriptor(ContentKind.None, new[] { "col", "template" }, ContentKind.None, TagEndKind.Omission, CanOmitIfFollowedByCommentOrSpace),
            ["data"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["datalist"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, new[] { "option" }, ContentKind.Phrasing | ContentKind.ScriptSupporting),
            ["dd"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Flow, TagEndKind.Omission, DefinitionDescriptionEndTagOmissionHandler),
            ["del"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, ContentKind.Transparent),
            ["details"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.SectioningRoot | ContentKind.Interactive | ContentKind.Palpable, new[] { "summary" }, ContentKind.Flow),
            ["dfn"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["dialog"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.SectioningRoot, ContentKind.Flow),
            ["div"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["dl"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, new[] { "dt", "dd" }, ContentKind.ScriptSupporting),
            ["dt"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Flow, TagEndKind.Omission, DefinitionDescriptionEndTagOmissionHandler),
            ["em"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["embed"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.Palpable, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["fieldset"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.SectioningRoot | ContentKind.Listed | ContentKind.FormAssociated | ContentKind.Palpable, new[] { "legend" }, ContentKind.Flow),
            ["figcaption"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Flow),
            ["figure"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.SectioningRoot | ContentKind.Palpable, new[] { "figcaption" }, ContentKind.Flow),
            ["footer"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["form"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["h1"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["h2"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["h3"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["h4"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["h5"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["h6"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, ContentKind.Phrasing),
            ["head"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Metadata),
            ["header"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["hgroup"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Heading | ContentKind.Palpable, new[] { "h1", "h2", "h3", "h4", "h5", "h6", "template"}),
            ["hr"] = new HtmlTagDescriptor(ContentKind.Flow, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["html"] = new HtmlTagDescriptor(ContentKind.None, new [] { "head", "body"}),
            ["i"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["iframe"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.Palpable, ContentKind.Text),
            ["img"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["input"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Listed | ContentKind.Labelable | ContentKind.Submittable | ContentKind.Resettable | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["ins"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Transparent),
            ["kbd"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["keygen"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Listed | ContentKind.Labelable | ContentKind.Submittable | ContentKind.Resettable | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["label"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Palpable, ContentKind.Phrasing),
            ["legend"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Phrasing),
            ["li"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Flow, TagEndKind.Omission, ListItemEndTagOmissionHandler),
            ["link"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow | ContentKind.Phrasing, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["main"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Flow),
            ["map"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, new [] {"area"}, ContentKind.Transparent),
            ["mark"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["math"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Palpable, ContentKind.Any),
            ["menu"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, new[] { "li", "menuitem", "hr", "menu" }, ContentKind.Flow | ContentKind.ScriptSupporting),
            ["menuitem"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Text, TagEndKind.Omission, MenuItemTagOmissionHandler),
            ["meta"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow | ContentKind.Phrasing, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["meter"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Labelable | ContentKind.Palpable, ContentKind.Phrasing),
            ["nav"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Sectioning | ContentKind.Palpable, ContentKind.Flow),
            ["noscript"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow | ContentKind.Phrasing, ContentKind.Any),
            ["object"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.Listed | ContentKind.Submittable | ContentKind.FormAssociated | ContentKind.Palpable, new[] { "param" }, ContentKind.Transparent),
            ["ol"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, new[] { "li" }, ContentKind.ScriptSupporting),
            ["optgroup"] = new HtmlTagDescriptor(ContentKind.None, new[] { "option" }, ContentKind.ScriptSupporting, TagEndKind.Omission, OptGroupEndTagOmissionHandler),
            ["option"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Text, TagEndKind.Omission, OptionEndTagOmissionHandler),
            ["output"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Listed | ContentKind.Labelable | ContentKind.Resettable | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.Phrasing),
            ["p"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Phrasing, TagEndKind.Omission, ParagraphEndTagOmissionHandler),
            ["param"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["picture"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded, new[] { "source", "img" }, ContentKind.ScriptSupporting),
            ["pre"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, ContentKind.Phrasing),
            ["progress"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Labelable | ContentKind.Palpable, ContentKind.Phrasing),
            ["q"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["rp"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Phrasing, TagEndKind.Omission, RubyEndTagOmissionHandler),
            ["rt"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Phrasing, TagEndKind.Omission, RubyEndTagOmissionHandler),
            ["ruby"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, new[] { "rt", "rp" }, ContentKind.Phrasing),
            ["s"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["samp"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["script"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow | ContentKind.Phrasing | ContentKind.ScriptSupporting, ContentKind.ScriptText),
            ["section"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Sectioning | ContentKind.Palpable, ContentKind.Flow),
            ["select"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Listed | ContentKind.Labelable | ContentKind.Submittable | ContentKind.Resettable | ContentKind.FormAssociated | ContentKind.Palpable, new[] { "option", "optgroup" }, ContentKind.ScriptSupporting),
            ["slot"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, ContentKind.Transparent),
            ["small"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["source"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["span"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["strong"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["style"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow, ContentKind.ScriptText),
            ["sub"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["summary"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.Phrasing),
            ["sup"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["svg"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Palpable, ContentKind.Any),
            ["table"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, new[] { "caption", "colgroup", "thead", "tbody", "tfoot", "tr"}, ContentKind.ScriptSupporting),
            ["tbody"] = new HtmlTagDescriptor(ContentKind.None, new[] {"tr"}, ContentKind.ScriptSupporting, TagEndKind.Omission, THeadTBodyTagOmissionHandler),
            ["td"] = new HtmlTagDescriptor(ContentKind.SectioningRoot, ContentKind.Flow, TagEndKind.Omission, TDTHTagOmissionHandler),
            ["template"] = new HtmlTagDescriptor(ContentKind.Metadata | ContentKind.Flow | ContentKind.Phrasing | ContentKind.ScriptSupporting, ContentKind.Any),
            ["textarea"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Interactive | ContentKind.Listed | ContentKind.Labelable | ContentKind.Submittable | ContentKind.Resettable | ContentKind.FormAssociated | ContentKind.Palpable, ContentKind.Text),
            ["tfoot"] = new HtmlTagDescriptor(ContentKind.None, new[] {"tr"}, ContentKind.ScriptSupporting, TagEndKind.Omission, CanOmitIfFollowedByCommentOrSpace),
            ["th"] = new HtmlTagDescriptor(ContentKind.Interactive, ContentKind.Flow, TagEndKind.Omission, TDTHTagOmissionHandler),
            ["thead"] = new HtmlTagDescriptor(ContentKind.None, new[] {"tr"}, ContentKind.ScriptSupporting, TagEndKind.Omission, THeadTBodyTagOmissionHandler),
            ["time"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["title"] = new HtmlTagDescriptor(ContentKind.Metadata, ContentKind.Text),
            ["tr"] = new HtmlTagDescriptor(ContentKind.None, new[] {"th", "td"}, ContentKind.ScriptSupporting, TagEndKind.Omission, TableRowEndTagOmissionHandler),
            ["track"] = new HtmlTagDescriptor(ContentKind.None, ContentKind.None, TagEndKind.AutoSelfClosing),
            ["u"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["ul"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Palpable, new[] {"li"}, ContentKind.ScriptSupporting),
            ["var"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Palpable, ContentKind.Phrasing),
            ["video"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing | ContentKind.Embedded | ContentKind.Interactive | ContentKind.Palpable, new[] {"source", "track"}, ContentKind.Transparent),
            ["wbr"] = new HtmlTagDescriptor(ContentKind.Flow | ContentKind.Phrasing, ContentKind.None, TagEndKind.AutoSelfClosing),
        };

        private static readonly Dictionary<string, bool> OpenTagsClosingParagraph = new string[]
        {
            "address",
            "article",
            "aside",
            "blockquote",
            "details",
            "div",
            "dl",
            "fieldset",
            "figcaption",
            "figure",
            "footer",
            "form",
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
            "header",
            "hgroup",
            "hr",
            "main",
            "menu",
            "nav",
            "ol",
            "p",
            "pre",
            "section",
            "table",
            "ul",
        }.ToDictionaryBool(false);

        private static readonly Dictionary<string, bool> ParentTagsClosingParagraph = new string[]
        {
            "a",
            "audio",
            "del",
            "ins",
            "map",
            "noscript",
            "video"
        }.ToDictionaryBool(false);

        private static bool ParagraphEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            // it is a closing nextSibling that will close this parent, so we allow it
            return nextSibling != null
                ? OpenTagsClosingParagraph.ContainsKey(nextSibling.Name)
                : !ParentTagsClosingParagraph.ContainsKey(parent.Parent.Name);
        }

        private static bool ListItemEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || nextSibling.Name.Equals("li", StringComparison.OrdinalIgnoreCase);
        }

        private static bool DefinitionDescriptionEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("dt", StringComparison.OrdinalIgnoreCase) 
                || nextSibling.Name.Equals("dd", StringComparison.OrdinalIgnoreCase));
        }

        private static bool RubyEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("rt", StringComparison.OrdinalIgnoreCase) 
                || nextSibling.Name.Equals("rp", StringComparison.OrdinalIgnoreCase));
        }

        private static bool CanOmitIfFollowedByCommentOrSpace(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (!(parent.FirstChild is HtmlComment || (parent.FirstChild is HtmlText && ((HtmlText)parent.FirstChild).Slice.StartsBySpace())));
        }

        private static bool THeadTBodyTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("tbody", StringComparison.OrdinalIgnoreCase) 
                || nextSibling.Name.Equals("tfoot", StringComparison.OrdinalIgnoreCase));
        }

        private static bool TableRowEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || nextSibling.Name.Equals("tr", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TDTHTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("td", StringComparison.OrdinalIgnoreCase) 
                || nextSibling.Name.Equals("th", StringComparison.OrdinalIgnoreCase));
        }

        private static bool OptGroupEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || nextSibling.Name.Equals("optgroup", StringComparison.OrdinalIgnoreCase);
        }

        private static bool OptionEndTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("optgroup", StringComparison.OrdinalIgnoreCase)
                || nextSibling.Name.Equals("option", StringComparison.OrdinalIgnoreCase));
        }

        private static bool MenuItemTagOmissionHandler(HtmlElement parent, HtmlElement nextSibling)
        {
            return nextSibling == null || (nextSibling.Name.Equals("menuitem", StringComparison.OrdinalIgnoreCase)
                || nextSibling.Name.Equals("hr", StringComparison.OrdinalIgnoreCase)
                || nextSibling.Name.Equals("menu", StringComparison.OrdinalIgnoreCase));
        }
    }
}
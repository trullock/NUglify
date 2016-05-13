// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using NUglify.Helpers;

namespace NUglify.Html
{
    public class HtmlMinifier
    {
        private static readonly HtmlSettings DefaultSettings = new HtmlSettings();

        private readonly HtmlDocument html;
        private int pendingTagNonCollapsibleWithSpaces;
        private readonly List<HtmlText> pendingTexts;
        private readonly HtmlSettings settings;

        public HtmlMinifier(HtmlDocument html, HtmlSettings settings = null)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            this.settings = settings ?? DefaultSettings;
            this.html = html;
            pendingTexts = new List<HtmlText>();
            Errors = new List<UglifyError>();
        }

        public bool HasErrors { get; private set; }

        public List<UglifyError> Errors { get; private set; }

        public void Minify()
        {
            ProcessChildren(html);
            TrimPendingTextNodes();
        }

        private void ProcessNode(HtmlNode node)
        {
            TrimNodeOnStart(node);
            
            var element = node as HtmlElement;
            bool isContentNonCollapsible = false;
            if (element != null && settings.TagsWithNonCollapsableWhitespaces.ContainsKey(element.Name))
            {
                pendingTagNonCollapsibleWithSpaces++;
                isContentNonCollapsible = true;
            }

            ProcessChildren(node);
            
            TrimNodeOnEnd(node);

            if (isContentNonCollapsible)
            {
                pendingTagNonCollapsibleWithSpaces--;
            }
        }
        private void ProcessChildren(HtmlNode node)
        {
            foreach (var subNode in node.Children)
            {
                ProcessNode(subNode);
            }
        }

        private void TrimNodeOnStart(HtmlNode node)
        {
            var textNode = node as HtmlText;
            if (textNode != null)
            {
                TrimNodeOnStart(textNode);
            }

            if (node is HtmlComment && settings.RemoveComments)
            {
                var comment = (HtmlComment) node;
                // Don't remove special ignoring comments
                if (!comment.Slice.StartsWith("!"))
                {
                    node.Remove();
                }
            }
        }

        private void TrimNodeOnEnd(HtmlNode node)
        {
            if (node is HtmlElement)
            {
                TrimNodeOnEnd((HtmlElement)node);
            }
        }

        private void TrimNodeOnStart(HtmlText textNode)
        {
            // If we don't do anything for TextNode, we can early exit
            if (!settings.CollapseWhitespaces)
            {
                return;
            }

            pendingTexts.Add(textNode);
        }

        private void TrimNodeOnEnd(HtmlElement element)
        {
            // If the element is a valid HTML descriptor, we can safely turn-it all lowercase
            element.Name = element.Name.ToLowerInvariant();

            // If the element being visited is not an inline tag, we need to clear the previous text node
            if (settings.CollapseWhitespaces && !settings.InlineTagsPreservingSpacesAround.ContainsKey(element.Name))
            {
                TrimPendingTextNodes();
            }

            // Remove optional tags
            if (settings.RemoveOptionalTags)
            {
                var nextElement = element.FindNextSibling<HtmlElement>();
                var canOmitEndTag = element.Descriptor?.CanOmitEndTag;
                if (element.Kind == ElementKind.StartWithEnd && canOmitEndTag != null && canOmitEndTag(element, nextElement))
                {
                    element.Kind = ElementKind.StartWithoutEnd;
                }
            }

            // Remove invalid closing tags
            if (settings.RemoveInvalidClosingTags && element.Kind == ElementKind.EndWithoutStart)
            {
                element.Remove();
            }

            if (element.Attributes != null)
            {
                for (int i = element.Attributes.Count - 1; i >= 0; i--)
                {
                    var attribute = element.Attributes[i];
                    if (TrimAttribute(element, attribute))
                    {
                        element.Attributes.RemoveAt(i);
                    }
                }
            }

            var isJavaScript = IsJavaScript(element);
            var isCssScript = !isJavaScript && IsCssStyle(element);
            if (isJavaScript || isCssScript)
            {
                TrimScriptOrStyle(element, isJavaScript);
            }
        }

        private void TrimPendingTextNodes()
        {
            HtmlText previousTextNode = null;
            for (int i = 0; i < pendingTexts.Count; i++)
            {
                var textNode = pendingTexts[i];

                // We can trim the heading whitespaces if:
                // - we don't have a previous element (either inline or parent container)
                // - OR the previous element (sibling or parent) is not a tag that require preserving spaces around
                // - OR the previous text node has already some trailing spaces
                if (previousTextNode == null || previousTextNode.Slice.HasTrailingSpaces())
                {
                    textNode.Slice.TrimStart();
                }

                // We can trim the traling whitespaces if:
                // - we don't have a next element (either inline or parent container)
                // - OR the next element (sibling or parent) is not a tag that require preserving spaces around
                if (previousTextNode != null && textNode.NextSibling == null && (i+1 >= pendingTexts.Count || pendingTexts[i+1].Slice.StartsBySpace()))
                {
                    textNode.Slice.TrimEnd();
                }

                // If we are not in the context of a tag that doesn't accept to collapse whitespaces, 
                // we can collapse them for this text node
                if (pendingTagNonCollapsibleWithSpaces == 0)
                {
                    textNode.Slice.CollapseSpaces();
                }

                // If the text node is empty, remove it from the tree
                if (textNode.Slice.IsEmptyOrWhiteSpace())
                {
                    textNode.Remove();
                }
                else
                {
                    // Replace the previous textnode
                    previousTextNode = textNode;
                }
            }

            // Trim any trailing spaces of the last known text node if we are moving to a block level
            if (previousTextNode != null)
            {
                previousTextNode.Slice.TrimEnd();
            }

            pendingTexts.Clear();
        }
        private void TrimScriptOrStyle(HtmlElement element, bool isJs)
        {
            if ((isJs && !settings.MinifyJs) || (!isJs && !settings.MinifyCss))
            {
                return;
            }

            var raw = element.FirstChild as HtmlRaw;
            if (raw == null)
            {
                return;
            }

            var slice = raw.Slice;

            slice.TrimStart();
            slice.TrimEnd();

            // If the text has a comment or CDATA, we won't try to minify it
            if (slice.StartsWith("<!--") || slice.StartsWithIgnoreCase("<![CDATA["))
            {
                return;
            }

            var text = slice.ToString();

            var result = isJs ?
                Uglify.Js(text, "inner_js", settings.JsSettings)
                : Uglify.Css(text, "inner_css", settings.CssSettings);

            if (result.Errors != null)
            {
                Errors.AddRange(result.Errors);
            }

            if (result.HasErrors)
            {
                HasErrors = true;
                return;
            }

            // We remove the type attribute, as it default to text/css and text/javascript
            element.RemoveAttribute("type");

            raw.Slice = new StringSlice(result.Code);
        }

        private bool TrimAttribute(HtmlElement element, HtmlAttribute attribute)
        {
            if (settings.RemoveEmptyAttributes)
            {
                if (attribute.Value != null && attribute.Value.IsNullOrWhiteSpace())
                {
                    attribute.Value = string.Empty;
                }
            }

            if (!settings.AttributesCaseSensitive)
            {
                attribute.Name = attribute.Name.ToLowerInvariant();
            }

            return false;
        }

        private static bool IsJavaScript(HtmlElement element)
        {
            if (!element.Name.Equals("script", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return IsAttributeValueJs(element.FindAttribute("type")?.Value);
        }

        private static bool IsCssStyle(HtmlElement element)
        {
            if (!element.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return IsAttributeValueCss(element.FindAttribute("type")?.Value);
        }

        private static bool IsAttributeValueCss(string value)
        {
            return string.IsNullOrEmpty(value) || value.Equals("text/css", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAttributeValueJs(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var text = value.Split(';')[0].ToLowerInvariant();
            switch (text)
            {
                case "text/javascript":
                case "text/ecmascript":
                case "text/jscript":
                case "application/javascript":
                case "application/x-javascript":
                case "application/ecmascript":
                    return true;
            }
            return false;
        }
    }
}
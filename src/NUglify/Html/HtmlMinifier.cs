// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using NUglify.Css;
using NUglify.Helpers;

namespace NUglify.Html
{
    public class HtmlMinifier
    {
        private readonly HtmlDocument html;
        private int pendingTagNonCollapsibleWithSpaces;
        private readonly List<HtmlText> pendingTexts;
        private readonly HtmlSettings settings;
        private readonly CssSettings attributeCssSettings;
        private int xmlNamespaceCount;

        private static readonly Dictionary<string, bool> AttributesRemovedIfEmpty = new[]
        {
            "class",
            "id",
            "style",
            "title",
            "lang",
            "dir",
            "onfocus",
            "onblur",
            "onchange",
            "onclick",
            "ondblclick",
            "onmousedown",
            "onmouseup",
            "onmouseover",
            "onmousemove",
            "onmouseout",
            "onkeypress",
            "onkeydown",
            "onkeyup",
        }.ToDictionaryBool(false);

        private static readonly Dictionary<string, bool> ScriptAttributes = new string[]
        {
            "onAbort",
            "onBlur",
            "onChange",
            "onClick",
            "onDblClick",
            "onDragDrop",
            "onError",
            "onFocus",
            "onKeyDown",
            "onKeyPress",
            "onKeyUp",
            "onLoad",
            "onMouseDown",
            "onMouseMove",
            "onMouseOut",
            "onMouseOver",
            "onMouseUp",
            "onMove",
            "onReset",
            "onResize",
            "onSelect",
            "onSubmit",
            "onUnload",
        }.ToDictionaryBool(false);

        public HtmlMinifier(HtmlDocument html, HtmlSettings settings = null)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            this.settings = settings ?? new HtmlSettings();
            attributeCssSettings = this.settings.CssSettings.Clone();
            attributeCssSettings.CssType = CssType.DeclarationList;

            this.html = html;
            pendingTexts = new List<HtmlText>();
            Errors = new List<UglifyError>();
        }

        public bool HasErrors { get; private set; }

        public List<UglifyError> Errors { get; private set; }

        public void Minify()
        {
            ProcessChildren(html);
            if (settings.CollapseWhitespaces)
            {
                TrimPendingTextNodes();
            }
        }

        private void ProcessNode(HtmlNode node)
        {
            var element = node as HtmlElement;
            bool isInXml = element?.Descriptor != null && (element.Descriptor.Category & ContentKind.Xml) != 0;
            if (isInXml)
            {
                xmlNamespaceCount++;
            }

            TrimNodeOnStart(node);
            
            bool isContentNonCollapsible = false;
            if (element != null && settings.TagsWithNonCollapsableWhitespaces.ContainsKey(element.Name))
            {
                pendingTagNonCollapsibleWithSpaces++;
                isContentNonCollapsible = true;
            }

            ProcessChildren(node);

            TrimNodeOnEnd(node);

            if (isInXml)
            {
                xmlNamespaceCount--;
            }

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
                bool keepComment = false;
                foreach (var regex in settings.KeepCommentsRegex)
                {
                    var result = regex.Match(comment.Slice.Text, comment.Slice.Start, comment.Slice.Length);
                    if (result.Success)
                    {
                        keepComment = true;
                        break;
                    }
                }

                // Don't remove special ignoring comments
                if (!keepComment)
                {
                    node.Remove();
                }
            }

            // Remove HTML script
            if (settings.RemoveJavaScript && node is HtmlElement && ((HtmlElement) node).Descriptor?.Name == "script")
            {
                node.Remove();
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
            // If we need to decode entities
            if (settings.DecodeEntityCharacters)
            {
                if (textNode.Slice.IndexOf('&') >= 0)
                {
                    var text = textNode.Slice.ToString();
                    textNode.Slice = new StringSlice(EntityHelper.Unescape(text));
                }
            }

            // If we don't do anything for TextNode, we can early exit
            if (!settings.CollapseWhitespaces)
            {
                return;
            }

            // Find the first non-transparent parent
            var parent = textNode.Parent;
            while (parent != null &&
                   (parent.Descriptor == null || parent.Descriptor.Category == ContentKind.Transparent))
            {
                parent = parent.Parent;
            }

            if (!textNode.Slice.IsEmptyOrWhiteSpace() || (parent?.Descriptor != null && xmlNamespaceCount == 0))
            {
                pendingTexts.Add(textNode);
            }
            else
            {
                textNode.Remove();
            }
        }

        private void TrimNodeOnEnd(HtmlElement element)
        {
            // If the element is a valid HTML descriptor, we can safely turn-it all lowercase
            if (xmlNamespaceCount == 0)
            {
                element.Name = element.Name.ToLowerInvariant();
            }

            // If the element being visited is not an inline tag, we need to clear the previous text node
            if (settings.CollapseWhitespaces && !settings.InlineTagsPreservingSpacesAround.ContainsKey(element.Name) && element.Descriptor != null && element.Kind != ElementKind.SelfClosing)
            {
                TrimPendingTextNodes();
            }

            // Remove invalid closing tags
            if (settings.RemoveInvalidClosingTags && element.Kind == ElementKind.Closing)
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

            // Remove optional tags
            if (settings.RemoveOptionalTags && !settings.KeepTags.Contains(element.Name))
            {
                var nextElement = element.FindNextSibling<HtmlElement>();

                // Handle end tag omit
                var canOmitEndTag = element.Descriptor?.CanOmitEndTag;
                if (element.Kind == ElementKind.OpeningClosing && canOmitEndTag != null && canOmitEndTag(element, nextElement, false))
                {
                    element.Kind = ElementKind.Opening;
                }

                // Handle start tag omit
                var canOmitStartTag = element.Descriptor?.CanOmitStartTag;
                if ((element.Attributes == null || element.Attributes.Count == 0) && element.Kind == ElementKind.Opening && canOmitStartTag != null && canOmitStartTag(element, nextElement, false))
                {
                    element.Kind = ElementKind.None;
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
            if (pendingTagNonCollapsibleWithSpaces == 0)
            {
                HtmlText previousTextNode = null;
                HtmlText firstTextNode = null;
                for (int i = 0; i < pendingTexts.Count; i++)
                {
                    var textNode = pendingTexts[i];
                    if (firstTextNode == null)
                    {
                        firstTextNode = textNode;
                    }

                    var previousElement = textNode.PreviousSibling as HtmlElement;
                    var nextElement = textNode.NextSibling as HtmlElement;

                    var isPreviousElementPreservingSpace = previousElement != null &&
                                                           settings.InlineTagsPreservingSpacesAround.ContainsKey(
                                                               previousElement.Name);
                    var isNextElementPreservingSpace = nextElement != null &&
                                                       settings.InlineTagsPreservingSpacesAround.ContainsKey(nextElement
                                                           .Name);

                    // If we expect to keep one space after collapsing
                    var isFirstText = textNode == firstTextNode;
                    var isLastText = i + 1 == pendingTexts.Count;
                    if (!settings.KeepOneSpaceWhenCollapsing || isFirstText || isLastText)
                    {
                        var isPreviousTrailing = previousTextNode != null && previousTextNode.Slice.HasTrailingSpaces();
                        var isNextStartsBySpace = i + 1 >= pendingTexts.Count ||
                                                  pendingTexts[i + 1].Slice.StartsBySpace();

                        // We can trim the heading whitespaces if:
                        // - we don't have a previous element (either inline or parent container)
                        // - OR the previous element (sibling or parent) is not a tag that require preserving spaces around
                        // - OR the previous text node has already some trailing spaces
                        if (!isPreviousElementPreservingSpace && (previousTextNode == null || isPreviousTrailing) &&
                            (previousElement == null || isFirstText))
                        {
                            textNode.Slice.TrimStart();
                        }

                        // We can trim the traling whitespaces if:
                        // - we don't have a next element (either inline or parent container)
                        // - OR the next element (sibling or parent) is not a tag that require preserving spaces around
                        if (!isNextElementPreservingSpace && isNextStartsBySpace)
                        {
                            textNode.Slice.TrimEnd();
                        }
                    }

                    // If we are not in the context of a tag that doesn't accept to collapse whitespaces, 
                    // we can collapse them for this text node
                    textNode.Slice.CollapseSpaces();

                    // If the text node is empty, remove it from the tree
                    if (textNode.Slice.IsEmpty() || (textNode.Slice.IsEmptyOrWhiteSpace() &&
                                                     !isPreviousElementPreservingSpace &&
                                                     !isNextElementPreservingSpace))
                    {
                        textNode.Remove();
                        if (firstTextNode == textNode)
                        {
                            firstTextNode = null;
                        }
                    }
                    else
                    {
                        // Replace the previous textnode
                        previousTextNode = textNode;
                    }
                }

                // Trim any trailing spaces of the last known text node if we are moving to a block level
                if (previousTextNode != null && previousTextNode.NextSibling == null)
                {
                    previousTextNode.Slice.TrimEnd();
                    if (previousTextNode.Slice.IsEmpty())
                    {
                        previousTextNode.Remove();
                    }
                }
            }

            pendingTexts.Clear();
        }
        private void TrimScriptOrStyle(HtmlElement element, bool isJs)
        {
            // We remove the type attribute, as it default to text/css and text/javascript
            if (settings.RemoveScriptStyleTypeAttribute)
            {
                element.RemoveAttribute("type");
            }

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

            var textMinified = MinifyJsOrCss(text, isJs);
            if (textMinified == null)
            {
                return;
            }
            raw.Slice = new StringSlice(textMinified);
        }


        private string MinifyJsOrCss(string text, bool isJs)
        {
            var result = isJs
                ? Uglify.Js(text, "inner_js", settings.JsSettings)
                : Uglify.Css(text, "inner_css", settings.CssSettings);

            if (result.Errors != null)
            {
                Errors.AddRange(result.Errors);
            }

            if (result.HasErrors)
            {
                HasErrors = true;
                return text;
            }

            return result.Code;
        }

        private string MinifyCssAttribute(string text)
        {
            var result = Uglify.Css(text, "inner_css", attributeCssSettings);
            if (result.Errors != null)
            {
                Errors.AddRange(result.Errors);
            }

            if (result.HasErrors)
            {
                HasErrors = true;
                return text;
            }

            return result.Code;
        }

        private bool TrimAttribute(HtmlElement element, HtmlAttribute attribute)
        {
            var tag = element.Name.ToLowerInvariant();
            var attr = attribute.Name.ToLowerInvariant();

            if (attribute.Value != null)
            {
                if (settings.RemoveJavaScript)
                {
                    if (ScriptAttributes.ContainsKey(attr) || attribute.Value.Trim().StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                if (IsUriTypeAttribute(element.Name, attribute.Name))
                {
                    attribute.Value = attribute.Value.Trim();
                    return false;
                }
                if (attr == "class")
                {
                    attribute.Value = attribute.Value.Trim();
                    //if (options.sortClassName)
                    //{
                    //    attrValue = options.sortClassName(attrValue);
                    //}
                    // else
                    {
                        attribute.Value = CharHelper.CollapseWhitespaces(attribute.Value);
                    }
                    return attribute.Value == string.Empty;
                }
                else if (attr == "style" && element.Descriptor != null && settings.MinifyCssAttributes)
                {
                    attribute.Value = MinifyCssAttribute(attribute.Value);
                    return attribute.Value == string.Empty;
                }
            }

            if (settings.ShortBooleanAttribute && attribute.Value == "true")
            {
                attribute.Value = null;
            }

            if (settings.RemoveEmptyAttributes)
            {
                if ((attribute.Value != null || (attribute.Value == null && AttributesRemovedIfEmpty.ContainsKey(attr))) && attribute.Value.IsNullOrWhiteSpace())
                {
                    attribute.Value = string.Empty;

                    return (tag == "input" && attr == "value") || AttributesRemovedIfEmpty.ContainsKey(attr);
                }
            }

            if (!settings.AttributesCaseSensitive && xmlNamespaceCount == 0)
            {
                attribute.Name = attr;
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

        private static bool IsUriTypeAttribute(string tag, string attr)
        {
            // Code from https://github.com/kangax/html-minifier/blob/gh-pages/src/htmlminifier.js
            return ((tag == "a" || tag == "area" || tag == "base") && attr == "href") ||
                   (tag == "img" && (attr == "src" || attr == "longdesc" || attr == "usemap")) ||
                   (tag == "object" &&
                    (attr == "classid" || attr == "codebase" || attr == "data" || attr == "usemap")) ||
                   (tag == "q" && attr == "cite") ||
                   (tag == "blockquote" && attr == "cite") ||
                   ((tag == "ins" || tag == "del") && attr == "cite") ||
                   (tag == "form" && attr == "action") ||
                   (tag == "input" && (attr == "src" || attr == "usemap")) ||
                   (tag == "head" && attr == "profile") ||
                   (tag == "script" && (attr == "src" || attr == "for"));
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
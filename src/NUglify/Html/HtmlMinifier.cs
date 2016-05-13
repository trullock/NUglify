// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;

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
        }

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
            if (element.Descriptor != null)
            {
                element.Name = element.Name.ToLowerInvariant();
            }

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
                if (canOmitEndTag != null && canOmitEndTag(element, nextElement))
                {
                    element.Kind = ElementKind.StartWithoutEnd;
                }
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
    }
}
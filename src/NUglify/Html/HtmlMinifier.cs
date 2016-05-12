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
        private readonly HtmlDocument html;
        private HtmlText previousTextNode;
        private int pendingTagNonCollapsibleWithSpaces;

        private static readonly Dictionary<string, bool> InlineTagsPreserveSpacesAround = new []
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

        private static readonly Dictionary<string, bool> TagsWithNonCollapsableWhitespaces = new[]
        {
            "pre",
            "textarea",
            "code",
        }.ToDictionaryBool(false);


        public HtmlMinifier(HtmlDocument html)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            this.html = html;
        }

        public void Minify()
        {
            ProcessChildren(html);
        }

        private void ProcessNode(HtmlNode node)
        {
            TrimNode(node);

            var element = node as HtmlElement;
            bool isContentNonCollapsible = false;
            if (element != null && TagsWithNonCollapsableWhitespaces.ContainsKey(element.Name))
            {
                pendingTagNonCollapsibleWithSpaces++;
                isContentNonCollapsible = true;
            }

            ProcessChildren(node);

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

        private void TrimNode(HtmlNode node)
        {
            var textNode = node as HtmlText;
            if (textNode != null)
            {
                TrimNode(textNode);
            }
            else if (node is HtmlElement)
            {
                TrimNode((HtmlElement) node);
            }
        }

        private void TrimNode(HtmlText textNode)
        {
            var previousElement = (textNode.PreviousSibling as HtmlElement) ?? textNode.Parent;

            // We can trim the heading whitespaces if:
            // - we don't have a previous element (either inline or parent container)
            // - OR the previous element (sibling or parent) is not a tag that require preserving spaces around
            // - OR the previous text node has already some trailing spaces
            if (previousElement == null || !InlineTagsPreserveSpacesAround.ContainsKey(previousElement.Name) || (previousTextNode != null && previousTextNode.Slice.HasTrailingSpaces()))
            {
                textNode.Slice.TrimStart();
            }

            // We can trim the traling whitespaces if:
            // - we don't have a next element (either inline or parent container)
            // - OR the next element (sibling or parent) is not a tag that require preserving spaces around
            var nextElement = textNode.NextSibling as HtmlElement ?? textNode.Parent;
            if (nextElement == null || !InlineTagsPreserveSpacesAround.ContainsKey(nextElement.Name))
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

            // Replace the previous textnode
            previousTextNode = textNode;
        }

        private void TrimNode(HtmlElement element)
        {
            // If the element is a valid HTML descriptor, we can safely turn-it all lowercase
            if (element.Descriptor != null)
            {
                element.Name = element.Name.ToLowerInvariant();
            }

            // If the element being visited is not an inline tag, we need to clear the previous text node
            if (!InlineTagsPreserveSpacesAround.ContainsKey(element.Name))
            {
                // Trim any trailing spaces of the last known text node if we are moving to a block level
                if (previousTextNode != null && pendingTagNonCollapsibleWithSpaces == 0)
                {
                    previousTextNode.Slice.TrimEnd();
                }
                previousTextNode = null;
            }
        }
    }
}
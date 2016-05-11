// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NUglify.Helpers;

namespace NUglify.Html
{
    /// <summary>
    /// Base class for an HTML node.
    /// </summary>
    public abstract class HtmlNode
    {
        private HtmlElement parent;
        private HtmlNode previousSibling;
        private HtmlNode nextSibling;
        private HtmlNode firstChild;
        private HtmlNode lastChild;

        public HtmlElement Parent => parent;

        public ChildrenEnumerator Children => new ChildrenEnumerator(this);

        public void AppendChild(HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Parent != null) throw new ArgumentException("Node has already a parent", nameof(node));

            if (lastChild != null)
            {
                lastChild.nextSibling = node;
                node.previousSibling = lastChild;
                lastChild = node;
            }
            else
            {
                firstChild = node;
                lastChild = node;
            }

            node.parent = (HtmlElement)this;
        }

        public void Remove()
        {
            if (parent == null)
            {
                return;
            }

            if (previousSibling != null)
            {
                previousSibling.nextSibling = nextSibling;
            }
            if (nextSibling != null)
            {
                nextSibling.previousSibling = previousSibling;
            }

            if (parent.firstChild == this)
            {
                parent.firstChild = nextSibling;
            }
            if (parent.lastChild == this)
            {
                parent.lastChild = nextSibling ?? previousSibling;
            }

            parent = null;
        }

        public struct ChildrenEnumerator : IEnumerable<HtmlNode>, IEnumerator<HtmlNode>
        {
            private readonly HtmlNode origin;
            private HtmlNode next;

            internal ChildrenEnumerator(HtmlNode node)
            {
                origin = node;
                Current = null;
                next = origin;
            }

            [MethodImpl((MethodImplOptions)256)]
            public ChildrenEnumerator GetEnumerator()
            {
                return this;
            }

            IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Dispose()
            {
            }

            [MethodImpl((MethodImplOptions)256)]
            public bool MoveNext()
            {
                if (next != null)
                {
                    Current = next;
                    next = Current?.nextSibling;
                    return true;
                }
                Current = null;
                return false;
            }

            public void Reset()
            {
                Current = null;
                next = origin;
            }

            public HtmlNode Current { get; private set; }

            object IEnumerator.Current => Current;
        }
    }

    public class HtmlDocument : HtmlElement
    {
        public HtmlDocument()
        {
            Name = "$root";
        }
    }
}
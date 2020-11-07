// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NUglify.Html
{
    /// <summary>
    /// Base class for a minimalistic HTML DOM suitable for removing text or extracting text.
    /// </summary>
    public abstract class HtmlNode
    {
	    public SourceLocation Location { get; set; }

        public HtmlElement Parent { get; private set; }

        public HtmlNode PreviousSibling { get; private set; }

        public HtmlNode NextSibling { get; private set; }

        public HtmlNode FirstChild { get; private set; }

        public HtmlNode LastChild { get; private set; }

        public ChildrenEnumerator Children => new ChildrenEnumerator(this);

        public void AppendChild(HtmlNode node)
        {
            if (node == null) 
	            throw new ArgumentNullException(nameof(node));
            if (node.Parent != null) 
	            throw new ArgumentException("Node has already a parent", nameof(node));

            if (LastChild != null)
            {
                LastChild.NextSibling = node;
                node.PreviousSibling = LastChild;
                LastChild = node;
            }
            else
            {
                FirstChild = node;
                LastChild = node;
            }

            node.Parent = (HtmlElement)this;
        }

        public T FindNextSibling<T>() where T : HtmlNode
        {
            var next = NextSibling;
            while (next != null)
            {
	            if (next is T nextElement)
                    return nextElement;

                next = next.NextSibling;
            }
            return null;
        }

        public IEnumerable<HtmlNode> FindAllDescendants()
        {
            foreach (var child in Children)
            {
                yield return child;

                foreach (var subChild in child.FindAllDescendants())
                    yield return subChild;
            }
        }

        public void Remove()
        {
            if (Parent == null)
                return;

            if (PreviousSibling != null)
                PreviousSibling.NextSibling = NextSibling;

            if (NextSibling != null)
                NextSibling.PreviousSibling = PreviousSibling;

            if (Parent.FirstChild == this)
                Parent.FirstChild = NextSibling;

            if (Parent.LastChild == this)
                Parent.LastChild = NextSibling ?? PreviousSibling;

            Parent = null;
        }

        public struct ChildrenEnumerator : IEnumerable<HtmlNode>, IEnumerator<HtmlNode>
        {
            readonly HtmlNode origin;
            HtmlNode next;

            internal ChildrenEnumerator(HtmlNode node)
            {
                origin = node.FirstChild;
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
                    next = Current?.NextSibling;
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
}
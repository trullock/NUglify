// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Helpers;
using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestCollapseWhiteSpaces : TestHtmlParserBase
    {
        [Test]
        public void TestSpaceCollapsing()
        {
            equal(minify("   This is  \t  a    text  \n   \r   \r\n with  \f too many    spaces    "),
                "This is a text with too many spaces");
        }

        [Test]
        public void TestBodyAndSpan()
        {
            equal(minify(@"<!DOCTYPE html> 
<html> 
<head> 
    <meta charset='utf-8' /> 
    <title>test</title>
</head> 
<body> 
 <span>Some</span> 
 <span>whitespace</span> 
</body> 
</html>"), "<!DOCTYPE html><meta charset=utf-8><title>test</title><span>Some</span> <span>whitespace</span>");
        }

        [Test]
        public void PreTagRetainsWhitespace()
        {
            var settings = new HtmlSettings();
            //settings.CollapseWhitespaces = false;
            equal(minify("<pre>Line1\nLine2</pre>", settings), "<pre>Line1\nLine2</pre>");
            equal(minify("<pre>    Line1\n    Line2</pre>", settings), "<pre>    Line1\n    Line2</pre>");
            equal(minify("<pre><code>Line1\nLine2</code></pre>", settings), "<pre><code>Line1\nLine2</code></pre>");
            equal(minify("<pre><code>    Line1\n    Line2</code></pre>", settings), "<pre><code>    Line1\n    Line2</code></pre>");
        }

        [Test]
        public void TestSpaceCollapsing2()
        {
            equal(minify("<span><strong>Multipage Version</strong> <code>whatwg.org/html</code></span>"),
                "<span><strong>Multipage Version</strong> <code>whatwg.org/html</code></span>");
        }

        [Test]
        public void TestSpanSpaceCollapsing()
        {
            equal(minify(@"<div>
    <span class='a'></span>          <span class='b'></span>        <span class='c'></span>
</div>", new HtmlSettings() {KeepOneSpaceWhenCollapsing = true}  ), "<div><span class=a></span> <span class=b></span> <span class=c></span></div>");
        }

        [Test]
        public void TestSpanSpaceCollapsingEnd()
        {
            var input = @"<span tabindex='2'>
    <i>
        hat
    </i>
</span>";
            equal(minify(input), "<span tabindex=2><i>hat</i></span>");
            equal(minify(input, new HtmlSettings() { KeepOneSpaceWhenCollapsing = true }), "<span tabindex=2><i>hat</i></span>");
        }

        [Test]
        public void TestPreserveBetweenInlineTags1()
        {
            // Check that spaces are collapsed at begin/end for inline tags instead of stripping surrounding texts
            // as this is nicer and feel more natural, so we should not have something like:
            // This is a text <em>with an emphasis </em>and trailing
            equal(minify("This is a text <em> with an emphasis </em> and trailing"),
                "This is a text <em>with an emphasis</em> and trailing");
        }

        [Test]
        public void TestPreserveBetweenInlineTags2()
        {
            equal(minify("This is a text<em>  with an emphasis  </em>and trailing"),
                "This is a text<em> with an emphasis </em>and trailing");
        }


        [Test]
        public void TestPreserveBetweenInlineTags3()
        {
            equal(minify("This  <b>  is a text  <em>  with an emphasis  </em>  and  </b>  trailing"),
                "This <b>is a text <em>with an emphasis</em> and</b> trailing");
        }

        [Test]
        public void TestTextareaPreserveNewLine()
        {
            var settings = new HtmlSettings();
            equal(minify("\n<label>Text</label>\n    <textarea>   Line1   \n    Line2   </textarea>", settings), "<label>Text</label><textarea>   Line1   \n    Line2   </textarea>");
        }

        [Test]
        public void SpaceNormalizationAroundText()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            new[]
            {
                "a", "abbr", "acronym", "b", "big", "del", "em", "font", "i", "ins", "kbd",
                "mark", "s", "samp", "small", "span", "strike", "strong", "sub", "sup",
                "time", "tt", "u", "var"
            }.ForEach(el =>
            {
                equal(minify($"foo <{el}>baz</{el}> bar"), $"foo <{el}>baz</{el}> bar");
                equal(minify($"foo<{el}>baz</{el}>bar"), $"foo<{el}>baz</{el}>bar");
                equal(minify($"foo <{el}>baz</{el}>bar"), $"foo <{el}>baz</{el}>bar");
                equal(minify($"foo<{el}>baz</{el}> bar"), $"foo<{el}>baz</{el}> bar");
                equal(minify($"foo <{el}> baz </{el}> bar"), $"foo <{el}>baz</{el}> bar");
                equal(minify($"foo<{el}> baz </{el}>bar"), $"foo<{el}> baz </{el}>bar");
                equal(minify($"foo <{el}> baz </{el}>bar"), $"foo <{el}>baz </{el}>bar");
                equal(minify($"foo<{el}> baz </{el}> bar"), $"foo<{el}> baz</{el}> bar");

                equal(minify($"<p>foo <{el}>baz</{el}> bar</p>"),
                    $"<p>foo <{el}>baz</{el}> bar");
                equal(minify($"<p>foo<{el}>baz</{el}>bar</p>"),
                    $"<p>foo<{el}>baz</{el}>bar");
                equal(minify($"<p>foo <{el}>baz</{el}>bar</p>"),
                    $"<p>foo <{el}>baz</{el}>bar");
                equal(minify($"<p>foo<{el}>baz</{el}> bar</p>"),
                    $"<p>foo<{el}>baz</{el}> bar");
                equal(minify($"<p>foo <{el}> baz </{el}> bar</p>"),
                    $"<p>foo <{el}>baz</{el}> bar");
                equal(minify($"<p>foo<{el}> baz </{el}>bar</p>"),
                    $"<p>foo<{el}> baz </{el}>bar");
                equal(minify($"<p>foo <{el}> baz </{el}>bar</p>"),
                    $"<p>foo <{el}>baz </{el}>bar");
                equal(minify($"<p>foo<{el}> baz </{el}> bar</p>"),
                    $"<p>foo<{el}> baz</{el}> bar");
            });


            var disableRemoveOptTag = new HtmlSettings()
            {
                RemoveOptionalTags = false,
                IsFragmentOnly = true
            };

            new []
            {
                "bdi", "bdo", "button", "cite", "code", "dfn", "math", "q", /*"rt", "rp"*/ "svg"
            }.ForEach(el =>
            {
                equal(minify($"foo <{el}>baz</{el}> bar", disableRemoveOptTag), $"foo <{el}>baz</{el}> bar");
                equal(minify($"foo<{el}>baz</{el}>bar", disableRemoveOptTag), $"foo<{el}>baz</{el}>bar");
                equal(minify($"foo <{el}>baz</{el}>bar", disableRemoveOptTag), $"foo <{el}>baz</{el}>bar");
                equal(minify($"foo<{el}>baz</{el}> bar", disableRemoveOptTag), $"foo<{el}>baz</{el}> bar");
                equal(minify($"foo <{el}> baz </{el}> bar", disableRemoveOptTag), $"foo <{el}>baz</{el}> bar");
                equal(minify($"foo<{el}> baz </{el}>bar", disableRemoveOptTag), $"foo<{el}> baz </{el}>bar");
                equal(minify($"foo <{el}> baz </{el}>bar", disableRemoveOptTag), $"foo <{el}>baz </{el}>bar");
                equal(minify($"foo<{el}> baz </{el}> bar", disableRemoveOptTag), $"foo<{el}> baz</{el}> bar");
                equal(minify($"<div>foo <{el}>baz</{el}> bar</div>", disableRemoveOptTag),
                    $"<div>foo <{el}>baz</{el}> bar</div>");
                equal(minify($"<div>foo<{el}>baz</{el}>bar</div>", disableRemoveOptTag),
                    $"<div>foo<{el}>baz</{el}>bar</div>");
                equal(minify($"<div>foo <{el}>baz</{el}>bar</div>", disableRemoveOptTag),
                    $"<div>foo <{el}>baz</{el}>bar</div>");
                equal(minify($"<div>foo<{el}>baz</{el}> bar</div>", disableRemoveOptTag),
                    $"<div>foo<{el}>baz</{el}> bar</div>");
                equal(minify($"<div>foo <{el}> baz </{el}> bar</div>", disableRemoveOptTag),
                    $"<div>foo <{el}>baz</{el}> bar</div>");
                equal(minify($"<div>foo<{el}> baz </{el}>bar</div>", disableRemoveOptTag),
                    $"<div>foo<{el}> baz </{el}>bar</div>");
                equal(minify($"<div>foo <{el}> baz </{el}>bar</div>", disableRemoveOptTag),
                    $"<div>foo <{el}>baz </{el}>bar</div>");
                equal(minify($"<div>foo<{el}> baz </{el}> bar</div>", disableRemoveOptTag),
                    $"<div>foo<{el}> baz</{el}> bar</div>");
            });

            new string[][]
            {
                new[] {"<span> foo </span>", "<span>foo</span>"},
                new[] {" <span> foo </span> ", "<span>foo</span>"},
                new[] {"<nobr>a</nobr>", "<nobr>a</nobr>"},
                new[] {"<nobr>a </nobr>", "<nobr>a</nobr>"},
                new[] {"<nobr> a</nobr>", "<nobr>a</nobr>"},
                new[] {"<nobr> a </nobr>", "<nobr>a</nobr>"},
                new[] {"a<nobr>b</nobr>c", "a<nobr>b</nobr>c"},
                new[] {"a<nobr>b </nobr>c", "a<nobr>b </nobr>c"},
                new[] {"a<nobr> b</nobr>c", "a<nobr> b</nobr>c"},
                new[] {"a<nobr> b </nobr>c", "a<nobr> b </nobr>c"},
                new[] {"a<nobr>b</nobr> c", "a<nobr>b</nobr> c"},
                new[] {"a<nobr>b </nobr> c", "a<nobr>b</nobr> c"},
                new[] {"a<nobr> b</nobr> c", "a<nobr> b</nobr> c"},
                new[] {"a<nobr> b </nobr> c", "a<nobr> b</nobr> c"},
                new[] {"a <nobr>b</nobr>c", "a <nobr>b</nobr>c"},
                new[] {"a <nobr>b </nobr>c", "a <nobr>b </nobr>c"},
                new[] {"a <nobr> b</nobr>c", "a <nobr>b</nobr>c"},
                new[] {"a <nobr> b </nobr>c", "a <nobr>b </nobr>c"},
                new[] {"a <nobr>b</nobr> c", "a <nobr>b</nobr> c"},
                new[] {"a <nobr>b </nobr> c", "a <nobr>b</nobr> c"},
                new[] {"a <nobr> b</nobr> c", "a <nobr>b</nobr> c"},
                new[] {"a <nobr> b </nobr> c", "a <nobr>b</nobr> c"}
            }.ForEach(inputs =>
            {
                equal(minify(inputs[0]), inputs[1]);
                input = "<div>" + inputs[0] + "</div>";
                output = "<div>" + inputs[1] + "</div>";
                equal(minify(input), output);
            });

            equal(minify("<p>foo <img> bar</p>", disableRemoveOptTag), "<p>foo <img /> bar</p>");
            equal(minify("<p>foo<img>bar</p>", disableRemoveOptTag), "<p>foo<img />bar</p>");
            equal(minify("<p>foo <img>bar</p>", disableRemoveOptTag), "<p>foo <img />bar</p>");
            equal(minify("<p>foo<img> bar</p>", disableRemoveOptTag), "<p>foo<img /> bar</p>");
            equal(minify("<p>  <a href=\"#\">  <code>foo</code></a> bar</p>", disableRemoveOptTag), "<p><a href=#><code>foo</code></a> bar</p>");
            equal(minify("<p><a href=\"#\"><code>foo</code></a> bar</p>", disableRemoveOptTag), "<p><a href=#><code>foo</code></a> bar</p>");
            equal(minify("<p>  <a href=\"#\">  <code>   foo</code></a> bar   </p>", disableRemoveOptTag), "<p><a href=#><code>foo</code></a> bar</p>");
            equal(minify("<div> Empty <!-- or --> not </div>", new HtmlSettings() { RemoveComments =  false}), "<div>Empty<!-- or --> not</div>");

            input = "<li><i></i> <b></b> foo</li>";
            output = "<li><i></i> <b></b> foo</li>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<li><i> </i> <b></b> foo</li>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<li> <i></i> <b></b> foo</li>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<li><i></i> <b> </b> foo</li>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<li> <i> </i> <b> </b> foo</li>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<div> <a href=\"#\"> <span> <b> foo </b> <i> bar </i> </span> </a> </div>";
            output = "<div><a href=#><span><b>foo</b> <i>bar</i></span></a></div>";
            equal(minify(input, disableRemoveOptTag), output);
            input = "<head> <!-- a --> <!-- b --><link> </head>";
            output = "<head><!-- a --><!-- b --><link>";
            equal(minify(input, new HtmlSettings() { RemoveComments = false }), output);
            input = "<head> <!-- a --> <!-- b --> <!-- c --><link> </head>";
            output = "<head><!-- a --><!-- b --><!-- c --><link>";
            equal(minify(input, new HtmlSettings() { RemoveComments = false } ), output);
        }
    }
}
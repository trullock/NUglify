// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests ported from html-minifier https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
    /// </summary>
    [TestFixture]
    public class TestComments : TestHtmlParserBase
    {
        [Test]
        public void RemovingComments()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            input = "<!-- test -->";
            equal(minify(input), "");

            input = "<!-- foo --><div>baz</div><!-- bar\n\n moo -->";
            equal(minify(input), "<div>baz</div>");
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), input);

            input = "<p title=\"<!-- comment in attribute -->\">foo";
            equal(minify(input), input);

            input = "<script><!-- alert(1) --></script>";
            equal(minify(input), input);

            input = "<STYLE><!-- alert(1) --></STYLE>";
            equal(minify(input), "<style><!-- alert(1) --></style>");
        }

        [Test]
        public void IgnoringComments()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            input = "<!--! test -->";
            equal(minify(input), input);
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), input);

            input = "<!--! foo --><div>baz</div><!--! bar\n\n moo -->";
            equal(minify(input), input);
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), input);

            input = "<!--! foo --><div>baz</div><!-- bar\n\n moo -->";
            equal(minify(input), "<!--! foo --><div>baz</div>");
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), input);

            input = "<!-- ! test -->";
            equal(minify(input), "");
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), input);

            input = "<div>\n\n   \t<div><div>\n\n<p>\n\n<!--!      \t\n\nbar\n\n moo         -->      \n\n</p>\n\n        </div>  </div></div>";
            output = "<div><div><div><p><!--!      \t\n\nbar\n\n moo         --></div></div></div>";
            equal(minify(input), output);
            equal(minify(input, new HtmlSettings() {RemoveComments = false}), output);

            input = "<p rel=\"<!-- comment in attribute -->\" title=\"<!--! ignored comment in attribute -->\">foo";
            equal(minify(input), input);
        }
    }
}
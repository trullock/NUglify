// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests ported from html-minifier https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
    /// </summary>
    [TestFixture]
    public class TestCaseAndSpaceNormalization : TestHtmlParserBase
    {
        [Test]
        public void TestCaseNormalization()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            equal(minify("<P>foo</p>"), "<p>foo");
            equal(minify("<DIV>boo</DIV>"), "<div>boo</div>");
            equal(minify("<DIV title=\"moo\">boo</DiV>"), "<div title=moo>boo</div>");
            equal(minify("<DIV TITLE=\"blah\">boo</DIV>"), "<div title=blah>boo</div>");
            equal(minify("<DIV tItLe=\"blah\">boo</DIV>"), "<div title=blah>boo</div>");
            equal(minify("<DiV tItLe=\"blah\">boo</DIV>"), "<div title=blah>boo</div>");
        }

        [Test]
        public void TestSpaceNormalizationBetweenAttributes()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            equal(minify("<p title=\"bar\">foo</p>"), "<p title=bar>foo");
            equal(minify("<img src=\"test\"/>"), "<img src=test>");
            equal(minify("<p title = \"bar\">foo</p>"), "<p title=bar>foo");
            equal(minify("<p title\n\n\t  =\n     \"bar\">foo</p>"), "<p title=bar>foo");
            equal(minify("<img src=\"test\" \n\t />"), "<img src=test>");
            equal(minify("<input title=\"bar\"       id=\"boo\"    value=\"hello world\">"), "<input title=bar id=boo value=\"hello world\">");
        }
    }
}
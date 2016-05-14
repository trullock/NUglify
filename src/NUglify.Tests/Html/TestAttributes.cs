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
    public class TestAttributes : TestHtmlParserBase
    {
        [Test]
        public void EmptyAttributes()
        {
            input = "<p id=\"\" class=\"\" STYLE=\" \" title=\"\n\" lang=\"\" dir=\"\">x</p>";
            equal(minify(input), "<p>x");

            input = "<p onclick=\"\"   ondblclick=\" \" onmousedown=\"\" ONMOUSEUP=\"\" onmouseover=\" \" onmousemove=\"\" onmouseout=\"\" " +
                    "onkeypress=\n\n  \"\n     \" onkeydown=\n\"\" onkeyup\n=\"\">x</p>";
            equal(minify(input), "<p>x");

            input = "<input onfocus=\"\" onblur=\"\" onchange=\" \" value=\" boo \">";
            equal(minify(input), "<input value=\" boo \">");

            input = "<input value=\"\" name=\"foo\">";
            equal(minify(input), "<input name=foo>");

            input = "<img src=\"\" alt=\"\">";
            equal(minify(input), "<img src=\"\" alt=\"\">");

            // preserve unrecognized attribute
            // remove recognized attrs with unspecified values
            input = "<div data-foo class id style title lang dir onfocus onblur onchange onclick ondblclick onmousedown onmouseup onmouseover onmousemove onmouseout onkeypress onkeydown onkeyup></div>";
            equal(minify(input), "<div data-foo></div>");
        }
    }
}
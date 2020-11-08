// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestSafeHtml : TestHtmlParserBase
    {
        [Test]
        public void RemoveScript()
        {
            var settings = new HtmlSettings() {RemoveJavaScript = true};

            input = "<script><!--\nalert(1);\n--></script>";
            equal(minify(input, settings), string.Empty);
        }

        [Test]
        public void RemoveAttributes()
        {
            var settings = new HtmlSettings() { RemoveJavaScript = true };

            input = "<a onclick='wtf()'>test</a>";
            equal(minify(input, settings), "<a>test</a>");
        }


        [Test]
        public void RemoveAttributes2()
        {
            var settings = new HtmlSettings() { RemoveJavaScript = true };

            input = "<a src='javascript:wtf()'>test</a>";
            equal(minify(input, settings), "<a>test</a>");
        }


        [Test]
        public void RemoveAttributes3()
        {
            var settings = new HtmlSettings { RemoveJavaScript = true };

            input = "<a style='text-align: center;'>test</a>";
            equal(minify(input, settings), "<a style=text-align:center>test</a>");
        }
    }
}
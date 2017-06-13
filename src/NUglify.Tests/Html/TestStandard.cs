// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestStandard : TestHtmlParserBase
    {
        [Test]
        public void TestDocHtmlHeadBody()
        {
            var input = @"<!DOCTYPE html>
<html lang='en-us'>
<head>
</head>
<body>
<p>This is a paragraph</p>
</body>
</html>";
            var output = @"<!DOCTYPE html><html lang=en-us><head></head><body><p>This is a paragraph</body></html>";
            var settings = new HtmlSettings();
            // settings.RemoveOptionalTags = true
            settings.KeepTags.Add("html");
            settings.KeepTags.Add("body");
            settings.KeepTags.Add("head");
            equal(minify(input, settings), output);
        }


    }
}
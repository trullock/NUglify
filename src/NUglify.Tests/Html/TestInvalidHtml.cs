// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
	[TestFixture]
	public class TestInvalidHtml : TestHtmlParserBase
	{
		[Test]
		public void TestInvalidHtmlTags()
		{
			var input = "<p>this should be <parsed>\nthis should> appear\nthis text &lt;should appear</p>";
			var settings = HtmlSettings.Pretty();
			settings.IsFragmentOnly = true;
			settings.MinifyCss = false;
			settings.MinifyCssAttributes = false;
			settings.MinifyJs = false;
			settings.MinifyJsAttributes = false;
			settings.RemoveJavaScript = true;
			var result = minify(input, settings);
			equal(result, "<p>\n  this should be\n  <parsed>\n    this should> appear this text &lt;should appear\n  </parsed>\n</p>", "(1,19): warning : Unbalanced tag [parsed] within tag [p] requiring a closing tag. Force closing it");
		}
	}
}
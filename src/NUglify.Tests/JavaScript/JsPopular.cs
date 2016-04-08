// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Net;
using System.Net.Http;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class JsPopular
    {
        [Test]
        public void TestJQuery()
        {
            var client = new WebClient();
            var jqueryVersion = "jquery-2.2.3.js";
            var jqueryText = client.DownloadString($"https://code.jquery.com/{jqueryVersion}");
            var result = Uglify.Js(jqueryText, jqueryVersion, new CodeSettings()
            {
                PreserveImportantComments = false,
                StripDebugStatements = true,
                LineBreakThreshold = int.MaxValue,
            });
            Assert.False(result.HasErrors);
        }
    }
}
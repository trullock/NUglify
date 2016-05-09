// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestHtmlUglify
    {
        [Test]
        public void TestSimple()
        {
            var result = Uglify.Html("<html>  <BODY>   </body>   </html>");
            Assert.False(result.HasErrors);
            Assert.AreEqual("<html><body></body></html>", result.Code);
        }
    }
}
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
        public void TestSpaceCollapsing()
        {
            var result = Uglify.Html("   This is  \t  a    text  \n   \r   \r\n with  \f too many    spaces    ");
            Assert.False(result.HasErrors);
            Assert.AreEqual("This is a text with too many spaces", result.Code);
        }

        [Test]
        public void TestPreserveBetweenInlineTags1()
        {
            // Check that spaces are collapsed at begin/end for inline tags instead of stripping surrounding texts
            // as this is nicer and feel more natural, so we should not have something like:
            // This is a text <em>with an emphasis </em>and trailing
            var result = Uglify.Html("This is a text <em> with an emphasis </em> and trailing");
            Assert.False(result.HasErrors);
            //                                                       |<- with keep the space here
            Assert.AreEqual("This is a text <em>with an emphasis</em> and trailing", result.Code);
        }

        [Test]
        public void TestPreserveBetweenInlineTags2()
        {
            var result = Uglify.Html("This is a text<em>  with an emphasis  </em>and trailing");
            Assert.False(result.HasErrors);
            Assert.AreEqual("This is a text<em> with an emphasis </em>and trailing", result.Code);
        }


        [Test]
        public void TestPreserveBetweenInlineTags3()
        {
            var result = Uglify.Html("This  <b>  is a text  <em>  with an emphasis  </em>  and  </b>  trailing");
            Assert.False(result.HasErrors);
            Assert.AreEqual("This <b>is a text <em>with an emphasis</em> and</b> trailing", result.Code);
        }
    }
}
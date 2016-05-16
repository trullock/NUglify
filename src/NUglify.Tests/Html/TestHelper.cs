// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestHelper
    {
        [Test]
        public void TestCollapseSpace()
        {
            var result = CharHelper.CollapseWhitespaces("");
            Assert.AreEqual("", result);

            result = CharHelper.CollapseWhitespaces("test1");
            Assert.AreEqual("test1", result);

            result = CharHelper.CollapseWhitespaces("test1 ");
            Assert.AreEqual("test1 ", result);

            result = CharHelper.CollapseWhitespaces(" test1");
            Assert.AreEqual(" test1", result);

            result = CharHelper.CollapseWhitespaces(" test1  test2  ");
            Assert.AreEqual(" test1 test2 ", result);

            result = CharHelper.CollapseWhitespaces(" \n \f \r \t ");
            Assert.AreEqual(" ", result);

            result = CharHelper.CollapseWhitespaces("\n \f \r \t ");
            Assert.AreEqual(" ", result);

            result = CharHelper.CollapseWhitespaces("\ntest1\n test2\n ");
            Assert.AreEqual(" test1 test2 ", result);
        }

    }
}
// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public partial class TestHtmlUglify
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

        [Test]
        public void TestTagOmissionInTables()
        {
            // Test parsing/collapsing of colgroup/col/tr/th/thead/tbody
            var result = Uglify.Html(@"<table>
 <caption>37547 TEE Electric Powered Rail Car Train Functions (Abbreviated)</caption>
 <colgroup><col><col><col></colgroup>
 <thead>
  <tr>
   <th>Function</th>
   <th>Control Unit</th>
   <th>Central Station</th>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>Headlights</td>
   <td>✔</td>
   <td>✔</td>
  </tr>
  <tr>
   <td>Interior Lights</td>
   <td>✔</td>
   <td>✔</td>
  </tr>
  <tr>
   <td>Electric locomotive operating sounds</td>
   <td>✔</td>
   <td>✔</td>
  </tr>
  <tr>
   <td>Engineer's cab lighting</td>
   <td></td>
   <td>✔</td>
  </tr>
  <tr>
   <td>Station Announcements - Swiss</td>
   <td></td>
   <td>✔</td>
  </tr>
 </tbody>
</table>"
);
            Assert.False(result.HasErrors);
            Assert.AreEqual("<table><caption>37547 TEE Electric Powered Rail Car Train Functions (Abbreviated)<colgroup><col><col><col><thead><tr><th>Function<th>Control Unit<th>Central Station<tbody><tr><td>Headlights<td>✔<td>✔<tr><td>Interior Lights<td>✔<td>✔<tr><td>Electric locomotive operating sounds<td>✔<td>✔<tr><td>Engineer's cab lighting<td><td>✔<tr><td>Station Announcements - Swiss<td><td>✔</table>", result.Code);
        }
    }
}
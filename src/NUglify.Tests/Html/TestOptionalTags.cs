// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    public class TestOptionalTags : TestHtmlParserBase
    {
        [Test]
        public void TestListItemAndParagraph()
        {
            var input = @"<ul> <li><p>test <li><p>test2 <li><p>test3 </ul>";
            equal(minify(input),
                "<ul><li><p>test<li><p>test2<li><p>test3</ul>");

            var settings = new HtmlSettings() {RemoveOptionalTags = false, IsFragmentOnly = true};

            equal(minify(input, settings),
                "<ul><li><p>test</p></li><li><p>test2</p></li><li><p>test3</p></li></ul>");
        }

        [Test]
        public void TestTagOmissionInTables()
        {
            // Test parsing/collapsing of colgroup/col/tr/th/thead/tbody
            equal(minify(@"<table>
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
                ),
                "<table><caption>37547 TEE Electric Powered Rail Car Train Functions (Abbreviated)<colgroup><col><col><col><thead><tr><th>Function<th>Control Unit<th>Central Station<tbody><tr><td>Headlights<td>✔<td>✔<tr><td>Interior Lights<td>✔<td>✔<tr><td>Electric locomotive operating sounds<td>✔<td>✔<tr><td>Engineer's cab lighting<td><td>✔<tr><td>Station Announcements - Swiss<td><td>✔</table>");
        }

        [Test]
        public void TestTrTd()
        {
            input = @"<table>
<tbody>
<tr><td><span>A</span><td><span>B</span><td><span>C</span>
<tr><td><span>A</span><td><span>B</span><td><span>C</span>
</tbody>
</table>
";
            // Make sure that we are actually parsing correctly tr/td and they are correctly nested
            // so we disable RemoveOptionalTags to make sure the structure is correct
            output = @"<table><tbody><tr><td><span>A</span></td><td><span>B</span></td><td><span>C</span></td></tr><tr><td><span>A</span></td><td><span>B</span></td><td><span>C</span></td></tr></tbody></table>";
            equal(minify(input, new HtmlSettings() {RemoveOptionalTags = false, IsFragmentOnly = true}), output);
        }

        [Test]
        public void TestSelfClosingTagWithOptionalTags()
        {
            var settings = new HtmlSettings()
            {
                IsFragmentOnly = true,
                RemoveOptionalTags = false,
                RemoveAttributeQuotes = false
            };

            input = "<link rel=\"stylesheet\" href=\"style.css\" />";
            output = "<link rel=\"stylesheet\" href=\"style.css\" />";

            equal(minify(input, settings), output);
        }

        [Test]
        public void TestSelfClosingTagWithoutOptionalTags()
        {
            var settings = new HtmlSettings()
            {
                IsFragmentOnly = true,
                RemoveOptionalTags = true,
                RemoveAttributeQuotes = false
            };

            input = "<link rel=\"stylesheet\" href=\"style.css\" />";
            output = "<link rel=\"stylesheet\" href=\"style.css\">";

            equal(minify(input, settings), output);
        }
    }
}
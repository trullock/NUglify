// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    [TestFixture]
    public class TestHtmlParser
    {
        [Test]
        public void TestSimple()
        {
            AssertHtml("<!DOCTYPE html><html><!--comment--><![CDATA[boo]]><body>Test</body></html>",
                "[0001] !doc: <!DOCTYPE html>",
                "[0001] [tag: <html>",
                "[0002] !com: <!--comment-->",
                "[0002] !cda: <![CDATA[boo]]>",
                "[0002] [tag: <body>",
                "[0003] #txt: Test",
                "[0002] ]tag: </body>",
                "[0001] ]tag: </html>"
                );
        }

        [Test]
        public void TestText()
        {
            AssertHtml("This is a test",
                "[0001] #txt: This is a test"
                );
        }

        [Test]
        public void TestOpenTagWithSpaces()
        {
            AssertHtml("<html   >",
                "[0001] [tag: <html>",
                "[0001] ]tag: </html>"
                );
        }

        [Test]
        public void TestSelfClosingTags()
        {
            AssertHtml("<ul><li>test1<li>test2</ul>",
                "[0001] [tag: <ul>",
                "[0002] [tag: <li>",
                "[0003] #txt: test1",
                "[0002] ]tag: </li>",
                "[0002] [tag: <li>",
                "[0003] #txt: test2",
                "[0002] ]tag: </li>",
                "[0001] ]tag: </ul>"
                );

            AssertHtml("<div><p>test1</div>",
                "[0001] [tag: <div>",
                "[0002] [tag: <p>",
                "[0003] #txt: test1",
                "[0002] ]tag: </p>",
                "[0001] ]tag: </div>"
                );
        }


        [Test]
        public void TestInvalidNestedTag()
        {
            // Try to use a <p> tag inside a <ul> tag
            // the parser will give a warning 
            AssertHtml("<ul><p>test1</ul>",
                "[0001] [tag: <ul>",
                "[0002] [tag: <p>",
                "[0003] #txt: test1",
                "[0002] ]tag: </p>",
                "[0001] ]tag: </ul>",
                "(1,5): warning : The tag <p> is not a valid tag within the parent tag <ul>"
                );
        }

        [Test]
        public void TestOpenTagInvalidAttributes()
        {
            // We are still recovering with an invalid character in a tag
            // but we get an error
            AssertHtml("<html @x>",
                "[0001] [tag: <html x>",
                "[0001] ]tag: </html>",
                "(1,7): error : Invalid character '@' found while parsing <html>"
                );
        }

        [Test]
        public void TestOpenTagInvalidAttributes2()
        {
            // We are still recovering with an invalid character in a tag
            // but we get an error
            AssertHtml("<html x=>",
                "[0001] [tag: <html x>",
                "[0001] ]tag: </html>",
                "(1,9): error : Invalid character '>' found while parsing <html> and after attribute [x]. Expecting valid character after '='"
                );
        }

        [Test]
        public void TestOpenTagWithAttributes()
        {
            AssertHtml("<html x=1 y='test' z=\"test2\" w a b c>",
                "[0001] [tag: <html x=\"1\" y=\"test\" z=\"test2\" w a b c>",
                "[0001] ]tag: </html>"
                );
        }

        [Test]
        public void TestOpenTagWithAttributesInvalid()
        {
            // We don't try to recover from an unclosed string
            AssertHtml("<html x=1 y='test' z=\"test2 w a b c>",
                "[0001] #txt: <html x=1 y='test' z=\"test2 w a b c>",
                "(1,37): error : Invalid EOF found while parsing <html>"
                );
        }

        [Test]
        public void TestOpenCloseTag()
        {
            AssertHtml("<br/>",
                "[0001] [tag: <br>"
                );
        }

        [Test]
        public void TestOpenCloseTagError()
        {
            AssertHtml("<br\n/?>",
                "[0001] [tag: <br>",
                "[0001] ]tag: </br>",
                "(2,2): error : Invalid character '?' found while parsing <br>"
                );
        }

        [Test]
        public void TestProcessingInstructionSimple()
        {
            AssertHtml("<?xml version=\"1.0\"?>",
                "[0001] [tag: <?xml version=\"1.0\"?>"
                );
        }

        [Test]
        public void TestProcessingInstructionInvalid()
        {
            AssertHtml("<?xml version=\"1.0\">",
                "[0001] #txt: <?xml version=\"1.0\">",
                "(1,20): error : Invalid character '>' found while parsing <xml>",
                "(1,21): error : Invalid EOF found while parsing <?xml?>"
                );
        }

        [Test]
        public void TestBadTag()
        {
            AssertHtml("<html   t",
                "[0001] #txt: <html   t",
                "(1,10): error : Invalid EOF found while parsing <html>"
                );
        }

        [Test]
        public void TestEndTag()
        {
            AssertHtml("</html>",
                "[0001] [tag: </html>",
                "(1,1): warning : Unable to find opening tag for closing tag </html>"
                );
        }

        [Test]
        public void TestEndTagInvalid()
        {
            AssertHtml("</@html>",
                "[0001] #txt: </@html>",
                "(1,3): error : Invalid character '@' following end tag </"
                );
        }

        [Test]
        public void TestEndTagInvalid2()
        {
            AssertHtml("</html",
                "[0001] #txt: </html",
                "(1,7): error : Invalid EOF found while parsing </html>"
                );
        }

        [Test]
        public void TestComment()
        {
            AssertHtml("<!-- test -->",
                "[0001] !com: <!-- test -->"
                );
        }

        [Test]
        public void TestComment2()
        {
            AssertHtml("<!---10-->",
                "[0001] !com: <!---10-->"
                );
        }

        [Test]
        public void TestComment3()
        {
            AssertHtml("<!----10--->",
                "[0001] #txt: <!----10--->",
                "(1,7): error : Invalid character '1' found while parsing <!--"
                );
        }

        [Test]
        public void TestCommentInvalidEOF()
        {
            AssertHtml("<!----10---",
                "[0001] #txt: <!----10---",
                "(1,7): error : Invalid character '1' found while parsing <!--"
                );
        }

        [Test]
        public void TestCDATASimple()
        {
            AssertHtml("<![CDATA[This is a test]]>",
                "[0001] !cda: <![CDATA[This is a test]]>"
                );
        }

        [Test]
        public void TestCDATAEmpty()
        {
            AssertHtml("<![CDATA[]]>",
                "[0001] !cda: <![CDATA[]]>"
                );
        }

        [Test]
        public void TestCDATAIncomplete()
        {
            AssertHtml("<![CDATA[This is a test",
                "[0001] #txt: <![CDATA[This is a test",
                "(1,24): error : Invalid EOF found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestCDATAMaybe1()
        {
            AssertHtml("<![CDATA[This ]is a test]]>",
                "[0001] !cda: <![CDATA[This ]is a test]]>"
                );
        }

        [Test]
        public void TestCDATAMaybe2()
        {
            AssertHtml("<![CDATA[This ]]is a test]]>",
                "[0001] !cda: <![CDATA[This ]]is a test]]>"
                );
        }

        [Test]
        public void TestCDATAMaybe3()
        {
            AssertHtml("<![CDATA[This ]>is a test]]>",
                "[0001] !cda: <![CDATA[This ]>is a test]]>"
                );
        }

        [Test]
        public void TestCDATANot1()
        {
            AssertHtml("<![C ]]>",
                "[0001] #txt: <![C ]]>",
                "(1,5): error : Invalid character ' ' found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestCDATANot2()
        {
            AssertHtml("<![CD ]]>",
                "[0001] #txt: <![CD ]]>",
                "(1,6): error : Invalid character ' ' found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestCDATANot3()
        {
            AssertHtml("<![CDA ]]>",
                "[0001] #txt: <![CDA ]]>",
                "(1,7): error : Invalid character ' ' found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestCDATANot4()
        {
            AssertHtml("<![CDAT ]]>",
                "[0001] #txt: <![CDAT ]]>",
                "(1,8): error : Invalid character ' ' found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestCDATANot5()
        {
            AssertHtml("<![CDATA ]]>",
                "[0001] #txt: <![CDATA ]]>",
                "(1,9): error : Invalid character ' ' found while parsing <![CDATA["
                );
        }

        [Test]
        public void TestDOCTYPE1()
        {
            AssertHtml("<!DOCTYPE>",
                "[0001] !doc: <!DOCTYPE>"
                );
        }

        [Test]
        public void TestDOCTYPE2()
        {
            AssertHtml("<!DOCTYPE html>",
                "[0001] !doc: <!DOCTYPE html>"
                );
        }

        [Test]
        public void TestDOCTYPE3()
        {
            AssertHtml("<!DOCTYPE html \"test with fake url\">",
                "[0001] !doc: <!DOCTYPE html \"test with fake url\">"
                );
        }

        [Test]
        public void TestDOCTYPEInvalid1()
        {
            AssertHtml("<!DOCTYPE html",
                "[0001] #txt: <!DOCTYPE html",
                "(1,15): error : Invalid EOF found while parsing <!DOCTYPE"
                );
        }

        [Test]
        public void TestDOCTYPEInvalid2()
        {
            AssertHtml("<!DOCTYPEX>",
                "[0001] #txt: <!DOCTYPEX>",
                "(1,10): error : Invalid character 'X' found while parsing <!DOCTYPE"
                );
        }

        [Test]
        public void TestScriptSimple()
        {
            AssertHtml("<script>abc</script>def",
                "[0001] [tag: <script>",
                "[0002] #raw: abc",
                "[0001] ]tag: </script>",
                "[0001] #txt: def"
                );
        }

        [Test]
        public void TestStyleSimple()
        {
            AssertHtml("<style>abc</style>def",
                "[0001] [tag: <style>",
                "[0002] #raw: abc",
                "[0001] ]tag: </style>",
                "[0001] #txt: def"
                );
        }

        [Test]
        public void TestScriptMaybe1()
        {
            AssertHtml("<script>abc</sdef</script>",
                "[0001] [tag: <script>",
                "[0002] #raw: abc</sdef",
                "[0001] ]tag: </script>"
                );
            AssertHtml("<script>abc</scriptdef</script>",
                "[0001] [tag: <script>",
                "[0002] #raw: abc</scriptdef",
                "[0001] ]tag: </script>",
                "(1,20): warning : Invalid end of tag <script>. Expecting a '>'"
                );
            AssertHtml("<script>abc</script def</script>",
                "[0001] [tag: <script>",
                "[0002] #raw: abc</script def",
                "[0001] ]tag: </script>",
                "(1,21): warning : Invalid end of tag <script>. Expecting a '>'"
                );
            AssertHtml("<script>abc</script def</script     >",
                "[0001] [tag: <script>",
                "[0002] #raw: abc</script def",
                "[0001] ]tag: </script>",
                "(1,21): warning : Invalid end of tag <script>. Expecting a '>'"
                );
        }

        [Test]
        public void TestStyleMaybe1()
        {
            AssertHtml("<style>abc</sdef</style>",
                "[0001] [tag: <style>",
                "[0002] #raw: abc</sdef",
                "[0001] ]tag: </style>");

            AssertHtml("<style>abc</styledef</style>",
                "[0001] [tag: <style>",
                "[0002] #raw: abc</styledef",
                "[0001] ]tag: </style>",
                "(1,18): warning : Invalid end of tag <style>. Expecting a '>'"
                );
            AssertHtml("<style>abc</style def</style>",
                "[0001] [tag: <style>",
                "[0002] #raw: abc</style def",
                "[0001] ]tag: </style>",
                "(1,19): warning : Invalid end of tag <style>. Expecting a '>'");

            AssertHtml("<style>abc</style def</style     >",
                "[0001] [tag: <style>",
                "[0002] #raw: abc</style def",
                "[0001] ]tag: </style>",
                "(1,19): warning : Invalid end of tag <style>. Expecting a '>'"
                );
        }

        [Test]
        public void TestRemote()
        {
            var urls = new List<string>()
            {
                "http://google.com",
            };

            var webClient = new WebClient();
            foreach (var url in urls)
            {
                var html = webClient.DownloadString(url);
                var parser = new HtmlParser(html);
                var nodes = parser.Parse();
                // Assert.False(parser.HasError);
            }
        }

        private void AssertHtml(string input, params string[] expected)
        {
            var parser = new HtmlParser(input);
            var nodes = parser.Parse();

            var domWriter = new HtmlWriterToDOM();
            domWriter.Write(nodes);

            var output = domWriter.DOMDumpList;

            foreach (var message in parser.Errors)
            {
                output.Add(message.ToString());
            }

            Console.Out.WriteLine("======================");
            Console.Out.WriteLine("OUTPUT");
            Console.Out.WriteLine("----------------------");
            foreach (var txt in output)
            {
                Console.Out.WriteLine(txt);
            }
            Console.Out.WriteLine("======================");
            Console.Out.WriteLine("EXPECTED");
            Console.Out.WriteLine("----------------------");
            foreach (var txt in expected)
            {
                Console.Out.WriteLine(txt);
            }

            Console.Out.WriteLine();

            Assert.AreEqual(expected, output);
        }
    }
}
// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Collections.Generic;
using System.IO;
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
                "doct: <!DOCTYPE html>",
                "[tag: <html>",
                "comm: comment",
                "cdat: boo",
                "[tag: <body>",
                "text: Test",
                "]tag: </body>",
                "]tag: </html>"
                );
        }

        [Test]
        public void TestText()
        {
            AssertHtml("This is a test",
                "text: This is a test"
                );
        }

        [Test]
        public void TestOpenTagWithSpaces()
        {
            AssertHtml("<html   >",
                "[tag: <html>"
                );
        }

        [Test]
        public void TestOpenTagInvalidAttributes()
        {
            AssertHtml("<html @x>",
                "text: <html @x>"
                );
        }

        [Test]
        public void TestOpenTagInvalidAttributes2()
        {
            AssertHtml("<html x=>",
                "text: <html x=>"
                );
        }

        [Test]
        public void TestOpenTagWithAttributes()
        {
            AssertHtml("<html x=1 y='test' z=\"test2\" w a b c>",
                "[tag: <html x='1' y='test' z='test2' w a b c>"
                );
        }

        [Test]
        public void TestOpenTagWithAttributesInvalid()
        {
            AssertHtml("<html x=1 y='test' z=\"test2 w a b c>",
                "text: <html x=1 y='test' z=\"test2 w a b c>"
                );
        }

        [Test]
        public void TestProcessingInstructionSimple()
        {
            AssertHtml("<?xml version=\"1.0\"?>",
                "[tag: <?xml version='1.0'?>"
                );
        }

        [Test]
        public void TestProcessingInstructionInvalid()
        {
            AssertHtml("<?xml version=\"1.0\">",
                "text: <?xml version=\"1.0\">"
                );
        }

        [Test]
        public void TestBadTag()
        {
            AssertHtml("<html   t",
                "text: <html   t"
                );
        }

        [Test]
        public void TestEndTag()
        {
            AssertHtml("</html>",
                "]tag: </html>"
                );
        }

        [Test]
        public void TestEndTagInvalid()
        {
            AssertHtml("</@html>",
                "text: </@html>"
                );
        }

        [Test]
        public void TestEndTagInvalid2()
        {
            AssertHtml("</html",
                "text: </html"
                );
        }

        [Test]
        public void TestComment()
        {
            AssertHtml("<!-- test -->",
                "comm:  test "
                );
        }

        [Test]
        public void TestCDATASimple()
        {
            AssertHtml("<![CDATA[This is a test]]>",
                "cdat: This is a test"
                );
        }

        [Test]
        public void TestCDATAEmpty()
        {
            AssertHtml("<![CDATA[]]>",
                "cdat: "
                );
        }

        [Test]
        public void TestCDATAIncomplete()
        {
            AssertHtml("<![CDATA[This is a test",
                "text: <![CDATA[This is a test"
                );
        }

        [Test]
        public void TestCDATAMaybe1()
        {
            AssertHtml("<![CDATA[This ]is a test]]>",
                "cdat: This ]is a test"
                );
        }

        [Test]
        public void TestCDATAMaybe2()
        {
            AssertHtml("<![CDATA[This ]]is a test]]>",
                "cdat: This ]]is a test"
                );
        }

        [Test]
        public void TestCDATAMaybe3()
        {
            AssertHtml("<![CDATA[This ]>is a test]]>",
                "cdat: This ]>is a test"
                );
        }

        [Test]
        public void TestCDATANot1()
        {
            AssertHtml("<![C ]]>",
                "text: <![C ]]>"
                );
        }

        [Test]
        public void TestCDATANot2()
        {
            AssertHtml("<![CD ]]>",
                "text: <![CD ]]>"
                );
        }

        [Test]
        public void TestCDATANot3()
        {
            AssertHtml("<![CDA ]]>",
                "text: <![CDA ]]>"
                );
        }

        [Test]
        public void TestCDATANot4()
        {
            AssertHtml("<![CDAT ]]>",
                "text: <![CDAT ]]>"
                );
        }

        [Test]
        public void TestCDATANot5()
        {
            AssertHtml("<![CDATA ]]>",
                "text: <![CDATA ]]>"
                );
        }

        [Test]
        public void TestDOCTYPE1()
        {
            AssertHtml("<!DOCTYPE>",
                "doct: <!DOCTYPE>"
                );
        }

        [Test]
        public void TestDOCTYPE2()
        {
            AssertHtml("<!DOCTYPE html>",
                "doct: <!DOCTYPE html>"
                );
        }

        [Test]
        public void TestDOCTYPE3()
        {
            AssertHtml("<!DOCTYPE html \"test with fake url\">",
                "doct: <!DOCTYPE html \"test with fake url\">"
                );
        }

        [Test]
        public void TestDOCTYPEInvalid1()
        {
            AssertHtml("<!DOCTYPE html",
                "text: <!DOCTYPE html"
                );
        }

        [Test]
        public void TestDOCTYPEInvalid2()
        {
            AssertHtml("<!DOCTYPEX>",
                "text: <!DOCTYPEX>"
                );
        }

        [Test]
        public void TestScriptSimple()
        {
            AssertHtml("<script>abc</script>def",
                "[tag: <script> content: abc",
                "text: def"
                );
        }

        [Test]
        public void TestStyleSimple()
        {
            AssertHtml("<style>abc</style>def",
                "[tag: <style> content: abc",
                "text: def"
                );
        }

        [Test]
        public void TestScriptMaybe1()
        {
            AssertHtml("<script>abc</sdef</script>", "[tag: <script> content: abc</sdef");
            AssertHtml("<script>abc</scriptdef</script>", "[tag: <script> content: abc</scriptdef");
            AssertHtml("<script>abc</script def</script>", "[tag: <script> content: abc</script def");
            AssertHtml("<script>abc</script def</script     >", "[tag: <script> content: abc</script def");
        }


        [Test]
        public void TestStyleMaybe1()
        {
            AssertHtml("<style>abc</sdef</style>", "[tag: <style> content: abc</sdef");
            AssertHtml("<style>abc</styledef</style>", "[tag: <style> content: abc</styledef");
            AssertHtml("<style>abc</style def</style>", "[tag: <style> content: abc</style def");
            AssertHtml("<style>abc</style def</style     >", "[tag: <style> content: abc</style def");
        }

        private void AssertHtml(string input, params string[] expected)
        {
            var parser = new HtmlParser(new StringReader(input));
            var nodes = parser.Parse();

            var output = Dump(nodes);
            Assert.AreEqual(expected, output);
        }

        private List<string> Dump(List<HtmlNode> nodes)
        {
            var stringNodes = new List<string>();
            var builder = new StringBuilder();
            foreach (var node in nodes)
            {
                builder.Clear();
                if (node is HtmlTagNode)
                {
                    var tagNode = (HtmlTagNode) node;
                    builder.Append($"[tag: <");
                    if (tagNode.IsProcessingInstruction)
                    {
                        builder.Append("?");
                    }
                    builder.Append(tagNode.Name);
                    if (tagNode.Attributes.Count > 0)
                    {
                        foreach (var attr in tagNode.Attributes)
                        {
                            builder.Append(" ");
                            builder.Append(attr.Name);
                            if (attr.Value != null)
                            {
                                builder.Append("='");
                                builder.Append(attr.Value);
                                builder.Append("'");
                            }
                        }
                    }
                    if (tagNode.IsProcessingInstruction)
                    {
                        builder.Append("?");
                    }
                    builder.Append(">");

                    if (tagNode.Content != null)
                    {
                        builder.Append(" content: ").Append(tagNode.Content);
                    }
                }
                else if (node is HtmlEndTagNode)
                {
                    builder.Append($"]tag: </").Append(((HtmlEndTagNode) node).Name).Append(">");
                }
                else if (node is HtmlCommentNode)
                {
                    builder.Append($"comm: ").Append(((HtmlCommentNode)node).Text);
                }
                else if (node is HtmlCDATANode)
                {
                    builder.Append($"cdat: ").Append(((HtmlCDATANode)node).Text);
                }
                else if (node is HtmlTextNode)
                {
                    builder.Append($"text: ").Append(node);
                }
                else if (node is HtmlDOCTYPENode)
                {
                    builder.Append($"doct: ").Append(((HtmlDOCTYPENode)node).Text);
                }
                else
                {
                    builder.Append($"invalid: ").Append(node);
                }

                stringNodes.Add(builder.ToString());
            }

            return stringNodes;
        }
    }
}
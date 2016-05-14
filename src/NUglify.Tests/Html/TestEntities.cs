using NUglify.Html;
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests for decoding entities
    /// </summary>
    [TestFixture]
    public class TestEntities : TestHtmlParserBase
    {
        [Test]
        public void RemoveEntities()
        {
            input = "<p>This is a &quot; with an &amp; and a &gt;</p>";
            output = "<p>This is a \" with an &amp; and a &gt;";
            equal(minify(input), output);

            output = "<p>This is a &quot; with an &amp; and a &gt;";
            equal(minify(input, new HtmlSettings() { DecodeEntityCharacters = false }), output);
        }
    }
}
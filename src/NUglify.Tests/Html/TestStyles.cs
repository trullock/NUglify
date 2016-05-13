using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests ported from html-minifier https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
    /// </summary>
    [TestFixture]
    public class TestStyles : TestHtmlParserBase
    {
        [Test]
        public void RemoveComments()
        {
            input = "<style><!--\np.a{background:red}\n--></style>";
            equal(minify(input), input);

            input = "<style><!--p.b{background:red}--></style>";
            equal(minify(input), input);

            input = "<style><!--p.c{background:red}\n--></style>";
            equal(minify(input), input);

            input = "<style><!--\np.d{background:red}--></style>";
            equal(minify(input), input);

            input = "<style><!--p.e{background:red}\np.f{background:red}\np.g{background:red}--></style>";
            equal(minify(input), input);

            input = "<style>p.h{background:#f00}<!--p.i{background:#f00}-->p.j{background:#f00}</style>";
            equal(minify(input), input);

            input = "<style type=\"text/css\"><!-- p { color: red } --></style>";
            equal(minify(input), input);

            input = "<style type=\"text/css\">p::before { content: \"<!--\" }</style>";
            equal(minify(input), "<style type=\"text/css\">p::before{content:\"<!--\"}</style>");

            input = "<style type=\"text/html\">\n<div>\n</div>\n<!-- aa -->\n</style>";
            equal(minify(input), input);
        }
    }
}
using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests ported from html-minifier https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
    /// </summary>
    [TestFixture]
    public class TestSvg : TestHtmlParserBase
    {
        [Test]
        public void StandardSvg()
        {
            // Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
            // MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE

            input = "<html><body>\n" +
              "  <svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\n" +
              "     width=\"612px\" height=\"502.174px\" viewBox=\"0 65.326 612 502.174\" enable-background=\"new 0 65.326 612 502.174\"\n" +
              "     xml:space=\"preserve\" class=\"logo\">" +
              "" +
              "    <ellipse class=\"ground\" cx=\"283.5\" cy=\"487.5\" rx=\"259\" ry=\"80\"/>" +
              "    <polygon points=\"100,10 40,198 190,78 10,78 160,198\"\n" +
              "      style=\"fill:lime;stroke:purple;stroke-width:5;fill-rule:evenodd;\" />\n" +
              "    <filter id=\"pictureFilter\">\n" +
              "      <feGaussianBlur stdDeviation=\"15\" />\n" +
              "    </filter>\n" +
              "  </svg>\n" +
              "</body></html>";

            output = "<svg version=1.1 id=Layer_1 xmlns=http://www.w3.org/2000/svg xmlns:xlink=http://www.w3.org/1999/xlink x=0px y=0px width=612px height=502.174px viewBox=\"0 65.326 612 502.174\" enable-background=\"new 0 65.326 612 502.174\" xml:space=preserve class=logo>" +
              "<ellipse class=ground cx=283.5 cy=487.5 rx=259 ry=80 />" +
              "<polygon points=\"100,10 40,198 190,78 10,78 160,198\" style=fill:lime;stroke:purple;stroke-width:5;fill-rule:evenodd; />" +
              "<filter id=pictureFilter><feGaussianBlur stdDeviation=15 /></filter>" +
              "</svg>";

            // Should preserve case-sensitivity and closing slashes within svg tags
            equal(minify(input), output);
        }
    }
}
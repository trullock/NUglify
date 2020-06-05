using System.IO;
using System.Text;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    [TestFixture]
    public class SymbolsMapTests
    {
        [Test]
        public void CallMinifyAPI()
        {
            var sourcePath = @"someSource.js";
            var mapPath = @"someSource.map.js";

            // just some random source
            var sourceCode = "var foo = 42;function globalFunc(text){ alert(text); }var arrow = e => { return e * 2; }";

            string minifiedCode = null;

            var mapBuilder = new StringBuilder();
            using(var mapWriter = new StringWriter(mapBuilder))
            {
                using(var sourceMap = new V3SourceMap(mapWriter))
                {
                    var settings = new CodeSettings();
                    settings.SymbolsMap = sourceMap;
                    sourceMap.StartPackage(sourcePath, mapPath);

                    minifiedCode = Uglify.Js(sourceCode, settings).Code;
                }
            }

            // just verify that we got some source map content
            var mapContent = mapBuilder.ToString();
            Assert.IsNotNull(mapContent, "map content should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(mapContent), "map content should not be empty");

            // better have some minified code results
            Assert.IsNotNull(minifiedCode, "minified code should not be null");

            // verify the code UP TO the first line break, since the comment should be added on a new line and there
            // shouldn't be any linebreaks other than that.
            var indexLineBreak = minifiedCode.IndexOf('\n');
            var trimmedCode = indexLineBreak < 0 ? minifiedCode : minifiedCode.Substring(0, indexLineBreak);
            Assert.AreEqual("function globalFunc(n){alert(n)}var foo=42,arrow=n=>n*2", trimmedCode, "minified code not expected");

            // verify that the minified code has the sourceMappingURL directive in it
            Assert.IsTrue(minifiedCode.Contains("sourceMappingURL=" + mapPath), "sourceMappingURL should be in minified content");
        }
    }
}

using System.IO;
using System.Text;
using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2018
    {
        [Test]
        public void SpreadOperator()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForAwait()
        {
            TestHelper.Instance.RunTest();
        }


        [Test]
        public void ForAwaitSourceMap()
        {
            UglifyResult result;

            string sFileContent = @"(async function () {
    try {
        for await (let num of generator()) {
            console.log(num);
        }
    } catch (e) {
        console.log('caught', e);
    }
})();";

            var builder = new StringBuilder();
            using (TextWriter mapWriter = new StringWriter(builder))
            {
                using (var sourceMap = new V3SourceMap(mapWriter))
                {
                    sourceMap.MakePathsRelative = false;

                    var settings = new CodeSettings();
                    settings.SymbolsMap = sourceMap;
                    sourceMap.StartPackage(@"C:\some\long\path\to\js", @"C:\some\other\path\to\map");

                    result = Uglify.Js(sFileContent, @"C:\some\path\to\output\js", settings);
                }
            }

            Assert.AreEqual("(async function(){try{for await(let n of generator())console.log(n)}catch(n){console.log(\"caught\",n)}})()\n//# sourceMappingURL=C:\\some\\other\\path\\to\\map\n", result.Code);

            Assert.AreEqual("{\r\n\"version\":3,\r\n\"file\":\"C:\\some\\long\\path\\to\\js\",\r\n\"mappings\":\"AAAAA,SAASA,IAAI,CAACC,CAAD,CAAG,CACf,OAAOA,CAAC,EAAE,CADK\",\r\n\"sources\":[\"C:\\some\\path\\to\\output\\js\"],\r\n\"names\":[\"test\",\"t\"]\r\n}\r\n", builder.ToString());
        }
    }
}

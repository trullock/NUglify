// SourceMap.cs
//
// Copyright 2010 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Text;
using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// unit tests dealing with the MAP input used to generate node mapping between source and output scripts
    /// </summary>
    [TestFixture]
    public class SourceMap
    {
        [Test]
        public void MapArgNotSpecified()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ScriptSharpMap_TwoInputs()
        {
            TestHelper.Instance.RunTest("-map", "MapArgNotSpecified.js");
        }

        [Test]
        public void SourceMapV3()
        {
            TestHelper.Instance.RunTest("-map:v3", "ScriptSharpMap.js", "MapArgNotSpecified.js");
        }


        [Test]
        public void SourceMapForMultipleFiles()
        {
            var assetBuilder = new StringBuilder();
            var assets = new[]{ "SourceMapForMultipleFiles1.js", "SourceMapForMultipleFiles2.js", "SourceMapForMultipleFiles3.js" };

            var currentPath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var asset in assets)
            {
                assetBuilder.AppendLine("///#source 1 1 " + asset);
                var path = Path.Combine(currentPath, "TestData\\JS\\Input\\SourceMap\\" + asset);
                assetBuilder.AppendLine(File.ReadAllText(path));
            }

            var stringBuilder = new StringBuilder();

            using (var textWriter = new StringWriter(stringBuilder))
            {
                var sourceMap = new V3SourceMap(textWriter);

                sourceMap.StartPackage("infile.js", "SourceMapForMultipleFiles.js.map");
                var uglifyResult = Uglify.Js(assetBuilder.ToString(), "SourceMapForMultipleFiles.js", new CodeSettings
                {
                    SymbolsMap = sourceMap,
                    SourceMode = JavaScriptSourceMode.Program,
                    MinifyCode = true,
                    OutputMode = OutputMode.SingleLine,
                    StripDebugStatements = false,
                    LineTerminator = "\r\n"
                });
                sourceMap.EndPackage();
                sourceMap.Dispose();

                var code = uglifyResult.Code;
                Assert.That(code, Is.EqualTo(File.ReadAllText(Path.Combine(currentPath, "TestData\\JS\\Expected\\SourceMap\\SourceMapForMultipleFiles.js"))));
                var builder = stringBuilder.ToString();
                Assert.That(builder, Is.EqualTo(File.ReadAllText(Path.Combine(currentPath, "TestData\\JS\\Expected\\SourceMap\\SourceMapForMultipleFiles.js.map"))));
            }
		}
		
        [Test]
        public void RelativePaths()
        {
            UglifyResult result;

            string sFileContent = @"function test(t){
	return t**2;
}";

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

            Assert.That(result.Code, Is.EqualTo("function test(n){return n**2}\n//# sourceMappingURL=C:\\some\\other\\path\\to\\map\n"));
            
            Assert.That(builder.ToString(), Is.EqualTo("{\r\n\"version\":3,\r\n\"file\":\"C:\\\\some\\\\long\\\\path\\\\to\\\\js\",\r\n\"mappings\":\"AAAAA,SAASA,IAAI,CAACC,CAAD,CAAG,CACf,OAAOA,CAAC,EAAE,CADK\",\r\n\"sources\":[\"C:\\\\some\\\\path\\\\to\\\\output\\\\js\"],\r\n\"names\":[\"test\",\"t\"]\r\n}\r\n"));
        }
    }
}


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
                Assert.AreEqual(File.ReadAllText(Path.Combine(currentPath, "TestData\\JS\\Expected\\SourceMap\\SourceMapForMultipleFiles.js")), code);
                var builder = stringBuilder.ToString();
                Assert.AreEqual(File.ReadAllText(Path.Combine(currentPath, "TestData\\JS\\Expected\\SourceMap\\SourceMapForMultipleFiles.js.map")), builder);
            }
        }
    }
}


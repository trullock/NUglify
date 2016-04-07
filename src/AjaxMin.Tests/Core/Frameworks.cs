// Frameworks.cs
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

using System.Diagnostics;
using System.IO;
using System.Text;

using Microsoft.Ajax.Utilities;
using NUnit.Framework;

namespace DllUnitTest
{
    /// <summary>
    /// Summary description for Frameworks
    /// </summary>
    [TestFixture]
    public class Frameworks
    {
        public Frameworks()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod,
         DeploymentItem(@"..\..\TestData\Dll\Input\Frameworks\NoErrors.csv"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\Dll\Input\Frameworks\NoErrors.csv", "NoErrors#csv", DataAccessMethod.Sequential)]
        public void NoErrors()
        {
            // get the source code in the file specified by the first column
            string sourceCode;
            var fileName = TestContext.DataRow[0].ToString();
            var filePath = Path.Combine(TestContext.DeploymentDirectory, @"Dll\Input\Frameworks", fileName);
            Assert.IsTrue(File.Exists(filePath), "Input file must exist");

            Trace.Write("Reading source file: ");
            Trace.WriteLine(filePath);
            using (var reader = new StreamReader(filePath))
            {
                sourceCode = reader.ReadToEnd();
            }

            // run the framework file through a parser with standard settings (except for multi-line mode)
            // there should be only the errors specified in the other columns in the CSV file (if any)
            var errorCount = 0;
            var parser = new JSParser();
            parser.CompilerError += (sender, ea) =>
                {
                    if (ea.Error.IsError)
                    {
                        Trace.WriteLine(ea.ToString());
                        ++errorCount;
                    }
                };
            var block = parser.Parse(new DocumentContext(sourceCode) { FileContext = filePath });//, new CodeSettings() { OutputMode = OutputMode.MultipleLines });
            var minifiedCode = OutputVisitor.Apply(block, parser.Settings);

            // save the results in Output folder
            var outputPath = Path.Combine(TestContext.DeploymentDirectory, @"Dll\Output\Frameworks", fileName);
            Trace.Write("Output path: ");
            Trace.WriteLine(outputPath);

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                writer.Write(minifiedCode);
            }

            // report if there were any errors, then reset the count.
            if (errorCount > 0)
            {
                Trace.Write("Original minification produced ");
                Trace.Write(errorCount);
                Trace.WriteLine(" errors!");
                errorCount = 0;
            }

            // now run the output through another parser with no minify
            // settings -- there should DEFINITELY be no errors this time.
            parser = new JSParser();
            parser.CompilerError += (sender, ea) =>
            {
                if (ea.Error.IsError)
                {
                    Trace.WriteLine(ea.Error.ToString());
                    ++errorCount;
                }
            };
            block = parser.Parse(minifiedCode, new CodeSettings() { MinifyCode = false });

            Assert.IsTrue(errorCount == 0, "Parsing minified " + fileName + " produces errors!");
        }
    }
}

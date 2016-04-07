using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AjaxMin.JavaScript;
using NUnit.Framework;

namespace AjaxMin.Tests.Core
{
    /// <summary>
    /// Summary description for SourceDirective
    /// </summary>
    [TestFixture]
    public class SourceDirective
    {
        private const string OutputFolder = @"TestData\Core\Output";
        private const string ExpectedFolder = @"TestData\Core\Expected";
        private const string InputFolder = @"TestData\Core\Input";

        public SourceDirective()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(typeof (SourceDirective).Assembly.Location);
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

        [Test]
        public void SourceChange()
        {
            const string FileName = "SourceChange.js";
            const string OriginalFileContext = "NULL";

            // get the source code
            string source;
            var inputPath = Path.Combine(InputFolder, FileName);
            Trace.Write("Source: ");
            Trace.WriteLine(inputPath);
            using (var reader = new StreamReader(inputPath))
            {
                source = reader.ReadToEnd();
            }
            Trace.WriteLine(source);
            Trace.WriteLine("");
            Trace.WriteLine("-----------------------");
            Trace.WriteLine("");

            // get the expected results
            string expected;
            var expectedPath = new FileInfo(Path.Combine(ExpectedFolder, FileName));
            Trace.Write("Expected: ");
            Trace.WriteLine(inputPath);
            using (var reader = new StreamReader(expectedPath.FullName))
            {
                expected = reader.ReadToEnd();
            }

            Trace.WriteLine(expected);
            Trace.WriteLine("");
            Trace.WriteLine("-----------------------");
            Trace.WriteLine("");

            // parse the source, keeping track of the errors
            var errors = new List<ContextError>();
            var parser = new JSParser();
            parser.CompilerError += (sender, ea) =>
                {
                    errors.Add(ea.Error);
                };

            var settings = new CodeSettings()
            {
                LocalRenaming = LocalRenaming.KeepAll
            };
            var block = parser.Parse(new DocumentContext(source) { FileContext = OriginalFileContext }, settings);
            var minified = OutputVisitor.Apply(block, parser.Settings);

            // write the output so we can diagnose later if we need to
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            var actualPath = new FileInfo(Path.Combine(OutputFolder, FileName));
            Trace.Write("Actual: ");
            Trace.WriteLine(actualPath);
            using (var writer = new StreamWriter(actualPath.FullName, false, Encoding.UTF8))
            {
                writer.Write(minified);
            }

            Trace.WriteLine(minified);
            Trace.WriteLine("");
            Trace.WriteLine("-----------------------");
            Trace.WriteLine("");

            Trace.WriteLine("Output Comparison:");
            Trace.WriteLine(string.Format("odd.exe \"{0}\" \"{1}\"", expectedPath.FullName, actualPath.FullName));
            Trace.WriteLine("");

            // and compare them -- they should be equal
            Assert.IsTrue(string.CompareOrdinal(minified, expected) == 0, "actual is not the expected");

            var expectedErrors = new[] {
                new {FileContext = "anonfunc.js", StartLine = 2, EndLine = 2, StartColumn = 20, EndColumn = 21, ErrorCode = "JS1010"},
                new {FileContext = "anonfunc.js", StartLine = 5, EndLine = 5, StartColumn = 3, EndColumn = 4, ErrorCode = "JS1195"},
                new {FileContext = "addclass.js", StartLine = 2, EndLine = 2, StartColumn = 8, EndColumn = 14, ErrorCode = "JS1135"},
                new {FileContext = "addclass.js", StartLine = 10, EndLine = 10, StartColumn = 42, EndColumn = 48, ErrorCode = "JS1135"},
            };

            // now, the errors should be the same -- in particular we are looking for the line/column
            // numbers and source path. they should be what got reset by the ///#SOURCE comments, not the
            // real values from the source file.
            Trace.WriteLine("Errors:");
            foreach (var error in errors)
            {
                Trace.WriteLine(error.ToString());

                var foundIt = false;
                foreach (var expectedError in expectedErrors)
                {
                    if (expectedError.StartLine == error.StartLine
                        && expectedError.EndLine == error.EndLine
                        && expectedError.StartColumn == error.StartColumn
                        && expectedError.EndColumn == error.EndColumn
                        && string.CompareOrdinal(expectedError.FileContext, error.File) == 0
                        && string.CompareOrdinal(expectedError.ErrorCode, error.ErrorCode) == 0)
                    {
                        foundIt = true;
                        break;
                    }
                }

                Assert.IsTrue(foundIt, "Unexpected error");
            }

            Trace.WriteLine("");
        }
    }
}

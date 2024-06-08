using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUglify.Css;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for CssEncoding
    /// </summary>
    [TestFixture]
    public class CssEncoding
    {
        #region private fields

        static string s_inputFolder;

        static string s_outputFolder;

        static string s_expectedFolder;

        #endregion

        public CssEncoding()
        {
        }

        TestContext testContextInstance;

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
        [SetUp]
        public void Setup()
        {
            var testClassName = TestContext.CurrentContext.Test.ClassName.Substring(
                TestContext.CurrentContext.Test.ClassName.LastIndexOf('.') + 1);

            s_inputFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Core", "Input", testClassName);
            s_outputFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Core", "Output", testClassName);
            s_expectedFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Core", "Expected", testClassName);

            // make sure the output folder exists
            Directory.CreateDirectory(s_outputFolder);
        }

        [Test]
        public void Utf8WithAsciiEncoding()
        {
            var failed = false;

            var encoding = Encoding.GetEncoding(
                        "Windows-1252",
                        new EncoderReplacementFallback("?"),
                        new DecoderReplacementFallback("\uFFFD"));

            // @charset is ascii, but saved as UTF-8; no problems
            failed = ParseFile("utf8ascii.css", encoding) || failed;

            // no @charset, but saved as UTF-8; potential problems
            failed = ParseFile("utf8none.css", encoding, CssErrorCode.PossibleCharsetError) || failed;

            // @charset utf-8, but saved as UTF-8; potential problems
            failed = ParseFile("utf8utf8.css", encoding, CssErrorCode.PossibleCharsetError) || failed;

            // saved as UTF-16; potential problems
            failed = ParseFile("utf16.css", encoding, CssErrorCode.PossibleCharsetError, CssErrorCode.UnexpectedEndOfFile) || failed;

            // empty file with just UTF-8 BOM
            failed = ParseFile("empty.css", encoding) || failed;

            Assert.That(!failed, "at least one test failed");
        }

        [Test]
        public void ParseNull()
        {
            var errors = new List<UglifyError>();
            var cssParser = new CssParser();
            cssParser.CssError += (sender, ea) =>
            {
                errors.Add(ea.Error);
            };

            // parse it
            var results = cssParser.Parse(null);
            Assert.That(results, Is.EqualTo(string.Empty));
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        bool ParseFile(string fileName, Encoding encoding, params CssErrorCode[] expectedErrors)
        {
            var failed = false;
            Trace.WriteLine("File: " + fileName);
            Trace.Indent();
            try
            {
                // read the source as ascii
                var source = ReadFileAsAscii(Path.Combine(s_inputFolder, fileName), encoding);

                // set up the parser and the error condition
                var errors = new List<UglifyError>();
                var cssParser = new CssParser();
                cssParser.CssError += (sender, ea) =>
                {
                    errors.Add(ea.Error);
                };

                // parse it
                var results = cssParser.Parse(source);
                Trace.WriteLine(results);

                // the errors must match
                var mismatch = false;
                var ndxExpected = 0;
                var ndxActual = 0;

                if (errors.Count == 0)
                {
                    Trace.WriteLine("No actual errors");
                }
                else
                {
                    while (ndxExpected < expectedErrors.Length && ndxActual < errors.Count)
                    {
                        if (errors[ndxActual].ErrorNumber == (int)expectedErrors[ndxExpected])
                        {
                            // log the match and move to the next
                            Trace.WriteLine("Matched error: " + expectedErrors[ndxExpected]);
                            ++ndxActual;
                            ++ndxExpected;
                        }
                        else
                        {
                            // log the match and move to the next ACTUAL error (but not the expected)
                            Trace.WriteLine("Unexpected error: " + (CssErrorCode)errors[ndxActual].ErrorNumber);
                            ++ndxActual;

                            // make sure we fail this test
                            mismatch = true;
                        }
                    }
                }

                if (ndxExpected < expectedErrors.Length)
                {
                    // expected errors not found
                    mismatch = true;
                    for (; ndxExpected < expectedErrors.Length; ++ndxExpected)
                    {
                        Trace.WriteLine("Expected error not found: " + expectedErrors[ndxExpected]);
                    }
                }

                if (ndxActual < errors.Count)
                {
                    // unexpected errors
                    mismatch = true;
                    for (; ndxActual < errors.Count; ++ndxActual)
                    {
                        Trace.WriteLine("Unexpected error: " + (CssErrorCode)errors[ndxActual].ErrorNumber);
                    }
                }

                if (errors.Count != expectedErrors.Length)
                {
                    Trace.WriteLine("FAILED: Number of actual errors doesn't match expected errors");
                    failed = true;
                }
                else if (mismatch)
                {
                    Trace.WriteLine("FAILED: Errors don't match");
                    failed = true;
                }
                else
                {
                    Trace.WriteLine("Succeeded");
                }
            }
            finally
            {
                Trace.Unindent();
            }

            return failed;
        }

        string ReadFileAsAscii(string path, Encoding encoding)
        {
            using (var reader = new StreamReader(path, encoding, false))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

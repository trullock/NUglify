using System;
using System.Collections.Generic;
using System.IO;
using NUglify.Css;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    [TestFixture]
    public class Preprocessor
    {
	    static string s_inputFolder;
	    static string s_outputFolder;
	    static string s_expectedFolder;

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
        public void SourceDirectiveJS()
        {
            string source;
            using(var reader = new StreamReader(Path.Combine(s_inputFolder, @"SourceDirective.js")))
            {
                source = reader.ReadToEnd();
            }

            var errors = new List<Tuple<string, int, int>>
                {
                    new Tuple<string, int, int>("foo.js", 12, 3),
                    new Tuple<string, int, int>("foo.js", 12, 7),
                    new Tuple<string, int, int>("fargo.htm", 5, 44),
                    new Tuple<string, int, int>("fargo.htm", 5, 48),
                };

            var errorCount = 0;
            var parser = new JSParser();
            parser.CompilerError += (sender, ea) =>
                {
                    Assert.That(errors.Count > errorCount, "too many errors");
                    Assert.That(ea.Error.File, Is.EqualTo(errors[errorCount].Item1), "file path");
                    Assert.That(ea.Error.StartLine, Is.EqualTo(errors[errorCount].Item2), "line number");
                    Assert.That(ea.Error.StartColumn, Is.EqualTo(errors[errorCount].Item3), "column number");

                    ++errorCount;
                };
            var block = parser.Parse(source, new CodeSettings());
            Assert.That(errorCount, Is.EqualTo(errors.Count), "errors found");
        }

        [Test]
        public void SourceDirectiveCSS()
        {
            string source;
            using (var reader = new StreamReader(Path.Combine(s_inputFolder, @"SourceDirective.css")))
            {
                source = reader.ReadToEnd();
            }

            var errors = new List<Tuple<string, int, int>>
                {
                    new Tuple<string, int, int>("foo.css", 12, 7),
                    new Tuple<string, int, int>("foo.css", 12, 7),
                    new Tuple<string, int, int>("bigfile.css", 1025, 1),
                    new Tuple<string, int, int>("fargo.htm", 5, 44),
                    new Tuple<string, int, int>("bat.scss", 19, 1),
                };

            var errorCount = 0;
            var parser = new CssParser();
            parser.CssError += (sender, ea) =>
            {
                Assert.That(errors.Count > errorCount, "too many errors");
                Assert.That(ea.Error.File, Is.EqualTo(errors[errorCount].Item1), "file path");
                Assert.That(ea.Error.StartLine, Is.EqualTo(errors[errorCount].Item2), "line number");
                Assert.That(ea.Error.StartColumn, Is.EqualTo(errors[errorCount].Item3), "column number");

                ++errorCount;
            };

            parser.Settings = new CssSettings();
            var minified = parser.Parse(source);
            Assert.That(errorCount, Is.EqualTo(errors.Count), "errors found");
        }
    }
}

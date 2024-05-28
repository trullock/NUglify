using System.Collections.Generic;
using System.IO;
using NUglify.Css;
using NUglify.JavaScript;
using NUglify.JavaScript.Visitors;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for ReplacementTokens
    /// </summary>
    [TestFixture]
    public class ReplacementTokens
    {
        #region private fields

        static string s_inputFolder;

        static string s_outputFolder;

        static string s_expectedFolder;

        #endregion

        #region Additional test attributes

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        // Use ClassInitialize to run code before running the first test in the class
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


        #endregion

        [Test]
        public void ReplacementStringsJS()
        {
            // reuse the same parser object
            var parser = new JSParser();

            // default should leave tokens intact
            var settings = new CodeSettings();
            var source = "var a = 'He said, %My.Token:foo%'";
            var actual = Parse(parser, settings, source);
            Assert.That(actual, Is.EqualTo("var a=\"He said, %My.Token:foo%\""));

            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "my.token", "\"Now he's done it!\"" },
                    { "num_token", "123" },
                    { "my-json", "{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }" },
                });
            settings.ReplacementFallbacks.Add("zero", "0");

            actual = Parse(parser, settings, source);
            Assert.That(actual, Is.EqualTo("var a='He said, \"Now he\\'s done it!\"'"));

            actual = Parse(parser, settings, "var b = '%Num_Token%';");
            Assert.That(actual, Is.EqualTo("var b=\"123\""));

            actual = Parse(parser, settings, "var c = '%My-JSON%';");
            Assert.That(actual, Is.EqualTo("var c='{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }'"));
        }

        [Test]
        public void ReplacementNodesJS()
        {
            // reuse the same parser object
            var parser = new JSParser();

            // default should leave tokens intact
            var settings = new CodeSettings();
            var source = "var a = %My.Token:foo%;";
            var actual = Parse(parser, settings, source);
            Assert.That(actual, Is.EqualTo("var a=%My.Token:foo%"));

            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "my.token", "\"Now he's done it!\"" },
                    { "num_token", "123" },
                    { "my-json", "{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }" },
                });
            settings.ReplacementFallbacks.Add("zero", "0");

            actual = Parse(parser, settings, source);
            Assert.That(actual, Is.EqualTo("var a=\"Now he's done it!\""));

            actual = Parse(parser, settings, "var b = %Num_Token%;");
            Assert.That(actual, Is.EqualTo("var b=123"));

            actual = Parse(parser, settings, "var c = %My-JSON%;");
            Assert.That(actual, Is.EqualTo("var c={\"a\":1,\"b\":2,\"c\":[1,2,3]}"));

            actual = Parse(parser, settings, "var d = '*%MissingToken:zero%*';");
            Assert.That(actual, Is.EqualTo("var d=\"*0*\""));

            actual = Parse(parser, settings, "var e = '*%MissingToken:ack%*';");
            Assert.That(actual, Is.EqualTo("var e=\"**\""));

            actual = Parse(parser, settings, "var f = '*%MissingToken:%*';");
            Assert.That(actual, Is.EqualTo("var f=\"**\""));
        }

        [Test]
        public void ReplacementFallbacksJS()
        {
            // reuse the same parser object
            var parser = new JSParser();

            // default should leave tokens intact
            var settings = new CodeSettings();

            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "my.token", "\"Now he's done it!\"" },
                    { "num_token", "123" },
                    { "my-json", "{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }" },
                });
            settings.ReplacementFallbacks.Add("zero", "0");

            var actual = Parse(parser, settings, "var a = %MissingToken:zero%;");
            Assert.That(actual, Is.EqualTo("var a=0"));

            actual = Parse(parser, settings, "var b = %MissingToken:ack% + 0;");
            Assert.That(actual, Is.EqualTo("var b=+0"));

            actual = Parse(parser, settings, "var c = %MissingToken:% + 0;");
            Assert.That(actual, Is.EqualTo("var c=+0"));

            actual = Parse(parser, settings, "var d = %MissingToken:%;debugger;throw 'why?';");
            Assert.That(actual, Is.EqualTo("var d=;throw\"why?\";"));
        }

        [Test]
        public void ReplacementTokensCSS()
        {
            var source = ReadFile(s_inputFolder, "replacements.css");

            var settings = new CssSettings();
            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "MediaQueries.SnapMax", "600px" },
                    { "bing-orange", "#930" },
                    { "MetroSdk.Resolution", "24x" },
                    { "Global.Right", "right" },
                    { "dim-gray", "#cccccc" },
                    { "theme_name", "green" },
                    { "Module-ID", "weather" },
                });
            settings.ReplacementFallbacks.Add("full", "100%");
            settings.ReplacementFallbacks.Add("1x", "1x");
            settings.ReplacementFallbacks.Add("color", "#ff0000");

            var actual = Uglify.Css(source, settings);

            var expected = ReadFile(s_expectedFolder, "replacements.css");
            Assert.That(actual.Code, Is.EqualTo(expected));
        }

        string ReadFile(string folder, string fileName)
        {
            var inputPath = Path.Combine(folder, fileName);
            using (var reader = new StreamReader(inputPath))
            {
                return reader.ReadToEnd();
            }
        }

        string Parse(JSParser parser, CodeSettings settings, string source)
        {
            var block = parser.Parse(source, settings);
            return OutputVisitor.Apply(block, settings);
        }
    }
}

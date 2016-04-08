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

        private static string s_inputFolder;

        private static string s_outputFolder;

        private static string s_expectedFolder;

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
            Assert.AreEqual("var a=\"He said, %My.Token:foo%\"", actual);

            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "my.token", "\"Now he's done it!\"" },
                    { "num_token", "123" },
                    { "my-json", "{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }" },
                });
            settings.ReplacementFallbacks.Add("zero", "0");

            actual = Parse(parser, settings, source);
            Assert.AreEqual("var a='He said, \"Now he\\'s done it!\"'", actual);

            actual = Parse(parser, settings, "var b = '%Num_Token%';");
            Assert.AreEqual("var b=\"123\"", actual);

            actual = Parse(parser, settings, "var c = '%My-JSON%';");
            Assert.AreEqual("var c='{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }'", actual);
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
            Assert.AreEqual("var a=%My.Token:foo%", actual);

            settings.ReplacementTokensApplyDefaults(new Dictionary<string, string> { 
                    { "my.token", "\"Now he's done it!\"" },
                    { "num_token", "123" },
                    { "my-json", "{\"a\": 1, \"b\": 2, \"c\": [ 1, 2, 3 ] }" },
                });
            settings.ReplacementFallbacks.Add("zero", "0");

            actual = Parse(parser, settings, source);
            Assert.AreEqual("var a=\"Now he's done it!\"", actual);

            actual = Parse(parser, settings, "var b = %Num_Token%;");
            Assert.AreEqual("var b=123", actual);

            actual = Parse(parser, settings, "var c = %My-JSON%;");
            Assert.AreEqual("var c={\"a\":1,\"b\":2,\"c\":[1,2,3]}", actual);

            actual = Parse(parser, settings, "var d = '*%MissingToken:zero%*';");
            Assert.AreEqual("var d=\"*0*\"", actual);

            actual = Parse(parser, settings, "var e = '*%MissingToken:ack%*';");
            Assert.AreEqual("var e=\"**\"", actual);

            actual = Parse(parser, settings, "var f = '*%MissingToken:%*';");
            Assert.AreEqual("var f=\"**\"", actual);
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
            Assert.AreEqual("var a=0", actual);

            actual = Parse(parser, settings, "var b = %MissingToken:ack% + 0;");
            Assert.AreEqual("var b=+0", actual);

            actual = Parse(parser, settings, "var c = %MissingToken:% + 0;");
            Assert.AreEqual("var c=+0", actual);

            actual = Parse(parser, settings, "var d = %MissingToken:%;debugger;throw 'why?';");
            Assert.AreEqual("var d=;throw\"why?\";", actual);
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
            Assert.AreEqual(expected, actual.Code);
        }

        private string ReadFile(string folder, string fileName)
        {
            var inputPath = Path.Combine(folder, fileName);
            using (var reader = new StreamReader(inputPath))
            {
                return reader.ReadToEnd();
            }
        }

        private string Parse(JSParser parser, CodeSettings settings, string source)
        {
            var block = parser.Parse(source, settings);
            return OutputVisitor.Apply(block, settings);
        }
    }
}

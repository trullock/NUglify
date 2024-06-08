using System.Diagnostics;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for DebugBlock
    /// </summary>
    [TestFixture]
    public class DebugBlock
    {
        public DebugBlock()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }

        [Test]
        public void StripDebugStatements()
        {
            var source = "///#DEBUG\r\nalert('hi!');\r\n///#ENDDEBUG\r\n";

            // default settings should strip the alert leaving nothing left
            var minified = ProcessCode(source, new CodeSettings());
            Assert.That(minified, Is.EqualTo(string.Empty));
            Trace.WriteLine("Default settings: " + minified);

            // with strip debug statements set to false, should NOT strip the alert
            minified = ProcessCode(source, new CodeSettings() { StripDebugStatements = false });
            Assert.That(minified, Is.EqualTo("alert(\"hi!\")"));
            Trace.WriteLine("StripDebugStatements set to false: " + minified);

            // with DEBUG defined, should NOT strip the alert
            minified = ProcessCode(source, new CodeSettings() { PreprocessorDefineList = "DEBUG" });
            Assert.That(minified, Is.EqualTo("alert(\"hi!\")"));
            Trace.WriteLine("DEBUG preprocessor name defined: " + minified);
        }

        /// <summary>
        /// minify the given JavaScript source using the given settings WITH NO ERRORS
        /// and return the minified results
        /// </summary>
        /// <param name="source">JavaScript source code to minify</param>
        /// <param name="codeSettings">Code settings to use for minification</param>
        /// <returns>minified results</returns>
        static string ProcessCode(string source, CodeSettings codeSettings)
        {
            var minified = Uglify.Js(source, codeSettings);

            // shouldn't be any errors
            Assert.That(!minified.HasErrors);

            return minified.Code;
        }
    }
}

using System.Diagnostics;

using Microsoft.Ajax.Utilities;
using NUnit.Framework;

namespace DllUnitTest
{
    /// <summary>
    /// Summary description for LogicalNot
    /// </summary>
    [TestFixture]
    public class LogicalNot
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [Test,
         DeploymentItem(@"..\..\TestData\Dll\NotExpressions.csv"), 
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\NotExpressions.csv", "NotExpressions#csv", DataAccessMethod.Sequential)]
        public void NotExpressions()
        {
            var expressionSource = TestContext.DataRow[0].ToString();
            var expectedResult = TestContext.DataRow[1].ToString();

            // parse the source into an AST
            var parser = new JSParser();
            var block = parser.Parse(expressionSource, new CodeSettings() { MinifyCode = false, SourceMode = JavaScriptSourceMode.Expression });

            if (block.Count == 1)
            {
                var expression = block[0];

                // create the logical-not visitor on the expression
                var logicalNot = new Microsoft.Ajax.Utilities.LogicalNot(expression, parser.Settings);

                // get the original code
                var original = OutputVisitor.Apply(expression, parser.Settings);

                Trace.Write("ORIGINAL EXPRESSION:    ");
                Trace.WriteLine(original);

                // get the measured delta
                var measuredDelta = logicalNot.Measure();

                // perform the logical-not operation
                logicalNot.Apply();

                // get the resulting code -- should still be only one statement in the block
                var notted = OutputVisitor.Apply(block[0], parser.Settings);

                Trace.Write("LOGICAL-NOT EXPRESSION: ");
                Trace.WriteLine(notted);

                Trace.Write("EXPECTED EXPRESSION:    ");
                Trace.WriteLine(expectedResult);

                Trace.Write("DELTA: ");
                Trace.WriteLine(measuredDelta);

                // what's the actual difference
                var actualDelta = notted.Length - original.Length;
                Assert.AreEqual(actualDelta, measuredDelta,
                    "Measurement was off; calculated {0} but was actually {1}",
                    measuredDelta,
                    actualDelta);

                Assert.AreEqual(expectedResult, notted, "Expected output is not the same!!!!");
            }
            else
            {
                Assert.Fail(string.Format("Source line '{0}' parsed to more than one statement!", expressionSource));
            }
        }
    }
}

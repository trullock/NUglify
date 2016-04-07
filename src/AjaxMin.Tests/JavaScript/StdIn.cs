using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Ajax.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSUnitTest
{
    /// <summary>
    /// Summary description for StdIn
    /// </summary>
    [TestClass]
    public class StdIn
    {
        public StdIn()
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

        public static string ExpectedFolder { get; private set; }
        public static string InputFolder { get; private set; }
        public static string OutputFolder { get; private set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var dataFolder = Path.Combine(testContext.TestDeploymentDir, @"JS");
            var className = testContext.FullyQualifiedTestClassName.Substring(testContext.FullyQualifiedTestClassName.LastIndexOf('.') + 1);

            ExpectedFolder = Path.Combine(Path.Combine(dataFolder, "Expected"), className);
            InputFolder = Path.Combine(Path.Combine(dataFolder, "Input"), className);
            OutputFolder = Path.Combine(Path.Combine(dataFolder, "Output"), className);

            // output folder may not exist -- create it if it doesn't
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }
        }

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

        [TestMethod]
        public void StdInTest()
        {
            var ndxUnderscore = TestContext.TestName.IndexOf('_');
            var testName = ndxUnderscore < 0 ? TestContext.TestName : TestContext.TestName.Substring(0, ndxUnderscore);

            // get the input, output, and expected path names
            var inputPath = Path.ChangeExtension(Path.Combine(InputFolder, testName), ".js");
            var outputPath = Path.ChangeExtension(Path.Combine(OutputFolder, testName), ".js");
            var expectedPath = Path.ChangeExtension(Path.Combine(ExpectedFolder, testName), ".js");

            // get the input code
            string inputCode;
            using (var inputStream = new StreamReader(inputPath, true))
            {
                inputCode = inputStream.ReadToEnd();
            }

            // get the path to the AjaxMin assembly.
            // we are linking to the EXE in this project, so this gives us the path
            // we need to spawn a new process for which we can redirect the stdin stream.
            var ajaxMin = Assembly.GetAssembly(typeof(IScopeReport));

            // create the process to the EXE with a redirected stdin
            var ajaxMinProcess = new Process();
            ajaxMinProcess.StartInfo.FileName = ajaxMin.Location;
            ajaxMinProcess.StartInfo.ErrorDialog = false;
            ajaxMinProcess.StartInfo.UseShellExecute = false;
            ajaxMinProcess.StartInfo.RedirectStandardInput = true;
            ajaxMinProcess.StartInfo.RedirectStandardOutput = false;
            ajaxMinProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            // no input files mean pull from stdin. Add the -js flag so we know to expect JavaScript.
            // quote the output path because it might contains spaces.
            ajaxMinProcess.StartInfo.Arguments = "-a -js -out \"" + outputPath + '"';

            // trace the command line
            Trace.WriteLine("EXECUTING:");
            Trace.Write('"');
            Trace.Write(ajaxMinProcess.StartInfo.FileName);
            Trace.Write("\" ");
            Trace.WriteLine(ajaxMinProcess.StartInfo.Arguments);
            Trace.WriteLine(string.Empty);

            // trace the input path and code
            Trace.Write("INPUT CODE: ");
            Trace.WriteLine(inputPath);
            Trace.WriteLine(inputCode);
            Trace.WriteLine(string.Empty);

            // for debugging purposes
            Trace.WriteLine(string.Format("odd \"{0}\" \"{1}\"", expectedPath, outputPath));
            Trace.WriteLine(string.Empty);

            // start the process 
            ajaxMinProcess.Start();

            // write the input file to the redirected standard input, and close the standard input
            // to signal that we're done
            ajaxMinProcess.StandardInput.Write(inputCode);
            ajaxMinProcess.StandardInput.Close();

            // and wait for the process to exit -- but no more than 10 seconds
            ajaxMinProcess.WaitForExit(10000);

            // if it hasn't exited normally, KILL IT with extreme predjudice.
            // (protect against an INFINITE LOOP or something)
            if (!ajaxMinProcess.HasExited)
            {
                ajaxMinProcess.Kill();
                Assert.Fail("process had to be killed - infinite loop?");
            }

            Trace.Write("EXIT CODE: ");
            Trace.WriteLine(ajaxMinProcess.ExitCode.ToString("X"));
            Trace.WriteLine(string.Empty);

            // no longer need the process
            ajaxMinProcess.Close();

            // read the expected code
            string expectedCode;
            using (var reader = new StreamReader(expectedPath))
            {
                expectedCode = reader.ReadToEnd();
            }

            // trace the expected path and code
            Trace.Write("EXPECTED CODE: ");
            Trace.WriteLine(expectedPath);
            Trace.WriteLine(expectedCode);
            Trace.WriteLine(string.Empty);

            // read the code actually output by the process
            string outputCode;
            using (var reader = new StreamReader(outputPath))
            {
                outputCode = reader.ReadToEnd();
            }

            // trace the output path and code
            Trace.Write("OUTPUT CODE: ");
            Trace.WriteLine(outputPath);
            Trace.WriteLine(outputCode);
            Trace.WriteLine(string.Empty);

            // compare them
            var exactMatch = string.CompareOrdinal(expectedCode, outputCode) == 0;
            Assert.IsTrue(exactMatch, "Expected and actual Output code does not match!");
        }
    }
}

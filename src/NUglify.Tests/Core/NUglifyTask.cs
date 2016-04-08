namespace DllUnitTest
{
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Ajax.Minifier.Tasks;
    using Microsoft.Build.Utilities;
    using NUnit.Framework;

    /// <summary>
    /// Summary description for LegacyTask
    /// </summary>
    [TestFixture]
    public class NUglifyTask
    {
        #region private fields

        private static string s_inputFolder;

        private static string s_outputFolder;

        private static string s_expectedFolder;

        #endregion

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

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var testClassName = testContext.FullyQualifiedTestClassName.Substring(
                testContext.FullyQualifiedTestClassName.LastIndexOf('.') + 1);
            s_inputFolder = Path.Combine(testContext.DeploymentDirectory, "Dll", "Input", testClassName);
            s_outputFolder = Path.Combine(testContext.DeploymentDirectory, "Dll", "Output", testClassName);
            s_expectedFolder = Path.Combine(testContext.DeploymentDirectory, "Dll", "Expected", testClassName);

            // make sure the output folder exists
            Directory.CreateDirectory(s_outputFolder);
        }

        #endregion

        [Test]
        public void TestNUglifyTask()
        {
            // create the task
            var task = CreateTaskWithInputs();

            // set up separate output JS files
            task.JsSourceExtensionPattern = @"\.js$";
            task.JsTargetExtension = ".min.js";

            // set up separate output CSS files
            task.CssSourceExtensionPattern = @"\.css$";
            task.CssTargetExtension = ".min.css";

            // make sure the files we expect to create are not there to begin with
            // unfortunately the task puts the minified files in the same folder as the source,
            // so there's no separate output folder
            Assert.IsTrue(!File.Exists(Path.Combine(s_inputFolder, "file1.min.js")), "file1.min.js already exists");
            Assert.IsTrue(!File.Exists(Path.Combine(s_inputFolder, "file2.min.js")), "file2.min.js already exists");
            Assert.IsTrue(!File.Exists(Path.Combine(s_inputFolder, "test1.min.css")), "test1.min.css already exists");
            Assert.IsTrue(!File.Exists(Path.Combine(s_inputFolder, "test2.min.css")), "test2.min.css already exists");

            var success = ExecuteAndReport(task);

            // check overall success
            Assert.IsTrue(success, "expected the task to succeed");

            // make sure all the files we expect were created
            Assert.IsTrue(File.Exists(Path.Combine(s_inputFolder, "file1.min.js")), "file1.min.js does not exist");
            Assert.IsTrue(File.Exists(Path.Combine(s_inputFolder, "file2.min.js")), "file2.min.js does not exist");
            Assert.IsTrue(File.Exists(Path.Combine(s_inputFolder, "test1.min.css")), "test1.min.css does not exist");
            Assert.IsTrue(File.Exists(Path.Combine(s_inputFolder, "test2.min.css")), "test2.min.css does not exist");

            // verify output file contents
            var test1JSVerify = VerifyFileContents("file1.min.js", s_inputFolder);
            var test2JSVerify = VerifyFileContents("file2.min.js", s_inputFolder);
            var test1CssVerify = VerifyFileContents("test1.min.css", s_inputFolder);
            var test2CssVerify = VerifyFileContents("test2.min.css", s_inputFolder);

            Assert.IsTrue(test1JSVerify, "file1.min.js output doesn't match");
            Assert.IsTrue(test2JSVerify, "file2.min.js output doesn't match");
            Assert.IsTrue(test1CssVerify, "test1.min.css output doesn't match");
            Assert.IsTrue(test2CssVerify, "test2.min.css output doesn't match");
        }

        [Test]
        public void TestNUglifyTaskFail()
        {
            // create the task
            var task = new NUglify();

            // common settings
            task.JsKnownGlobalNames = "jQuery,$";
            task.JsEvalTreatment = "MakeImmediateSafe";
            task.Switches = "-rename:none -reorder:N";

            // set up the JS files
            task.JsSourceFiles = new[] { 
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\fail.js" },
            };

            // set up the CSS files
            task.CssSourceFiles = new[] {
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\fail.css" },
            };

            // our mockup build engine
            task.BuildEngine = new TestBuildEngine()
            {
                MockProjectPath = Path.Combine(testContextInstance.DeploymentDirectory, "mock.csproj")
            };

            // set up separate output JS files
            task.JsSourceExtensionPattern = @"\.js$";
            task.JsTargetExtension = ".min.js";

            // set up separate output CSS files
            task.CssSourceExtensionPattern = @"\.css$";
            task.CssTargetExtension = ".min.css";


            // check overall success
            var success = ExecuteAndReport(task);
            Assert.IsFalse(success, "expected the task to fail");

            // make sure all the files we expect were created
            Assert.IsFalse(File.Exists(Path.Combine(s_inputFolder, "fail.min.js")), "fail.min.js should not exist");
            Assert.IsFalse(File.Exists(Path.Combine(s_inputFolder, "fail.min.css")), "fail.min.css should not exist");
        }

        [Test]
        public void TestNUglifyCombinedTask()
        {
            // create the task
            var task = CreateTaskWithInputs();

            // set up the combined file outputs
            task.JsCombinedFileName = @"Dll\Output\NUglifyTask\Combined.min.js";
            task.CssCombinedFileName = @"Dll\Output\NUglifyTask\Combined.min.css";

            var success = ExecuteAndReport(task);

            // check overall success
            Assert.IsTrue(success, "expected the task to succeed");

            // make sure all the files we expect were created
            Assert.IsTrue(File.Exists(Path.Combine(s_outputFolder, "Combined.min.js")), "Combined.min.js does not exist");
            Assert.IsTrue(File.Exists(Path.Combine(s_outputFolder, "Combined.min.css")), "Combined.min.css does not exist");

            // verify output file contents
            var testJSVerify = VerifyFileContents("Combined.min.js", s_outputFolder);
            var testCssVerify = VerifyFileContents("Combined.min.css", s_outputFolder);

            Assert.IsTrue(testJSVerify, "Combined.min.js output doesn't match");
            Assert.IsTrue(testCssVerify, "Combined.min.css output doesn't match");
        }

        [Test]
        public void TestNUglifyCombinedFail()
        {
            // create the task
            var task = new NUglify();

            // common settings
            task.JsKnownGlobalNames = "jQuery,$";
            task.JsEvalTreatment = "MakeImmediateSafe";
            task.Switches = "-rename:none -reorder:N";

            // set up the JS files
            task.JsSourceFiles = new[] { 
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\file1.js" },
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\fail.js" },
            };

            // set up the CSS files
            task.CssSourceFiles = new[] {
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\test1.css" },
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\fail.css" },
            };

            // our mockup build engine
            task.BuildEngine = new TestBuildEngine()
            {
                MockProjectPath = Path.Combine(testContextInstance.DeploymentDirectory, "mock.csproj")
            };

            // set up the combined file outputs
            task.JsCombinedFileName = @"Dll\Output\NUglifyTask\CombinedFail.min.js";
            task.CssCombinedFileName = @"Dll\Output\NUglifyTask\CombinedFail.min.css";

            var success = ExecuteAndReport(task);

            // check overall success
            Assert.IsFalse(success, "expected the task to fail");

            // make sure all the files we expect were created
            Assert.IsFalse(File.Exists(Path.Combine(s_outputFolder, "CombinedFail.min.js")), "CombinedFail.min.js should not exist");
            Assert.IsFalse(File.Exists(Path.Combine(s_outputFolder, "CombinedFail.min.css")), "CombinedFail.min.css should not exist");
        }

        [Test]
        public void TestNUglifyCombinedKillTask()
        {
            // create the task
            var task = CreateTaskWithInputs();

            // set up the combined file outputs
            task.JsCombinedFileName = @"Dll\Output\NUglifyTask\CombinedKill.min.js";
            task.CssCombinedFileName = @"Dll\Output\NUglifyTask\CombinedKill.min.css";
            task.Switches += " -kill:-1";

            var success = ExecuteAndReport(task);

            // check overall success
            Assert.IsTrue(success, "expected the task to succeed");

            // make sure all the files we expect were created
            Assert.IsTrue(File.Exists(Path.Combine(s_outputFolder, "CombinedKill.min.js")), "CombinedKill.min.js does not exist");
            Assert.IsTrue(File.Exists(Path.Combine(s_outputFolder, "CombinedKill.min.css")), "CombinedKill.min.css does not exist");

            // verify output file contents
            var testJSVerify = VerifyFileContents("CombinedKill.min.js", s_outputFolder);
            var testCssVerify = VerifyFileContents("CombinedKill.min.css", s_outputFolder);

            Assert.IsTrue(testJSVerify, "CombinedKill.min.js output doesn't match");
            Assert.IsTrue(testCssVerify, "CombinedKill.min.css output doesn't match");
        }

        private NUglify CreateTaskWithInputs()
        {
            // create the task, set it up, and execute it
            var task = new NUglify();

            // common settings
            task.JsKnownGlobalNames = "jQuery,$";
            task.JsEvalTreatment = "MakeImmediateSafe";
            task.Switches = "-rename:none -reorder:N";

            // set up the JS files
            task.JsSourceFiles = new[] { 
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\file1.js" },
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\file2.js" },
            };

            // set up the CSS files
            task.CssSourceFiles = new[] {
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\test1.css" },
                new TaskItem() { ItemSpec = @"Dll\Input\NUglifyTask\test2.css" },
            };

            // our mockup build engine
            task.BuildEngine = new TestBuildEngine()
            {
                MockProjectPath = Path.Combine(testContextInstance.DeploymentDirectory, "mock.csproj")
            };

            return task;
        }

        private bool ExecuteAndReport(NUglify task)
        {
            var success = task.Execute();
            Trace.Write("TASK RESULT: ");
            Trace.WriteLine(success);

            Trace.WriteLine(string.Empty);
            Trace.WriteLine("BUILD MESSAGES:");
            foreach (var logMessage in ((TestBuildEngine)task.BuildEngine).LogMessages)
            {
                Trace.WriteLine(logMessage);
            }

            return success;
        }

        private bool VerifyFileContents(string fileName, string outputFolder)
        {
            Trace.WriteLine("");
            Trace.Write("VERIFY OUTPUTFILE: ");
            Trace.WriteLine(fileName);

            var outputPath = Path.Combine(outputFolder, fileName);
            var expectedPath = Path.Combine(s_expectedFolder, fileName);

            Trace.WriteLine(string.Format("odd \"{1}\" \"{0}\"", outputPath, expectedPath));

            string expectedCode;
            using (var reader = new StreamReader(expectedPath))
            {
                expectedCode = reader.ReadToEnd();
            }

            Trace.WriteLine("EXPECTED:");
            Trace.WriteLine(expectedCode);

            string outputCode;
            using (var reader = new StreamReader(outputPath))
            {
                outputCode = reader.ReadToEnd();
            }

            Trace.WriteLine("ACTUAL:");
            Trace.WriteLine(outputCode);

            return string.CompareOrdinal(outputCode, expectedCode) == 0;
        }
    }
}

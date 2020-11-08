using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for Preprocessor
    /// </summary>
    [TestFixture]
    public class Preprocessor
    {
        public Preprocessor()
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

        [Test]
        public void Defines()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Defines_ackbar()
        {
            TestHelper.Instance.RunTest("-define:ackbar");
        }

        [Test]
        public void Defines_ackbarmeow()
        {
            TestHelper.Instance.RunTest("-define:ackbar,meow");
        }

        [Test]
        public void BadIfDef()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BadDefines()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Nested()
        {
            TestHelper.Instance.RunTest("-define:foo");
        }

        [Test]
        public void DebugSet()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DebugSet_debug()
        {
            TestHelper.Instance.RunTest("-debug");
        }

        [Test]
        public void DebugClear()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DefineIf()
        {
            TestHelper.Instance.RunTest("-reorder:n");
        }

        [Test]
        public void DefineIf_defines()
        {
            TestHelper.Instance.RunTest("-define:version=2.0,ackbar=ADMIRAL,MEOW=hiss -reorder:n");
        }

        [Test]
        public void SourceDirective()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

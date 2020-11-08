// BlockOpts.cs
//
// Copyright 2010 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for BlockOpts
    /// </summary>
    [TestFixture]
    public class BlockOpts
    {
        #region generated code 

        public BlockOpts()
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

        #endregion

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
        public void VarReturn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ReturnIfs()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DirectivePrologue()
        {
            TestHelper.Instance.RunTest();
        }

		[Test]
		public void AspNetBlock()
		{
			TestHelper.Instance.RunTest("-aspnet:true");
		}

        [Test]
        public void AfterReturn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void AfterReturn_noreloc()
        {
            // don't relocate function declarations
            TestHelper.Instance.RunTest("-kill:0x0000018000000000");
        }

        [Test]
        public void VarIntoFor()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void IfReturnReturn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ReturnVoid()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ReturnLiteral()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ExprReturn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void KillIfReturn()
        {
            TestHelper.Instance.RunTest("-kill:-1");
        }

        [Test]
        public void CombineAssign()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void VarAssign()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

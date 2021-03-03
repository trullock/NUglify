// AstMods.cs
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
    /// Summary description for AstMods
    /// </summary>
    [TestFixture]
    public class AstMods
    {
        #region auto code

        public AstMods()
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
        public void CallToMember()
        {
            TestHelper.Instance.RunTest("-enc:out ascii");
        }

        [Test]
        public void DateGetTimeToUnaryPlus()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DateGetTimeToUnaryPlus_H()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void LiteralExpressions()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LiteralsToLeft()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LiteralsFarToLeft()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LiteralsToRight()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LiteralsFarToRight()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void SimplifyStrToNum()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void SimplifyStrToNum_noeval()
        {
            TestHelper.Instance.RunTest("-literals:noeval");
        }

        [Test]
        public void LiteralOverflows()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void StrictToRegular()
        {
            // turn off the "EvaluateNumericExpressions" feature
            TestHelper.Instance.RunTest("-kill:0x0000000020000000 -unused:keep -ignore:JS1287");
        }

        [Test]
        public void MoveFunctions()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void VerticalTab()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void AspNet()
        {
            TestHelper.Instance.RunTest("-term");
        }

        [Test]
        public void IfReturn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NestedIf()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ExpressionIf()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void IfContinue()
        {
            TestHelper.Instance.RunTest("-unused:keep -ignore:JS1026");
        }

        [Test]
        public void NegShortcutIf()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ConstantIf()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ReturnAssignOp()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void EvalLength()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Amd()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Amd_amd()
        {
            TestHelper.Instance.RunTest("-amd");
        }

        [Test]
        public void OpAssignCombine()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

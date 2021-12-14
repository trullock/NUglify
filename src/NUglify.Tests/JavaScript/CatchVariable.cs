// CatchVariable.cs
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

using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for CatchVariable
    /// </summary>
    [TestFixture]
    public class CatchVariable
    {
        #region auto-gen

        public CatchVariable()
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
        public void Ambiguous()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Ambiguous_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void OuterNotRef()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void OuterNotRef_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void NoOuter()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NoOuter_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void ForInLet()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForInLet_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void OuterIsGlobal()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test, Ignore("This no longer applies to modern browsers")]
        public void Collision()
        {
            TestHelper.Instance.RunErrorTest("-rename:all", JSError.AmbiguousCatchVar, JSError.SemicolonInsertion, JSError.MisplacedFunctionDeclaration);
        }
    }
}

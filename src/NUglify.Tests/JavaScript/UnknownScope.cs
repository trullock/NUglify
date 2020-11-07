// UnknownScope.cs
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
    /// Summary description for UnknownScope
    /// </summary>
    [TestFixture]
    public class UnknownScope
    {
        public UnknownScope()
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
        public void Eval()
        {
            TestHelper.Instance.RunTest("-evals:safeall -rename:all");
        }

        [Test]
        public void WindowEval()
        {
            TestHelper.Instance.RunTest("-evals:safeall -rename:all");
        }

        [Test]
        public void Eval_J()
        {
            TestHelper.Instance.RunTest("-rename:all -evals:ignore");
        }

        [Test]
        public void Eval_Immediate()
        {
            TestHelper.Instance.RunTest("-rename:all -evals:immediate");
        }

        [Test]
        public void With()
        {
            TestHelper.Instance.RunTest("-rename:all -debug:N,$Debug");
        }
    }
}

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
    /// Summary description for DebugNamespaces
    /// </summary>
    [TestFixture]
    public class DebugNamespaces
    {
        public DebugNamespaces()
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
        public void DebugAsKnownGlobal()
        {
            // default parameters -- we SHOULD get an error for the unknown global Foo
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.UndeclaredVariable);
        }

        [Test]
        public void DebugAsKnownGlobal_debug()
        {
            // we should NOT get an unknown global error because we specified it as a debug namespace.
            // it should still be in the output, though, because we have debug mode turned on.
            TestHelper.Instance.RunErrorTest("-rename:none -debug:Y,Foo.Bar,Bat,Foo.Cakes");
        }

        [Test]
        public void DebugAsKnownGlobal_release()
        {
            // we should NOT get an unknown global error because we specified it as a debug namespace.
            // it should NOT be in the output, though, because we have debug mode turned off.
            TestHelper.Instance.RunErrorTest("-rename:none -debug:N,Foo.Bar,Bat,Foo.Cakes");
        }

        [Test]
        public void DebugNamespaceDirective()
        {
            // we should NOT get an unknown global error because we specified it as a debug namespace.
            // it should NOT be in the output, though, because we have debug mode turned off.
            TestHelper.Instance.RunErrorTest("-rename:none -debug:N");
        }

        [Test]
        public void EmptyIf()
        {
            // we should NOT get an unknown global error because we specified it as a debug namespace.
            // it should NOT be in the output, though, because we have debug mode turned off.
            TestHelper.Instance.RunTest("-debug:N,console,window.console,Debug");
        }

        [Test]
        public void ConsoleDebug()
        {
            TestHelper.Instance.RunErrorTest("-debug:N,console");
        }
    }
}

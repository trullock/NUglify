// Syntax.cs
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

using Microsoft.Ajax.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSUnitTest
{
    /// <summary>
    /// Summary description for Syntax
    /// </summary>
    [TestClass]
    public class Syntax
    {
        [TestMethod]
        public void NestedBlocks()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void BOM()
        {
            TestHelper.Instance.RunTest("-term");
        }

        [TestMethod]
        public void SlashSpacing()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void EmptyStatement()
        {
            // don't change if-statements to expressions
            TestHelper.Instance.RunTest("-kill:0x800001000");
        }

        [TestMethod]
        public void EmptyStatement_P()
        {
            TestHelper.Instance.RunTest("-P");
        }

        [TestMethod]
        public void ES6()
        {
            // no errors
            TestHelper.Instance.RunErrorTest("-rename:none");
        }

        [TestMethod]
        public void ES6_h()
        {
            // no errors
            TestHelper.Instance.RunErrorTest();
        }

        [TestMethod]
        public void NonES6Yield()
        {
            // no errors
            TestHelper.Instance.RunErrorTest();
        }

        [TestMethod]
        public void NonES6Yield_norename()
        {
            // no errors
            TestHelper.Instance.RunErrorTest("-rename:none");
        }

        [TestMethod]
        public void BindingPatterns()
        {
            // no errors other than unreferenced arguments
            TestHelper.Instance.RunErrorTest("-ignore:JS1270,JS1268", JSError.BindingPatternRequiresInitializer);
        }

        [TestMethod]
        public void MissingMemberRoot()
        {
            // ignore the undefined variable, but make sure there's an expected-expression error
            TestHelper.Instance.RunErrorTest("-ignore:js1135", JSError.ExpressionExpected);
        }
    }
}

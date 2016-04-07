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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssUnitTest
{
    /// <summary>
    /// Summary description for Syntax
    /// </summary>
    [TestClass]
    public class Syntax
    {
        [TestMethod]
        public void Strings()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Function()
        {
            TestHelper.Instance.RunTest("-colors:strict");
        }

        [TestMethod]
        public void URI()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Important()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Term()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Expression()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void CDO()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Expr()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void UnicodeRange()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Escapes()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void AlphaHash()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void FontNames()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void PointZeroEms()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

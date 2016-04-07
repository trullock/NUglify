// Switch.cs
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
    /// Summary description for Switch
    /// </summary>
    [TestClass]
    public class Switch
    {
        [TestMethod]
        public void EmptyDefault()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [TestMethod]
        public void EmptyDefault_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void NoDefaultEmptyCases()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [TestMethod]
        public void NoDefaultEmptyCases_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void PromoteBreak()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [TestMethod]
        public void PromoteBreak_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void SwitchLabels()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [TestMethod]
        public void SwitchLabels_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void PrettyPrint()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void PrettyPrint_P()
        {
            TestHelper.Instance.RunTest("-pretty -clobber");
        }

        [TestMethod]
        public void PrettyPrint_same()
        {
            TestHelper.Instance.RunTest("-pretty -braces:same");
        }

        [TestMethod]
        public void MacQuirks()
        {
            TestHelper.Instance.RunTest("-mac:No");
        }

        [TestMethod]
        public void MacQuirks_M()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Unreachable()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void LineBreak()
        {
            TestHelper.Instance.RunTest("-line:10");
        }

        [TestMethod]
        public void LineBreak_Multi()
        {
            TestHelper.Instance.RunTest("-line:,multi");
        }

        [TestMethod]
        public void LineBreak_MultiIndent()
        {
            TestHelper.Instance.RunTest("-line:,multiple,8");
        }

        [TestMethod]
        public void LineBreak_BreakSingle()
        {
            TestHelper.Instance.RunTest("-line:10,single");
        }

        [TestMethod]
        public void LineBreak_BreakMultiIndent()
        {
            TestHelper.Instance.RunTest("-line:10,m,2");
        }

        [TestMethod]
        public void Braces()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Braces_new()
        {
            TestHelper.Instance.RunTest("-line:m -braces:new");
        }

        [TestMethod]
        public void Braces_same()
        {
            TestHelper.Instance.RunTest("-line:m -braces:same");
        }

        [TestMethod]
        public void Braces_source()
        {
            TestHelper.Instance.RunTest("-line:m -braces:source");
        }

        [TestMethod]
        public void IgnoreErrors()
        {
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.ErrorEndOfFile, JSError.UnclosedFunction, JSError.UndeclaredFunction, JSError.UndeclaredVariable, JSError.NoRightParenthesis, JSError.VariableDefinedNotReferenced);
        }

        [TestMethod]
        public void IgnoreErrors_some()
        {
            TestHelper.Instance.RunErrorTest("-rename:none -ignore:js1138,JS1135,Js1268,jS1310", JSError.ErrorEndOfFile, JSError.UnclosedFunction, JSError.NoRightParenthesis);
        }

        [TestMethod]
        public void IgnoreErrors_all()
        {
            TestHelper.Instance.RunErrorTest("-rename:none -ignore:All");
        }

        [TestMethod]
        public void NoBreakThrow()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [TestMethod]
        public void NoBreakBreak()
        {
            TestHelper.Instance.RunTest("-line:4 -unused:keep");
        }

        [TestMethod]
        public void NoBreakContinue()
        {
            TestHelper.Instance.RunTest("-line:4 -unused:keep");
        }

        [TestMethod]
        public void NoBreakReturn()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [TestMethod]
        public void NoBreakPostInc()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [TestMethod]
        public void NoBreakPostDec()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [TestMethod]
        public void Culture_fr()
        {
            TestHelper.Instance.RunTest("-culture:fr-fr");
        }

        [TestMethod]
        public void Culture_esZW()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:es-zw");
        }

        [TestMethod]
        public void Culture_hawUS()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:haw-US");
        }

        [TestMethod]
        public void Culture_zhCN()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:zh-cn");
        }
    }
}

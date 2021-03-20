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

using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for Switch
    /// </summary>
    [TestFixture]
    public class Switch
    {
        [Test]
        public void EmptyDefault()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void EmptyDefault_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void NoDefaultEmptyCases()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void NoDefaultEmptyCases_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void PromoteBreak()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void PromoteBreak_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void SwitchLabels()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void SwitchLabels_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void PrettyPrint()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void PrettyPrint_P()
        {
            TestHelper.Instance.RunTest("-pretty -clobber");
        }

        [Test]
        public void PrettyPrint_same()
        {
            TestHelper.Instance.RunTest("-pretty -braces:same");
        }

        [Test]
        public void MacQuirks()
        {
            TestHelper.Instance.RunTest("-mac:No");
        }

        [Test]
        public void MacQuirks_M()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Unreachable()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LineBreak()
        {
            TestHelper.Instance.RunTest("-line:10");
        }

        [Test]
        public void LineBreak_Multi()
        {
            TestHelper.Instance.RunTest("-line:,multi");
        }

        [Test]
        public void LineBreak_MultiIndent()
        {
            TestHelper.Instance.RunTest("-line:,multiple,8");
        }

        [Test]
        public void LineBreak_BreakSingle()
        {
            TestHelper.Instance.RunTest("-line:10,single");
        }

        [Test]
        public void LineBreak_BreakMultiIndent()
        {
            TestHelper.Instance.RunTest("-line:10,m,2");
        }

        [Test]
        public void Braces()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Braces_new()
        {
            TestHelper.Instance.RunTest("-line:m -braces:new");
        }

        [Test]
        public void Braces_same()
        {
            TestHelper.Instance.RunTest("-line:m -braces:same");
        }

        [Test]
        public void Braces_source()
        {
            TestHelper.Instance.RunTest("-line:m -braces:source");
        }

        [Test]
        public void IgnoreErrors()
        {
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.ErrorEndOfFile, JSError.UnclosedFunction, JSError.UndeclaredFunction, JSError.UndeclaredVariable, JSError.NoRightParenthesis, JSError.VariableDefinedNotReferenced);
        }

        [Test]
        public void IgnoreErrors_some()
        {
            TestHelper.Instance.RunErrorTest("-rename:none -ignore:js1138,JS1135,Js1268,jS1310", JSError.ErrorEndOfFile, JSError.UnclosedFunction, JSError.NoRightParenthesis);
        }

        [Test]
        public void IgnoreErrors_all()
        {
            TestHelper.Instance.RunErrorTest("-rename:none -ignore:All");
        }

        [Test]
        public void NoBreakThrow()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [Test]
        public void NoBreakBreak()
        {
            TestHelper.Instance.RunTest("-line:4 -unused:keep -ignore:JS1026,JS1019");
        }

        [Test]
        public void NoBreakContinue()
        {
            TestHelper.Instance.RunTest("-line:4 -unused:keep -ignore:JS1026,JS1020");
        }

        [Test]
        public void NoBreakReturn()
        {
            TestHelper.Instance.RunTest("-line:4 -ignore:JS1018");
        }

        [Test]
        public void NoBreakPostInc()
        {
            TestHelper.Instance.RunTest("-line:4");
        }

        [Test]
        public void NoBreakPostDec()
        {
            TestHelper.Instance.RunTest("-line:4");
        }
        /*
        [Test]
        public void Culture_fr()
        {
            TestHelper.Instance.RunTest("-culture:fr-fr");
        }

        [Test]
        public void Culture_esZW()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:es-zw");
        }

        [Test]
        public void Culture_hawUS()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:haw-US");
        }

        [Test]
        public void Culture_zhCN()
        {
            // doesn't exist, but should use es instead
            TestHelper.Instance.RunTest("-culture:zh-cn");
        }
        */
    }
}

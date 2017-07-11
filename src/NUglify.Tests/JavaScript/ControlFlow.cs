// ControlFlow.cs
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
    ///This is a test class for Microsoft.Ajax.Utilities.MainClass and is intended
    ///to contain all Microsoft.Ajax.Utilities.MainClass Unit Tests
    ///</summary>
    [TestFixture]
    public class ControlFlow
    {
        [Test]
        public void Break()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BreakLabel()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BreakNoLabel()
        {
            TestHelper.Instance.RunErrorTest("-ignore:JS1021", JSError.NoLabel, JSError.BadBreak);
        }

        [Test]
        public void Continue()
        {
            // don't optimize if(cond)continue;
            TestHelper.Instance.RunTest("-kill:0x800000000000");
        }

        [Test]
        public void Continue_inv()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Debugger()
        {
            TestHelper.Instance.RunTest("-debug:Y -kill:0x400000000000");
        }

        [Test]
        public void Debugger_D()
        {
            TestHelper.Instance.RunTest("-debug:N,Debug,$Debug,WAssert,Msn.Debug,Web.Debug -kill:0x400000000000");
        }

        [Test]
        public void Debugger_OnCustom()
        {
            // no flag is the same as Y -- and since turned on the debug
            // means no replacement of debug lookups, it doesn't really matter what
            // comes after the comma. We'll process them, but we won't be replacing anything
            // anyway!
            TestHelper.Instance.RunTest("-debug:,AckBar,FooBar -kill:0x400000000000");
        }

        [Test]
        public void Debugger_OffCustom()
        {
            TestHelper.Instance.RunTest("-debug:N,AckBar,FooBar,Debug,$Debug,Web.Debug -kill:0x400000000000");
        }

        [Test]
        public void Debugger_OffNone()
        {
            // adding the comma after means we want to specify the debug lookups.
            // but since we have nothing after the comma, we replace the defaults
            // ($Debug, Debug, WAssert) with nothing.
            TestHelper.Instance.RunTest("-debug:N, -kill:0x400000000000");
        }

        [Test]
        public void DoWhile()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForWhile()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForIn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForVar_reorder()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForVar()
        {
            TestHelper.Instance.RunTest("-reorder:N");
        }

        [Test]
        public void If()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Labels()
        {
            TestHelper.Instance.RunErrorTest("-ignore:JS1267 -rename:none -unused:keep", JSError.NoLabel, JSError.NoLabel, JSError.UnusedLabel, JSError.UnusedLabel);
        }

        [Test]
        public void Labels_H()
        {
            TestHelper.Instance.RunErrorTest("-ignore:JS1267", JSError.NoLabel, JSError.NoLabel, JSError.UnusedLabel, JSError.UnusedLabel);
        }

        [Test]
        public void Labels_keep()
        {
            TestHelper.Instance.RunErrorTest("-ignore:JS1267 -unused:keep", JSError.NoLabel, JSError.NoLabel, JSError.UnusedLabel, JSError.UnusedLabel);
        }

        [Test]
        public void Return()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Switch()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void Switch_h()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void TryCatch()
        {
            TestHelper.Instance.RunTest("-mac:FALSE");
        }

        [Test]
        public void TryCatch_m()
        {
            TestHelper.Instance.RunTest("-mac:Y");
        }

        [Test]
        public void CatchScope()
        {
            TestHelper.Instance.RunTest("-rename:none"); // we see the difference when hypercrunch is on
        }

        [Test]
        public void CatchScope_Local()
        {
            TestHelper.Instance.RunTest("-rename:all"); // catch-local switch and hypercrunch
        }

        [Test]
        public void EncloseBlock()
        {
            // kill switch: IfExpressionsToExpression, CombineAdjacentExpressionStatements, IfNotTrueFalseToIfFalseTrue,
            // IfConditionCallToConditionAndCall
            TestHelper.Instance.RunTest("-reorder:no -kill:0x21804002000");
        }

        [Test]
        public void EncloseBlock_nominify()
        {
            TestHelper.Instance.RunTest("-minify:no");
        }

        [Test]
        public void Throw()
        {
            TestHelper.Instance.RunTest("-mac:N");
        }

        [Test]
        public void Throw_M()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void While()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void While_NoFor()
        {
            TestHelper.Instance.RunTest("-kill:0x400000000000");
        }

        [Test]
        public void While_ForNoVar()
        {
            TestHelper.Instance.RunTest("-kill:0x400");
        }

        [Test]
        public void ForNoIn()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ForNoIn_kill()
        {
            TestHelper.Instance.RunTest("-kill:0x40000000000");
        }
    }
}

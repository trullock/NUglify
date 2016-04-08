// Declarations.cs
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
    ///This is a test class for Microsoft.Ajax.Utilities.MainClass and is intended
    ///to contain all Microsoft.Ajax.Utilities.MainClass Unit Tests
    ///</summary>
    [TestFixture]
    public class Declarations
    {
        [Test]
        public void New()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void This()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void Var()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Var_keep()
        {
            TestHelper.Instance.RunTest("-enc:out ascii -unused:keep");
        }

        [Test]
        public void With()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void With_H()
        {
            TestHelper.Instance.RunTest("-rename:all -unused:keep");
        }

        [Test]
        public void Function()
        {
            TestHelper.Instance.RunTest("-unused:keep");
        }

        [Test]
        public void Identifiers()
        {
            // just the one flag for "module"
            TestHelper.Instance.RunErrorTest("-rename:none");
        }

        [Test]
        public void Identifiers_h()
        {
            // just the one flag for "module"
            TestHelper.Instance.RunErrorTest();
        }

        [Test]
        public void Identifiers_ascii()
        {
            // just the one flag for "module"
            TestHelper.Instance.RunErrorTest("-rename:none -enc:out ascii");
        }

        [Test]
        public void LocalizationVars()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void LocalizationVars_H()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void LocalizationVars_HL()
        {
            TestHelper.Instance.RunTest("-rename:localization");
        }

        [Test]
        public void Const()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Const_moz()
        {
            TestHelper.Instance.RunTest("-const:moz");
        }
    }
}

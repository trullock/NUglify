// Strict.cs
//
// Copyright 2011 Microsoft Corporation
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
    /// Summary description for Strict
    /// </summary>
    [TestFixture]
    public class Strict
    {
        [Test]
        public void EvalArgsAssign()
        {
            TestHelper.Instance.RunErrorTest("-rename:none", 
                JSError.StrictModeInvalidAssign, 
                JSError.StrictModeInvalidAssign,
                JSError.StrictModeInvalidPreOrPost, 
                JSError.StrictModeInvalidPreOrPost, 
                JSError.StrictModeInvalidPreOrPost, 
                JSError.StrictModeInvalidPreOrPost);
        }

        [Test]
        public void InvalidVarName()
        {
            TestHelper.Instance.RunErrorTest("-rename:none",
                JSError.StrictModeVariableName,
                JSError.StrictModeVariableName,
                JSError.StrictModeArgumentName,
                JSError.StrictModeArgumentName,
                JSError.StrictModeCatchName,
                JSError.StrictModeCatchName,
                JSError.StrictModeFunctionName,
                JSError.StrictModeFunctionName,
                JSError.UndeclaredFunction,
                JSError.SemicolonInsertion);
        }

        [Test]
        public void DupArg()
        {
            TestHelper.Instance.RunErrorTest("-rename:none",
                JSError.StrictModeDuplicateArgument,
                JSError.DuplicateName,
                JSError.SemicolonInsertion);
        }

        [Test]
        public void With()
        {
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.StrictModeNoWith, JSError.SemicolonInsertion);
        }

        [Test]
        public void DupProperty()
        {
            TestHelper.Instance.RunErrorTest("-rename:none",
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.StrictModeDuplicateProperty,
                JSError.SemicolonInsertion,
                JSError.SemicolonInsertion);
        }

        [Test]
        public void InvalidDelete()
        {
            TestHelper.Instance.RunErrorTest("-rename:none",
                JSError.StrictModeInvalidDelete,
                JSError.StrictModeInvalidDelete,
                JSError.StrictModeInvalidDelete);
        }

        [Test]
        public void FuncDeclLoc()
        {
            // throw a warning, even when not in strict mode. 
            // this is because ES5 is a cross-browser issue.
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.MisplacedFunctionDeclaration);
        }

        [Test]
        public void FuncDeclStrict()
        {
            // error because it's in strict mode
            TestHelper.Instance.RunErrorTest("-rename:none", JSError.MisplacedFunctionDeclaration);
        }

        [Test]
        public void ExtraUseStrict()
        {
            // error because it's in strict mode
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void StrictSwitchOn()
        {
            // error because it's in strict mode
            TestHelper.Instance.RunTest("-strict -xml");
        }

        [Test]
        public void StrictPartial()
        {
            // error because it's in strict mode
            TestHelper.Instance.RunTest("-rename:all -xml");
        }

        [Test]
        public void UnknownGlobal()
        {
            TestHelper.Instance.RunErrorTest("-rename:all", JSError.UndeclaredVariable, JSError.StrictModeUndefinedVariable, JSError.UndeclaredFunction, JSError.UndeclaredVariable, JSError.StrictModeInvalidPreOrPost);
        }
    }
}

// Assignments.cs
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
    public class Assignments
    {
        [Test]
        public void Assign()
        {
            TestHelper.Instance.RunTest("-enc:out ascii");
        }

        [Test]
        public void Assign_utf8()
        {
            TestHelper.Instance.RunTest("-enc:out utf-8");
        }

        [Test]
        public void CompoundAssign()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void MultiVars()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void AssignAspNetBlock()
        {
            TestHelper.Instance.RunTest("-aspnet:true");
        }

        [Test]
        public void PlusAssignReturn()
        {
            TestHelper.Instance.RunTest();
        }
    }
}
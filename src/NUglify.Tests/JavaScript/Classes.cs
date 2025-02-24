// Classes.cs
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
    /// Summary description for Classes
    /// </summary>
    [TestFixture]
    public class Classes
    {
        [Test]
        public void ClassDecl()
        {
            TestHelper.Instance.RunErrorTest(JSError.ArgumentNotReferenced);
        }

        [Test]
        public void ClassExpr()
        {
            TestHelper.Instance.RunErrorTest();
        }

        [Test]
        public void ClassErrors()
        {
            TestHelper.Instance.RunErrorTest(JSError.NoIdentifier, JSError.NoIdentifier, JSError.NoLeftCurly, JSError.NoRightCurly);
        }

        [Test]
        public void ClassNormalAndStaticProperty()
        {
            TestHelper.Instance.RunErrorTest();
        }
    }
}

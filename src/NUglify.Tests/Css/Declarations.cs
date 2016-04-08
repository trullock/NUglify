// AtRules.cs
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

using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
{
    /// <summary>
    /// Summary description for Declarations
    /// </summary>
    [TestFixture]
    public class Declarations
    {
        [Test]
        public void TrailingSemicolon()
        {
            TestHelper.Instance.RunTest("-css:decls");
        }

        [Test]
        public void TrailingSemicolon_Term()
        {
            TestHelper.Instance.RunTest("-css:decls -term -colors:strict");
        }

        [Test]
        public void NoTrailingSemicolon()
        {
            TestHelper.Instance.RunTest("-css:decls");
        }

        [Test]
        public void NoTrailingSemicolon_Term()
        {
            TestHelper.Instance.RunTest("-css:decls -term:1 -colors:strict");
        }
    }
}

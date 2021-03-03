// ConditionalCompilation.cs
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
  /// Summary description for ConditionalCompilation
  /// </summary>
  [TestFixture]
  public class ConditionalCompilation
  {
    [Test]
    public void IfElse()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Set()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void PPConstant()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void PPOps()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void StartWithIf()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void StartWithSet()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void NoEnd()
    {
        TestHelper.Instance.RunErrorTest(JSError.OctalLiteralsDeprecated, JSError.NoCCEnd);
    }

    [Test]
    public void SpecialCase()
    {
        TestHelper.Instance.RunTest("-ignore:JS1282");
    }

    [Test]
    public void SpecialCase2()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void DoubleStart()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void MultipleOn()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void MultipleOn_Kill()
    {
        TestHelper.Instance.RunTest("-kill:134217728");
    }

    [Test]
    public void IETest()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [Test]
    public void TypeComments()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void EndEOF()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void FuncDecl()
    {
        TestHelper.Instance.RunTest();
    }
  }
}

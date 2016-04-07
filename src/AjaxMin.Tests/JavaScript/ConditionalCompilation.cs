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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSUnitTest
{
  /// <summary>
  /// Summary description for ConditionalCompilation
  /// </summary>
  [TestClass]
  public class ConditionalCompilation
  {
    [TestMethod()]
    public void IfElse()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void Set()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void PPConstant()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void PPOps()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void StartWithIf()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void StartWithSet()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void NoEnd()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void SpecialCase()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void SpecialCase2()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void DoubleStart()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void MultipleOn()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void MultipleOn_Kill()
    {
        TestHelper.Instance.RunTest("-kill:134217728");
    }

    [TestMethod]
    public void IETest()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [TestMethod]
    public void TypeComments()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void EndEOF()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void FuncDecl()
    {
        TestHelper.Instance.RunTest();
    }
  }
}

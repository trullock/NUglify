// Selectors.cs
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

namespace CssUnitTest
{
  /// <summary>
  /// Summary description for Selectors
  /// </summary>
  [TestClass]
  public class Selectors
  {
    [TestMethod]
    public void Simple()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Combinator()
    {
      TestHelper.Instance.RunTest("-colors:hex");
    }

    [TestMethod]
    public void PseudoClass_utf8()
    {
        TestHelper.Instance.RunTest("-enc:out utf-8 -colors:strict");
    }

    [TestMethod]
    public void PseudoClass()
    {
        TestHelper.Instance.RunTest("-enc:out ascii");
    }

    [TestMethod]
    public void PseudoElement()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Attribute()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Universal()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Grouping()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Not()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void CSS3()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void CSS3_all()
    {
        TestHelper.Instance.RunTest("-unused:keep");
    }

    [TestMethod]
    public void CSS3_pretty()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [TestMethod]
    public void Namespace()
    {
        TestHelper.Instance.RunTest("-css:full -colors:strict");
    }

    [TestMethod]
    public void NoSpaceUniversal()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void MatchCasing()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Escapes()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Any()
    {
        TestHelper.Instance.RunTest();
    }
  }
}

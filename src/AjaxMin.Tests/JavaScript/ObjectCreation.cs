// ObjectCreation.cs
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
  ///This is a test class for Microsoft.Ajax.Utilities.MainClass and is intended
  ///to contain all Microsoft.Ajax.Utilities.MainClass Unit Tests
  ///</summary>
  [TestClass()]
  public class ObjectCreation
  {
    [TestMethod()]
    public void Object()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void Object_H()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [TestMethod()]
    public void Object_hc()
    {
        TestHelper.Instance.RunTest("-rename:all -literals:combine");
    }

    [TestMethod()]
    public void Object_L()
    {
      TestHelper.Instance.RunTest("-new:keep");
    }

    [TestMethod()]
    public void CompareObjects()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void Constructor()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void Constructor_H()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [TestMethod()]
    public void Constructor_hc()
    {
        TestHelper.Instance.RunTest("-rename:all -literals:combine");
    }

    [TestMethod()]
    public void Prototype()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void InstanceOf()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void ToStr()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void TypeOf()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void RegularExpressions()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod()]
    public void Strings()
    {
      // outputs ascii-escaped characters
      TestHelper.Instance.RunTest("-enc:out ascii");
    }

    [TestMethod()]
    public void Strings_Utf8()
    {
      // outputs unicode characters
      TestHelper.Instance.RunTest("-enc:out utf-8");
    }
  }
}

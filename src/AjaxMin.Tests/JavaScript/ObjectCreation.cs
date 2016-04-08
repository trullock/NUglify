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

using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
  /// <summary>
  ///This is a test class for Microsoft.Ajax.Utilities.MainClass and is intended
  ///to contain all Microsoft.Ajax.Utilities.MainClass Unit Tests
  ///</summary>
  [TestFixture]
  public class ObjectCreation
  {
    [Test]
    public void Object()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Object_H()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [Test]
    public void Object_hc()
    {
        TestHelper.Instance.RunTest("-rename:all -literals:combine");
    }

    [Test]
    public void Object_L()
    {
      TestHelper.Instance.RunTest("-new:keep");
    }

    [Test]
    public void CompareObjects()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Constructor()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Constructor_H()
    {
        TestHelper.Instance.RunTest("-rename:all");
    }

    [Test]
    public void Constructor_hc()
    {
        TestHelper.Instance.RunTest("-rename:all -literals:combine");
    }

    [Test]
    public void Prototype()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void InstanceOf()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void ToStr()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void TypeOf()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void RegularExpressions()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Strings()
    {
      // outputs ascii-escaped characters
      TestHelper.Instance.RunTest("-enc:out ascii");
    }

    [Test]
    public void Strings_Utf8()
    {
      // outputs unicode characters
      TestHelper.Instance.RunTest("-enc:out utf-8");
    }
  }
}

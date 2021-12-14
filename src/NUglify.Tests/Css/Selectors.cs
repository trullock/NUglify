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

using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
{
  /// <summary>
  /// Summary description for Selectors
  /// </summary>
  [TestFixture]
  public class Selectors
  {
    [Test]
    public void Simple()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Combinator()
    {
      TestHelper.Instance.RunTest("-colors:hex");
    }

    [Test]
    public void PseudoClass_utf8()
    {
        TestHelper.Instance.RunTest("-enc:out utf-8 -colors:strict");
    }

    [Test]
    public void PseudoClass()
    {
        TestHelper.Instance.RunTest("-enc:out ascii");
    }

    [Test]
    public void PseudoElement()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Attribute()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Universal()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Grouping()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Not()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void CSS3()
    {
	    TestHelper.Instance.RunTest();
    }
    
    [Test]
    public void PseudoFunctions()
    {
	    TestHelper.Instance.RunTest();
    }
	
    [Test]
    public void CSS3_all()
    {
        TestHelper.Instance.RunTest("-unused:keep");
    }

    [Test]
    public void CSS3_pretty()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [Test]
    public void Namespace()
    {
        TestHelper.Instance.RunTest("-css:full -colors:strict");
    }

    [Test]
    public void NoSpaceUniversal()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void MatchCasing()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void Escapes()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void Any()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void Bootstrap4CssVariables()
    {
        TestHelper.Instance.RunTest();
    }
  }
}

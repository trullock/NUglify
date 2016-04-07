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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssUnitTest
{
  /// <summary>
  /// Summary description for AtRules
  /// </summary>
  [TestClass]
  public class AtRules
  {
    [TestMethod]
    public void Media()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Import()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Page()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void FontFace()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Charset()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Namespace()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void Other()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void ImportComment()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
      public void ImportComment_c()
    {
        TestHelper.Instance.RunTest("-comments:all");
    }

    [TestMethod]
    public void ImportComment_x()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [TestMethod]
    public void KeyFrames()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void KeyFrames_p()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [TestMethod]
    public void KeyFrames_same()
    {
        TestHelper.Instance.RunTest("-pretty -braces:same");
    }

    [TestMethod]
    public void KeyFrames_source()
    {
        TestHelper.Instance.RunTest("-pretty -braces:source");
    }
  }
}

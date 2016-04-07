// CommentHacks.cs
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
  /// Summary description for CommentHacks
  /// </summary>
  [TestClass]
  public class CommentHacks
  {
    [TestMethod]
    public void HideFromMacIE()
    {
      TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void HideFromMacIE_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void HideFromNS4()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void HideFromNS4_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void OnlyNS4()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void OnlyNS4_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void HideFromIE5()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void HideFromIE5_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void HideFromIE6()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void HideFromIE6_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void EmptyComments()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [TestMethod]
    public void EmptyComments_nc()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void ImportantComment()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void TwoImportantComments()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void TwoImportantComments_pretty()
    {
        TestHelper.Instance.RunTest("-p");
    }

    [TestMethod]
    public void EmbeddedImportantComment()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void EmbeddedImportantComment_pretty()
    {
        TestHelper.Instance.RunTest("-p");
    }

    [TestMethod]
    public void ImportantCommentHacks()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void ImportantComment_Kill()
    {
        TestHelper.Instance.RunTest("-kill:1");
    }

    [TestMethod]
    public void ImportantComment_None()
    {
        TestHelper.Instance.RunTest("-comments:none");
    }

    [TestMethod]
    public void ImportantComment_All()
    {
        TestHelper.Instance.RunTest("-comments:all");
    }

    [TestMethod]
    public void SingleLine()
    {
        // even though we say all comments, we won't persist single-line comments since
        // they aren't valid CSS comments
        TestHelper.Instance.RunTest("-comments:all");
    }

    [TestMethod]
    public void SharepointThemes()
    {
        TestHelper.Instance.RunTest();
    }

    [TestMethod]
    public void SharepointThemes_None()
    {
        TestHelper.Instance.RunTest("-comments:none");
    }
  }
}

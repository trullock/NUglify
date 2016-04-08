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

using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
{
  /// <summary>
  /// Summary description for CommentHacks
  /// </summary>
  [TestFixture]
  public class CommentHacks
  {
    [Test]
    public void HideFromMacIE()
    {
      TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void HideFromMacIE_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void HideFromNS4()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void HideFromNS4_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void OnlyNS4()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void OnlyNS4_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void HideFromIE5()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void HideFromIE5_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void HideFromIE6()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void HideFromIE6_nc()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void EmptyComments()
    {
        TestHelper.Instance.RunTest("-comments:hacks");
    }

    [Test]
    public void EmptyComments_nc()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void ImportantComment()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void TwoImportantComments()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void TwoImportantComments_pretty()
    {
        TestHelper.Instance.RunTest("-p");
    }

    [Test]
    public void EmbeddedImportantComment()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void EmbeddedImportantComment_pretty()
    {
        TestHelper.Instance.RunTest("-p");
    }

    [Test]
    public void ImportantCommentHacks()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void ImportantComment_Kill()
    {
        TestHelper.Instance.RunTest("-kill:1");
    }

    [Test]
    public void ImportantComment_None()
    {
        TestHelper.Instance.RunTest("-comments:none");
    }

    [Test]
    public void ImportantComment_All()
    {
        TestHelper.Instance.RunTest("-comments:all");
    }

    [Test]
    public void SingleLine()
    {
        // even though we say all comments, we won't persist single-line comments since
        // they aren't valid CSS comments
        TestHelper.Instance.RunTest("-comments:all");
    }

    [Test]
    public void SharepointThemes()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void SharepointThemes_None()
    {
        TestHelper.Instance.RunTest("-comments:none");
    }
  }
}

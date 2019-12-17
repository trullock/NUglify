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
  /// Summary description for AtRules
  /// </summary>
  [TestFixture]
  public class AtRules
  {
    [Test]
    public void Media()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Import()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Page()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void FontFace()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Charset()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Namespace()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void Other()
    {
      TestHelper.Instance.RunTest();
    }

    [Test]
    public void ImportComment()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
      public void ImportComment_c()
    {
        TestHelper.Instance.RunTest("-comments:all");
    }

    [Test]
    public void ImportComment_x()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [Test]
    public void KeyFrames()
    {
        TestHelper.Instance.RunTest();
    }

    [Test]
    public void KeyFrames_p()
    {
        TestHelper.Instance.RunTest("-pretty");
    }

    [Test]
    public void KeyFrames_same()
    {
        TestHelper.Instance.RunTest("-pretty -braces:same");
    }

    [Test]
    public void KeyFrames_source()
    {
        TestHelper.Instance.RunTest("-pretty -braces:source");
    }

    [Test]
    public void RazorEscapedDoubleAt()
    {
      TestHelper.Instance.RunTest("-razor");
    }


    [Test]
    public void Supports()
    {
        TestHelper.Instance.RunTest();
    }


    }
}

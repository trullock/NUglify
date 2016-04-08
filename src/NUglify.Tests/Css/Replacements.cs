// Replacements.cs
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
  /// Summary description for Replacements
  /// </summary>
  [TestFixture]
  public class Replacements
  {
      [Test]
      public void ValueReplacement()
      {
          TestHelper.Instance.RunTest("-res");
      }

      [Test]
      public void MajorColors()
      {
          TestHelper.Instance.RunTest("-colors:major");
      }

      [Test]
      public void MajorColors_strict()
      {
          TestHelper.Instance.RunTest("-colors:strict");
      }

      [Test]
      public void ColorNames_strict()
      {
          TestHelper.Instance.RunTest("-colors:strict");
      }

      [Test]
      public void ColorNames_hex()
      {
          TestHelper.Instance.RunTest("-colors:hex");
      }

      [Test]
      public void ColorNames_major()
      {
          TestHelper.Instance.RunTest("-colors:major");
      }

      [Test]
      public void ColorNames_noswap()
      {
          // noswap
          TestHelper.Instance.RunTest("-colors:noswap");
      }

      [Test]
      public void CssReplacementTokens()
      {
          TestHelper.Instance.RunTest();
      }

      [Test]
      public void Ie8Eot()
      {
          TestHelper.Instance.RunTest();
      }
  }
}

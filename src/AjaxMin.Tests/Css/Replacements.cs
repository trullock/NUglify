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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssUnitTest
{
  /// <summary>
  /// Summary description for Replacements
  /// </summary>
  [TestClass]
  public class Replacements
  {
      [TestMethod]
      public void ValueReplacement()
      {
          TestHelper.Instance.RunTest("-res");
      }

      [TestMethod]
      public void MajorColors()
      {
          TestHelper.Instance.RunTest("-colors:major");
      }

      [TestMethod]
      public void MajorColors_strict()
      {
          TestHelper.Instance.RunTest("-colors:strict");
      }

      [TestMethod]
      public void ColorNames_strict()
      {
          TestHelper.Instance.RunTest("-colors:strict");
      }

      [TestMethod]
      public void ColorNames_hex()
      {
          TestHelper.Instance.RunTest("-colors:hex");
      }

      [TestMethod]
      public void ColorNames_major()
      {
          TestHelper.Instance.RunTest("-colors:major");
      }

      [TestMethod]
      public void ColorNames_noswap()
      {
          // noswap
          TestHelper.Instance.RunTest("-colors:noswap");
      }

      [TestMethod]
      public void CssReplacementTokens()
      {
          TestHelper.Instance.RunTest();
      }

      [TestMethod]
      public void Ie8Eot()
      {
          TestHelper.Instance.RunTest();
      }
  }
}

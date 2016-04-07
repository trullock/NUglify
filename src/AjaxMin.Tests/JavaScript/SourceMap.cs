// SourceMap.cs
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
    /// unit tests dealing with the MAP input used to generate node mapping between source and output scripts
    /// </summary>
    [TestClass]
    public class SourceMap
    {
        [TestMethod]
        public void MapArgNotSpecified()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void ScriptSharpMap_TwoInputs()
        {
            TestHelper.Instance.RunTest("-map", "MapArgNotSpecified.js");
        }

        [TestMethod]
        public void SourceMapV3()
        {
            TestHelper.Instance.RunTest("-map:v3", "ScriptSharpMap.js", "MapArgNotSpecified.js");
        }
    }
}


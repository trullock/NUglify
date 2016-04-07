// Modules.cs
//
// Copyright 2013 Microsoft Corporation
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

using Microsoft.Ajax.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSUnitTest
{
    [TestClass]
    public class Modules
    {
        [TestMethod]
        public void NotModules()
        {
            TestHelper.Instance.RunErrorTest("-unused:keep -ignore:JS1310,JS1135");
        }

        [TestMethod]
        public void Module()
        {
            TestHelper.Instance.RunErrorTest("-unused:keep");
        }

        [TestMethod]
        public void Imports()
        {
            TestHelper.Instance.RunErrorTest("-unused:keep");
        }

        [TestMethod]
        public void ImportRename()
        {
            TestHelper.Instance.RunErrorTest();
        }

        [TestMethod]
        public void Exports()
        {
            TestHelper.Instance.RunErrorTest("-unused:keep");
        }

        [TestMethod]
        public void ExportRename()
        {
            TestHelper.Instance.RunErrorTest();
        }

        [TestMethod]
        public void ExportCombine()
        {
            TestHelper.Instance.RunErrorTest();
        }

        [TestMethod]
        public void NoExportInline()
        {
            // if we analyze the exports of the inline module, we should see that the expected import isn't actually exported
            TestHelper.Instance.RunErrorTest(/*JSError.NoModuleExport*/);
        }

        [TestMethod]
        public void NoExportExternal()
        {
            // if we load the module and check, we should see that the expected import isn't actually exported
            TestHelper.Instance.RunErrorTest(/*JSError.NoModuleExport*/);
        }

        [TestMethod]
        public void ReExport()
        {
            TestHelper.Instance.RunErrorTest(JSError.UndeclaredFunction, JSError.UndeclaredVariable);
        }

        [TestMethod]
        public void BadExport()
        {
            TestHelper.Instance.RunErrorTest(JSError.ExportNotAtModuleLevel);
        }

        [TestMethod]
        public void ImportAssign()
        {
            TestHelper.Instance.RunErrorTest(JSError.AssignmentToConstant);
        }

        //[TestMethod]
        //public void NoExportExternal_flatten()
        //{
        //    TestHelper.Instance.RunErrorTest("-imports:flatten", JSError.NoModuleExport);
        //}

        //[TestMethod]
        //public void CircularExternal()
        //{
        //    TestHelper.Instance.RunErrorTest("-imports:flatten");
        //}
    }
}

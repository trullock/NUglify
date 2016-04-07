using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSUnitTest
{
    [TestClass]
    public class Renaming
    {
        [TestMethod]
        public void NestedGlobals()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void ManualRename()
        {
            TestHelper.Instance.RunTest("-enc:out ascii");
        }

        [TestMethod]
        public void ManualRename_cmd()
        {
            TestHelper.Instance.RunTest("-rename:globalFunction=_g,oneGlobal=g1,oneLocal=l1 -rename:oneParam=p1,twoParam=p2,nameOne=n1,你好=中文,while=for -enc:out ascii");
        }

        [TestMethod]
        public void ManualRename_rename()
        {
            TestHelper.Instance.RunTest("-rename Rename.xml -enc:out ascii");
        }

        [TestMethod]
        public void ManualRename_noprops()
        {
            TestHelper.Instance.RunTest("-rename Rename.xml -rename:noprops -enc:out ascii");
        }

        [TestMethod]
        public void ManualRename_all()
        {
            TestHelper.Instance.RunTest("-rename:all -enc:out ascii");
        }

        [TestMethod]
        public void ManualRename_collide()
        {
            TestHelper.Instance.RunTest("-rename:all -rename Collide.xml");
        }

        [TestMethod]
        public void ManualRename_norename()
        {
            TestHelper.Instance.RunTest("-rename:all -rename NoRename.xml -enc:out ascii");
        }

        [TestMethod]
        public void Super()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void NoMunge()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void IfFunction()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [TestMethod]
        public void SafeAll()
        {
            TestHelper.Instance.RunTest("-rename:all -evals:safeall");
        }

        [TestMethod]
        public void SafeAll_imm()
        {
            TestHelper.Instance.RunTest("-rename:all -evals:immediate");
        }

        [TestMethod]
        public void LabelKeyword()
        {
            // ignore the unused-label warning
            TestHelper.Instance.RunErrorTest("-ignore:JS1021");
        }

        [TestMethod]
        public void evalImmediate()
        {
            // ignore the unused-label warning
            TestHelper.Instance.RunErrorTest("-evals:immediate");
        }
    }
}

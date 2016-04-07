using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssUnitTest
{
    /// <summary>
    /// Summary description for Values
    /// </summary>
    [TestClass]
    public class Values
    {
        [TestMethod]
        public void Calc()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Cycle()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Attr()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void ProgID()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Grids()
        {
            var retValue = TestHelper.Instance.RunTest();
            Assert.IsTrue(retValue == 0, "shouldn't have any errors");
        }

        [TestMethod]
        public void Units()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Zeros()
        {
            TestHelper.Instance.RunTest();
        }

        [TestMethod]
        public void Toggle()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

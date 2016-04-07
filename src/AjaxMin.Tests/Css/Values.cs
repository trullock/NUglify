using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CssUnitTest
{
    /// <summary>
    /// Summary description for Values
    /// </summary>
    [TestFixture]
    public class Values
    {
        [Test]
        public void Calc()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Cycle()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Attr()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ProgID()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Grids()
        {
            var retValue = TestHelper.Instance.RunTest();
            Assert.IsTrue(retValue == 0, "shouldn't have any errors");
        }

        [Test]
        public void Units()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Zeros()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Toggle()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

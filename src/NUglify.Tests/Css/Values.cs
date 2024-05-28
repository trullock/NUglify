using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
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
            Assert.That(retValue == 0, "shouldn't have any errors");
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

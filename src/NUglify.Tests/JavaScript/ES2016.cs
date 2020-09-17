using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2016
    {
        [Test]
        public void Exponent()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ExponentAssign()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

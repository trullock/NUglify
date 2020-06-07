using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class Bugs
    {
        [Test]
        public void Bug78()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug92()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

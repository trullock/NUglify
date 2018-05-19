using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2017
    {
        [Test]
        public void AsyncAwait()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void TrailingComma()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

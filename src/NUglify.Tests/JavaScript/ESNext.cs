using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ESNext
    {
        [Test]
        public void OptionalChaining()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

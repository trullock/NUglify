using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2020
    {
        [Test]
        public void NullCoalesce()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

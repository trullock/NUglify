using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2018
    {
        [Test]
        public void SpreadOperator()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

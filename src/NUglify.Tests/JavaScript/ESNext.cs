using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ESNext
    {
        [Test]
        public void GlobalThis()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }
    }
}

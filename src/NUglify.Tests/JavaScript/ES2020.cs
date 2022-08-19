using NUglify.JavaScript;
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

        [Test]
        public void OptionalChaining()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void OptionalChainingLineBreak() 
        {
            TestHelper.Instance.RunTest("-line:1");
        }

        [Test]
        public void GlobalThis()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void BigIntInitialisation()
        {
            TestHelper.Instance.RunTest();
        }

    }
}

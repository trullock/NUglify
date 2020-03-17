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

        [Test]
        public void AsyncFunctionExpressions()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void AsyncClassFunction()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void AsyncArrowFunction()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void MultipleAwaitExpression()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

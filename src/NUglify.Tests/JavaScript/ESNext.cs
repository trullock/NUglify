using NUglify.JavaScript;
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

        [Test]
        public void NumericSeparatorsBad()
        {
            TestHelper.Instance.RunErrorTest(JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral, JSError.BadNumericLiteral);
        }

        [Test]
        public void NumericSeparatorsInteger()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

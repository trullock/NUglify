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
        public void NumericSeparatorsInteger()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsDecimal1()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsDecimal2()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsExponent1()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsExponent2()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsBinary1()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsBinary2()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsHex1()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsHex2()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsOctal1()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NumericSeparatorsOctal2()
        {
            TestHelper.Instance.RunTest();
        }


        [Test]
        public void NumericSeparatorsNoTrailing()
        {
            TestHelper.Instance.RunErrorTest(JSError.BadNumericLiteral);
        }

        [Test]
        public void NumericSeparatorsNoAdjacent()
        {
            TestHelper.Instance.RunErrorTest(JSError.BadNumericLiteral);
        }

        [Test]
        public void NumericSeparatorsDecimalBefore()
        {
            TestHelper.Instance.RunErrorTest(JSError.BadNumericLiteral);
        }

        [Test]
        public void NumericSeparatorsDecimalAfter()
        {
            TestHelper.Instance.RunErrorTest(JSError.BadNumericLiteral);
        }
    }
}

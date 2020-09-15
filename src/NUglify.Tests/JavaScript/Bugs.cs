using System.IO;
using System.Diagnostics;
using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class Bugs
    {
      
		[Test]
        public void Bug35()
        {
            TestHelper.Instance.RunErrorTest();
        }

		[Test]
        public void Bug57()
        {
            TestHelper.Instance.RunErrorTest();
        }
      
		[Test]
        public void Bug63()
        {
            TestHelper.Instance.RunErrorTest(JSError.NoLeftParenthesis, JSError.ExpressionExpected, JSError.NoLeftCurly, JSError.BadSwitch);
        }

		[Test]
        public void Bug70()
        {
            TestHelper.Instance.RunTest();
        }


		[Test]
        public void Bug76()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

		
		[Test]
        public void Bug78()
        {
            TestHelper.Instance.RunTest();
        }
		
        [Test]
        public void Bug79()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void Bug80()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug87()
        {
            TestHelper.Instance.RunTest("-reorder:false -fnames:lock -term:true -unused:remove");
        }

        [Test]
        public void Bug92()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug94()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug120()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug138()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug139()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug139_A()
        {
            // previously these would throw exceptions because the closing backtick was skipped, creating an invalid ast
            // manually define cr and lfs to be sure we dont have platform sillynes

            var uglifyResult = Uglify.Js(@"var testString = `
`;
            testString += `} async init(){ }`;");
            Assert.AreEqual("var testString=`\n`+`} async init(){ }`", uglifyResult.Code);

            // CR
            uglifyResult = Uglify.Js("var testString = `"+ (char)13 +@"`;
            testString += `} async init(){ }`;");
            Assert.AreEqual("var testString=`\r`+`} async init(){ }`", uglifyResult.Code);

            // LF
            uglifyResult = Uglify.Js("var testString = `" + (char)10 + @"`;
            testString += `} async init(){ }`;");
            Assert.AreEqual("var testString=`\n`+`} async init(){ }`", uglifyResult.Code);

            // CRLF
            uglifyResult = Uglify.Js("var testString = `" + (char)13 + (char)10 + @"`;
            testString += `} async init(){ }`;");
            Assert.AreEqual("var testString=`\r\n`+`} async init(){ }`", uglifyResult.Code);

            //LFCR
            uglifyResult = Uglify.Js("var testString = `" + (char)10 + (char)13 + @"`;
            testString += `} async init(){ }`;");
            Assert.AreEqual("var testString=`\n\r`+`} async init(){ }`", uglifyResult.Code);

        }
    }
}

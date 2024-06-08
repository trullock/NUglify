using System;
using System.Diagnostics;
using NUglify.Css;
using NUglify.Helpers;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for CssErrorStrings
    /// </summary>
    [TestFixture]
    public class ErrorStrings
    {
        [Test]
        public void CssErrorStringsExist()
        {
            var hasFailed = false;
            foreach (var cssErrorName in Enum.GetNames(typeof(CssErrorCode)))
            {
                if (cssErrorName != "NoError")
                {
                    CssErrorCode errorCode;
                    if (Enum.TryParse(cssErrorName, out errorCode))
                    {
                        var message = CssParser.ErrorFormat(errorCode);
                        if (message.IsNullOrWhiteSpace())
                        {
                            Trace.WriteLine(cssErrorName + " has no corresponding error message");
                            hasFailed = true;
                        }
                    }
                    else
                    {
                        Trace.WriteLine(cssErrorName + " failed to parse back into enum");
                        hasFailed = true;
                    }
                }
            }

            Assert.That(!hasFailed);
        }

        [Test]
        public void JSErrorStringsExist()
        {
            var hasFailed = false;
            foreach (var jsErrorName in Enum.GetNames(typeof(JSError)))
            {
                if (jsErrorName != "NoError")
                {
                    JSError errorCode;
                    if (Enum.TryParse(jsErrorName, out errorCode))
                    {
                        var message = SourceContext.GetErrorString(errorCode);
                        if (message.IsNullOrWhiteSpace())
                        {
                            Trace.WriteLine(jsErrorName + " has no corresponding error message");
                            hasFailed = true;
                        }
                    }
                    else
                    {
                        Trace.WriteLine(jsErrorName + " failed to parse back into enum");
                        hasFailed = true;
                    }
                }
            }

            Assert.That(!hasFailed);
        }

//#if DEBUG
//        [Test]
//        public void JSErrorStringsExtra()
//        {
//            var hasFailed = false;
//            var count = 0;
//            foreach (var stringName in ErrorStringHelper.AvailableJSStrings)
//            {
//                ++count;
//                JSError errorCode;
//                if (!Enum.TryParse(stringName, out errorCode))
//                {
//                    Trace.WriteLine(stringName + " has no corresponding JSError enumeration");
//                    hasFailed = true;
//                }
//            }

//            Assert.IsFalse(hasFailed);
//            Assert.IsTrue(count > 0, "didn't get ANY properties");
//        }

//        [Test]
//        public void CssErrorStringsExtra()
//        {
//            var hasFailed = false;
//            var count = 0;
//            foreach (var stringName in ErrorStringHelper.AvailableCssStrings)
//            {
//                ++count;
//                CssErrorCode errorCode;
//                if (!Enum.TryParse(stringName, out errorCode))
//                {
//                    Trace.WriteLine(stringName + " has no corresponding CssErrorCode enumeration");
//                    hasFailed = true;
//                }
//            }

//            Assert.IsFalse(hasFailed);
//            Assert.IsTrue(count > 0, "didn't get ANY properties");
//        }
//#endif
    }
}

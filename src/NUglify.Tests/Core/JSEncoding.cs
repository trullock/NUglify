// JSEncoding.cs
//
// Copyright 2013 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for CssErrorStrings
    /// </summary>
    [TestFixture]
    public class JSEncoding
    {
        [Test]
        public void UnicodeEscapedHighSurrogate()
        {
            // we have to do this test in C# because technically if a text file had this code in it,
            // it would be an invalid surrogate pair and wouldn't even read properly. So this isn't
            // REALLY a valid valid real-world test scenario, but I still want it to work properly.
            // the high-surrogate is escaped, and the low isn't -- which mean we have a low without a matching high.
            var source = "var str = '\\ud83d\ude80';";
            var minified = Uglify.Js(source);
            foreach (var error in minified.Errors)
            {
                Trace.WriteLine(error.ToString());
            }

            Assert.That(minified.Code, Is.EqualTo("var str=\"🚀\""));
            Assert.That(!minified.HasErrors);
        }

        [Test]
        public void UnicodeEscapedLowSurrogate()
        {
            // we have to do this test in C# because technically if a text file had this code in it,
            // it would be an invalid surrogate pair and wouldn't even read properly. So this isn't
            // REALLY a valid valid real-world test scenario, but I still want it to work properly.
            // the low-surrogate is escaped -- which mean we have a high without a matching low.
            var source = "var str = '\ud83d\\ude80';";
            var minified = Uglify.Js(source);
            foreach (var error in minified.Errors)
            {
                Trace.WriteLine(error.ToString());
            }

            Assert.That(minified.Code, Is.EqualTo("var str=\"🚀\""));
            Assert.That(!minified.HasErrors);
        }

        [Test]
        public void SurrogatePairEscapedIdentifier()
        {
            // escaped surrogate pair as an identifier
            var source = "var \\ud840\\udc2f = 'foo';";
            var minified = Uglify.Js(source);
            foreach (var error in minified.Errors)
            {
                Trace.WriteLine(error.ToString());
            }

            Assert.That(minified.Code, Is.EqualTo("var 𠀯=\"foo\""));
            Assert.That(!minified.HasErrors);
        }

        [Test]
        public void SurrogatePairIdentifier()
        {
            // raw surrogate pair as an identifier
            var source = "var \ud840\udc2d = 'foo';";
            var minified = Uglify.Js(source);
            foreach (var error in minified.Errors)
            {
                Trace.WriteLine(error.ToString());
            }

            Assert.That(minified.Code, Is.EqualTo("var 𠀭=\"foo\""));
            Assert.That(!minified.HasErrors);
        }

        [Test]
        public void BadUnicodeIdentifier()
        {
            // make sure that a \u that isn't a hex escape is preserved.
            // although it SHOULD throw an error for not being a valid unicode escape.
            var source = "var \\umberland = 'north';";
            var minified = Uglify.Js(source);
            string firstErrorCode = null;
            foreach (var error in minified.Errors)
            {
                Trace.WriteLine(error.ToString());
                if (firstErrorCode == null)
                {
                    firstErrorCode = error.ErrorCode;
                }
            }

            Assert.That(minified.Code, Is.EqualTo("var \\umberland=\"north\""));
            Assert.That(minified.Errors.Count, Is.EqualTo(1));
            Assert.That(firstErrorCode, Is.EqualTo("JS1023"));
        }
    }
}

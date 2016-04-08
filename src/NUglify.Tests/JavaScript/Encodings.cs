// Encodings.cs
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

using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for Encoding
    /// </summary>
    [TestFixture]
    public class Encodings
    {
        [Test]
        public void UnicodeExtended()
        {
            TestHelper.Instance.RunErrorTest();
        }

        [Test]
        public void UnicodeExtended_ascii()
        {
            TestHelper.Instance.RunErrorTest("-enc:out ascii");
        }

        [Test]
        public void SurrogateHighError()
        {
            TestHelper.Instance.RunErrorTest(JSError.HighSurrogate);
        }

        [Test]
        public void SurrogateLowError()
        {
            TestHelper.Instance.RunErrorTest(JSError.LowSurrogate);
        }
    }
}

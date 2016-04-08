// XmlInput.cs
//
// Copyright 2010 Microsoft Corporation
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

using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// unit tests dealing with the XML input fklag (-x) which can specify a series of
    /// input and output files in an XML file for a single instance of tool execution
    /// </summary>
    [TestFixture]
    public class XmlInput
    {
        [Test]
        public void XmlOneOutputFile()
        {
            TestHelper.Instance.RunTest("-xml -reorder:0");
        }

        [Test]
        public void XmlTwoOutputFiles()
        {
            TestHelper.Instance.RunTest("-xml -reorder:N");
        }

        [Test]
        public void XmlWithSymbolMap()
        {
            TestHelper.Instance.RunTest("-xml -reorder:N");
        }

        [Test]
        public void XmlMixedType()
        {
            TestHelper.Instance.RunTest("-xml -reorder:F");
        }

        [Test]
        public void EncInputNone()
        {
            // neither Russian nor Chinese should be decoded properly
            TestHelper.Instance.RunTest("-xml");
        }

        [Test]
        public void EncInputNone_koi8r()
        {
            // Russian will be decoded properly, but not the Chinese
            TestHelper.Instance.RunTest("-xml -enc:in koi8-r");
        }

        [Test]
        public void EncInputRussian()
        {
            // Russian has encoding inline; will be decoded properly, but not the Chinese
            // output should be utf-8
            TestHelper.Instance.RunTest("-xml -enc:out utf-8");
        }

        [Test]
        public void EncInputRussian_big5()
        {
            // Russian has encoding inline; will be decoded properly.
            // Chinese big5 encoding specified as default, so both will be decoded properly
            TestHelper.Instance.RunTest("-xml -enc:in big5 -enc:out utf-8");
        }

        [Test]
        public void EncInputRussian_big5out()
        {
            // Russian has encoding inline; will be decoded properly.
            // Chinese big5 encoding specified as default, so both will be decoded properly.
            // but we're output-encoding to koi8-r, so the russian should be good-to-go, but the
            // Chinese should be JS-encoded.
            TestHelper.Instance.RunTest("-xml -enc:in big5 -enc:out koi8-r");
        }
    }
}

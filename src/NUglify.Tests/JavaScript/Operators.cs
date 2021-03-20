// Operators.cs
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

using NUglify.JavaScript;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for Operators
    /// </summary>
    [TestFixture]
    public class Operators
    {
        [Test]
        public void Member()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void In()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Void()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Unary()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Assign()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Conditional()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Comma()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void New()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void InstanceOf()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void NewPrecedence()
        {
            TestHelper.Instance.RunTest("-new:keep");
        }

        [Test]
        public void Associative()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Strict()
        {
            TestHelper.Instance.RunTest("-kill:0x0000001000000000 -ignore:JS1287");
        }

        [Test]
        public void ConditionalPrecedence()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void TypeOf()
        {
            TestHelper.Instance.RunErrorTest(JSError.WithNotRecommended);
        }
    }
}

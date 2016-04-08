// ResourceMerge.cs
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

using System;
using System.IO;
using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    /// <summary>
    /// Summary description for ResourceMerge
    /// </summary>
    [TestFixture]
    public class ResourceMerge
    {
        public ResourceMerge()
        {
            // Bug in Resharper
            Environment.CurrentDirectory = Path.GetDirectoryName(typeof (ResourceMerge).Assembly.Location);
        }


        [Test]
        public void ResourceResx()
        {
            TestHelper.Instance.RunTest("-res:Strings");
        }

        [Test]
        public void ResourceResx_I()
        {
            TestHelper.Instance.RunTest("-res:Strings -echo -enc:out ascii");
        }

        [Test]
        public void Resources()
        {
            TestHelper.Instance.RunTest("-res:Strings -rename:all -literals:combine");
        }

        [Test]
        public void StringsFooBar()
        {
            TestHelper.Instance.RunTest("-res:Strings.Foo.Bar");
        }

        [Test]
        public void ReplacementTokens()
        {
            // parse replacement tokens without error
            TestHelper.Instance.RunErrorTest("-rename:none");
        }
    }
}

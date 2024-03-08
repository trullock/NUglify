// AtRules.cs
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

using NUglify.Css;
using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
{
    /// <summary>
    /// Summary description for AtRules
    /// </summary>
    [TestFixture]
    public class Bugs
    {
        [Test]
        public void Bug33()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug56()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug74()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug181()
        {
	        var uglifyResult = Uglify.Css("p { color: red; }", new CssSettings { Indent = "   ", OutputMode = OutputMode.MultipleLines });
	        Assert.AreEqual("p\n{\n   color: #f00\n}", uglifyResult.Code);
        }

        [Test]
        public void Bug250()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug270()
        {
	        TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug280()
        {
            Assert.AreEqual(":is(.container) :is(.bold){font-weight:bold}", Uglify.Css(":is(.container) :is(.bold) { font-weight: bold; }").Code);

            Assert.AreEqual(":is(.container):is(.bold){font-weight:bold}", Uglify.Css(":is(.container):is(.bold) { font-weight: bold; }").Code);

            Assert.AreEqual(".container :is(.bold){font-weight:bold}", Uglify.Css(".container :is(.bold) { font-weight: bold; }").Code);

            // Assert whether extra unnecessary spaces around parentheses are removed:
            Assert.AreEqual(":is(.container) :is(.bold){font-weight:bold}", Uglify.Css(":is(.container ) :is(.bold ) { font-weight: bold; }").Code);

            // Assert a more extensive scenario:
            Assert.AreEqual("body{font-family:'Franklin Gothic Medium','Arial Narrow',Arial,sans-serif}:root{--blue:'blue';--five:5px;--width:100px;--half-width:calc(var(--width)/2);--half-width-with-indent:calc(var(--half-width) + 16px);--half-width-with-indent:calc(10% + var(--half-width) + 16px)}.margin{margin:calc(2*var(--five));padding:var(--half-width-with-indent)}:is(.container) :is(.bold){font-weight:bold}:is(.container) :where(.bold){grid-template-areas:'myArea myArea myArea myArea myArea'}div :is(.test),div:is(.test){color:var(--blue)}",
                 Uglify.Css(@"body {
    font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
}

:root {
    --blue: 'blue';
    --five: 5px;
    --width: 100px;
    --half-width: calc(var(--width) / 2);
    --half-width-with-indent: calc(var(--half-width) + 16px);
    --half-width-with-indent: calc(10% + var(--half-width) + 16px);
}

.margin {
    margin: calc(2 * var(--five));
    padding: var(--half-width-with-indent);
}


:is(.container) :is(.bold) {
    font-weight: bold;
}
:is(.container) :where(.bold) {
    grid-template-areas: 'myArea myArea myArea myArea myArea';
}
div :is(.test),
div:is(.test) {
    color: var(--blue);
}
").Code);
        }

        [Test]
        public void Bug302()
        {
	        Assert.AreEqual("p{background-color:var(--_flumo-grid-secondary-border-color)!important}", Uglify.Css(@"
p {
	background-color: var(--_flumo-grid-secondary-border-color) !important;
}").Code);
        }

        [Test]
        public void Bug309()
        {
	        Assert.AreEqual("body{border-top-width:.5vmax;--custom-property:0px}", Uglify.Css(@"
body
{
	border-top-width: 0.5vmax;
	--custom-property: 0px; 
}").Code);
        }


        [Test]
        public void Bug331()
        {
	        TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug317()
        {
	        Assert.AreEqual("body{border:0;border-top:0;border-right:0;border-bottom:0;border-left:0;outline:0}", Uglify.Css(@"
body
{
	border: none;
	border-top: none;
	border-right: none;
	border-bottom: none;
	border-left: none;
	outline: none;
}").Code);
        }

        [Test]
        public void Bug343()
        {
            Assert.AreEqual("body{border:0 none;border:0 none #f00}", Uglify.Css(@"
body
{
	border: 0 none;
	border: 0 none #f00;
}").Code);
        }


        [Test]
        public void Bug379()
        {
	        Assert.AreEqual(".\\!inline-flex{display:inline-flex!important}", Uglify.Css(@"
.\!inline-flex {
    display: inline-flex!important
}
").Code);
        }

        [Test]
        public void Bug383()
        {
            var result = Uglify.Css("""
@supports not selector(::-webkit-scrollbar) {
    .scroll {
        scrollbar-color: rgba(0,0,0,0.3) rgba(0,0,0,0);
        scrollbar-width: thin;
    }
}
""");
            Assert.False(result.HasErrors);
        }
    }
}
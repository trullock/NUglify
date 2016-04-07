using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Ajax.Utilities;
using NUnit.Framework;

namespace DllUnitTest
{
    /// <summary>
    /// Summary description for JSONValidate
    /// </summary>
    [TestFixture]
    public class JSONValidate
    {
        [Test]
        public void ValidJSONLiterals()
        {
            Assert.AreEqual("true", JSON.Validate("\t\n\r true  "));
            Assert.AreEqual("false", JSON.Validate("\t\n\r false  "));
            Assert.AreEqual("null", JSON.Validate("\t\n\r null  "));
        }

        [Test]
        public void ValidJSONNumbers()
        {
            Assert.AreEqual("0", JSON.Validate("\t\n\r 0  "));
            Assert.AreEqual("123", JSON.Validate("\t\n\r 123  "));
            Assert.AreEqual("123.456", JSON.Validate("\t\n\r 123.456  "));
            Assert.AreEqual("-0", JSON.Validate("\t\n\r -0  "));
            Assert.AreEqual("-123", JSON.Validate("\t\n\r -123  "));
            Assert.AreEqual("-123.456", JSON.Validate("\t\n\r -123.456  "));
            Assert.AreEqual("1e3", JSON.Validate("\t\n\r 1e3  "));
            Assert.AreEqual("1e-33", JSON.Validate("\t\n\r 1e-33  "));
            Assert.AreEqual("123E+300", JSON.Validate("\t\n\r 123E+300  "));
        }

        [Test]
        public void ValidJSONStrings()
        {
            Assert.AreEqual("\"\"", JSON.Validate("\t\n\r \"\"  "));
            Assert.AreEqual("\"123\"", JSON.Validate("\t\n\r \"123\"  "));
            Assert.AreEqual("\"\\\"\\\\\\b\\f\\n\\r\\t\"", JSON.Validate("\t\n\r \"\\\"\\\\\\b\\f\\n\\r\\t\"  "));
            Assert.AreEqual("\"\\u02bb\"", JSON.Validate("\t\n\r \"\\u02bb\"  "));
        }

        [Test]
        public void ValidJSONArrays()
        {
            Assert.AreEqual("[]", JSON.Validate("\t\n\r [   ]  "));
            Assert.AreEqual("[[1,\"a\",true],[2,null,false],[3,{},[]],{\"a\":\"b\"}]", JSON.Validate("\t\n\r [ [ 1, \"a\", true ] , [ 2 , null , false ] , [ 3 , { } , [ ] ] , { \"a\" : \"b\"}  ]  "));
        }

        [Test]
        public void ValidJSONObjects()
        {
            Assert.AreEqual("{}", JSON.Validate("\t\n\r {  }  "));
            Assert.AreEqual("{\"one\":1,\"two\":true,\"three\":false,\"four\":null}", JSON.Validate("\t\n\r { \"one\" : 1 , \"two\" : true , \"three\" : false , \"four\" : null }  "));
            Assert.AreEqual("{\"one\":{},\"two\":[{},{\"two\":2}]}", JSON.Validate("\t\n\r { \"one\" : {\r\n} ,\t\"two\" : [ { } ,{ \"two\":2} ] }  "));
        }
    }
}

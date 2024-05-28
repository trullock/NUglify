using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
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
            Assert.That(JsonParser.Validate("\t\n\r true  "), Is.EqualTo("true"));
            Assert.That(JsonParser.Validate("\t\n\r false  "), Is.EqualTo("false"));
            Assert.That(JsonParser.Validate("\t\n\r null  "), Is.EqualTo("null"));
        }

        [Test]
        public void ValidJSONNumbers()
        {
            Assert.That(JsonParser.Validate("\t\n\r 0  "), Is.EqualTo("0"));
            Assert.That(JsonParser.Validate("\t\n\r 123  "), Is.EqualTo("123"));
            Assert.That(JsonParser.Validate("\t\n\r 123.456  "), Is.EqualTo("123.456"));
            Assert.That(JsonParser.Validate("\t\n\r -0  "), Is.EqualTo("-0"));
            Assert.That(JsonParser.Validate("\t\n\r -123  "), Is.EqualTo("-123"));
            Assert.That(JsonParser.Validate("\t\n\r -123.456  "), Is.EqualTo("-123.456"));
            Assert.That(JsonParser.Validate("\t\n\r 1e3  "), Is.EqualTo("1e3"));
            Assert.That(JsonParser.Validate("\t\n\r 1e-33  "), Is.EqualTo("1e-33"));
            Assert.That(JsonParser.Validate("\t\n\r 123E+300  "), Is.EqualTo("123E+300"));
        }

        [Test]
        public void ValidJSONStrings()
        {
            Assert.That(JsonParser.Validate("\t\n\r \"\"  "), Is.EqualTo("\"\""));
            Assert.That(JsonParser.Validate("\t\n\r \"123\"  "), Is.EqualTo("\"123\""));
            Assert.That(JsonParser.Validate("\t\n\r \"\\\"\\\\\\b\\f\\n\\r\\t\"  "), Is.EqualTo("\"\\\"\\\\\\b\\f\\n\\r\\t\""));
            Assert.That(JsonParser.Validate("\t\n\r \"\\u02bb\"  "), Is.EqualTo("\"\\u02bb\""));
        }

        [Test]
        public void ValidJSONArrays()
        {
            Assert.That(JsonParser.Validate("\t\n\r [   ]  "), Is.EqualTo("[]"));
            Assert.That(JsonParser.Validate("\t\n\r [ [ 1, \"a\", true ] , [ 2 , null , false ] , [ 3 , { } , [ ] ] , { \"a\" : \"b\"}  ]  "), Is.EqualTo("[[1,\"a\",true],[2,null,false],[3,{},[]],{\"a\":\"b\"}]"));
        }

        [Test]
        public void ValidJSONObjects()
        {
            Assert.That(JsonParser.Validate("\t\n\r {  }  "), Is.EqualTo("{}"));
            Assert.That(JsonParser.Validate("\t\n\r { \"one\" : 1 , \"two\" : true , \"three\" : false , \"four\" : null }  "), Is.EqualTo("{\"one\":1,\"two\":true,\"three\":false,\"four\":null}"));
            Assert.That(JsonParser.Validate("\t\n\r { \"one\" : {\r\n} ,\t\"two\" : [ { } ,{ \"two\":2} ] }  "), Is.EqualTo("{\"one\":{},\"two\":[{},{\"two\":2}]}"));
        }
    }
}

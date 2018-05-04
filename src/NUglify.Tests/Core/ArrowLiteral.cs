using NUglify.JavaScript;
using NUglify.JavaScript.Visitors;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    [TestFixture]
    public class ArrowLiteral
    {
        [Test]
        public void LiteralCore()
        {
            var source = "var arrow = n => { return { x: n + 1, y: n - 1 }; };";
            var expected = "var arrow=n=>{return{x:n+1,y:n-1}}";
            var settings = new CodeSettings();
            var parser = new JSParser();
            var code = parser.Parse(source, settings);

            var minified = OutputVisitor.Apply(code, settings);

            Assert.AreEqual(expected, minified);
        }
    }
}

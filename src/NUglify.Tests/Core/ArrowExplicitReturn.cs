using NUglify.JavaScript;
using NUglify.JavaScript.Visitors;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    [TestFixture]
    public class ArrowExplicitReturn
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

        [Test]
        public void CommaExpressionCore()
        {
            var source = "var arrow = n => { delete n.x; return n; };";
            var expected = "var arrow=n=>{return delete n.x,n}";
            var settings = new CodeSettings();
            var parser = new JSParser();
            var code = parser.Parse(source, settings);

            var minified = OutputVisitor.Apply(code, settings);

            Assert.AreEqual(expected, minified);
        }
    }
}

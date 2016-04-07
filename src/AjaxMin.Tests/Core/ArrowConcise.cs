using AjaxMin.JavaScript;
using AjaxMin.JavaScript.Syntax;
using AjaxMin.JavaScript.Visitors;
using NUnit.Framework;

namespace AjaxMin.Tests.Core
{
    /// <summary>
    /// Summary description for ArrowConcise
    /// </summary>
    [TestFixture]
    public class ArrowConcise
    {
        private const string source = "var arrow = (a, b, c) => a * ( b + c);";
        private const string appendDebugger = "var arrow=(n,t,i)=>{return n*(t+i);debugger}";
        private const string insertDebugger = "var arrow=(n,t,i)=>{debugger;return n*(t+i)}";
        private const string insertTwo = "var arrow=(n,t,i)=>{n;debugger;return n*(t+i)}";
        private const string emptyBody = "var arrow=(n,t,i)=>{}";
        private const string justDebugger = "var arrow=(n,t,i)=>{debugger}";
        private const string justLookup = "var arrow=(n,t,i)=>n";

        [Test]
        public void ConciseAppend()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we append something to the arrow body, it should no longer be concise
            // and the first statement should be a return node.
            body.Append(new DebuggerNode(body[0].Context.FlattenToEnd()));
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body[0] is ReturnStatement);
            Assert.IsTrue(body[1] is DebuggerNode);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(appendDebugger, minified);
        }

        [Test]
        public void ConciseInsertAfter()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert a debugger statement after the first statement, it should no longer be concise
            body.InsertAfter(body[0], new DebuggerNode(body[0].Context.FlattenToEnd()));
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 2);
            Assert.IsTrue(body[0] is ReturnStatement);
            Assert.IsTrue(body[1] is DebuggerNode);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(appendDebugger, minified);
        }

        [Test]
        public void ConciseInsert()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert a debugger statement before the first statement, it should no longer be concise
            body.Insert(0, new DebuggerNode(body[0].Context.FlattenToStart()));
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 2);
            Assert.IsTrue(body[0] is DebuggerNode);
            Assert.IsTrue(body[1] is ReturnStatement);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(insertDebugger, minified);
        }

        [Test]
        public void ConciseInsertRange()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert two statements before the first statement, it should no longer be concise
            body.InsertRange(0, new AstNode[] { GetLookupToFirstParameter(body.Parent as FunctionObject), new DebuggerNode(body[0].Context.FlattenToStart()) });
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 3);
            Assert.IsTrue(body[0] is LookupExpression);
            Assert.IsTrue(body[1] is DebuggerNode);
            Assert.IsTrue(body[2] is ReturnStatement);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(insertTwo, minified);
        }

        [Test]
        public void ConciseClear()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we clear the arrow body, it should no longer be concise
            body.Clear();
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(emptyBody, minified);
        }

        [Test]
        public void ConciseRemoveLast()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we remove the last statement in the arrow body, it should no longer be concise
            body.RemoveLast();
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(emptyBody, minified);
        }

        [Test]
        public void ConciseRemoveAt()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we remove the first statement in the arrow body, it should no longer be concise
            body.RemoveAt(0);
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(emptyBody, minified);
        }

        [Test]
        public void ConciseReplaceWithNull()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the only statement with null, it should no longer be concise
            body.ReplaceChild(body[0], null);
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(emptyBody, minified);
        }

        [Test]
        public void ConciseReplaceWithNonExpression()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the expression with a non-expression, it should no longer be concise
            body.ReplaceChild(body[0], new DebuggerNode(body[0].Context));
            Assert.IsFalse(body.IsConcise);
            Assert.IsTrue(body.Count == 1);
            Assert.IsTrue(body[0] is DebuggerNode);
            Assert.IsFalse(body[0].IsExpression);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(justDebugger, minified);
        }

        [Test]
        public void ConciseReplaceWithExpression()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the expression with another expression, it should still be concise
            body.ReplaceChild(body[0], GetLookupToFirstParameter(body.Parent as FunctionObject));
            Assert.IsTrue(body.IsConcise);
            Assert.IsTrue(body.Count == 1);
            Assert.IsTrue(body[0] is LookupExpression);
            Assert.IsTrue(body[0].IsExpression);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.AreEqual(justLookup, minified);
        }

        private BlockStatement GetParsedArrowFunctionCode(CodeSettings settings)
        {
            var parser = new JSParser();
            return parser.Parse(source, settings);
        }

        private BlockStatement GetArrowFunctionBody(BlockStatement block)
        {
            // there should be a block, containing a var, containing a vardecl, which has an initializer that's a FunctionObject,
            // that is an arrow funtion with a body that is concise.
            var arrowFunction = (block[0] as VarDeclaration).IfNotNull(v => v[0].Initializer as FunctionObject);
            Assert.IsNotNull(arrowFunction);
            Assert.IsTrue(arrowFunction.FunctionType == FunctionType.ArrowFunction);
            Assert.IsTrue(arrowFunction.Body.Count == 1);
            Assert.IsFalse(arrowFunction.Body[0] is ReturnStatement);
            Assert.IsTrue(arrowFunction.Body[0].IsExpression);
            Assert.IsTrue(arrowFunction.Body.IsConcise);

            return arrowFunction.Body;
        }

        private LookupExpression GetLookupToFirstParameter(FunctionObject function)
        {
            var firstParameter = function.ParameterDeclarations[0] as ParameterDeclaration;
            var bindingIdentifier = firstParameter.Binding as BindingIdentifier;
            return new LookupExpression(function.Body.Context.FlattenToStart())
                {
                    Name = bindingIdentifier.Name,
                    VariableField = bindingIdentifier.VariableField
                };
        }
    }
}

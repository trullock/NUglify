using NUglify.Helpers;
using NUglify.JavaScript;
using NUglify.JavaScript.Syntax;
using NUglify.JavaScript.Visitors;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for ArrowConcise
    /// </summary>
    [TestFixture]
    public class ArrowConcise
    {
	    const string source = "var arrow = (a, b, c) => a * ( b + c);";
	    const string appendDebugger = "var arrow=(n,t,i)=>{return n*(t+i);debugger}";
	    const string insertDebugger = "var arrow=(n,t,i)=>{debugger;return n*(t+i)}";
	    const string insertTwo = "var arrow=(n,t,i)=>{n;debugger;return n*(t+i)}";
	    const string emptyBody = "var arrow=(n,t,i)=>{}";
	    const string justDebugger = "var arrow=(n,t,i)=>{debugger}";
	    const string justLookup = "var arrow=(n,t,i)=>n";

        [Test]
        public void ConciseAppend()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we append something to the arrow body, it should no longer be concise
            // and the first statement should be a return node.
            body.Append(new DebuggerNode(body[0].Context.FlattenToEnd()));
            Assert.That(!body.IsConcise);
            Assert.That(body[0] is ReturnStatement);
            Assert.That(body[1] is DebuggerNode);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(appendDebugger));
        }

        [Test]
        public void ConciseInsertAfter()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert a debugger statement after the first statement, it should no longer be concise
            body.InsertAfter(body[0], new DebuggerNode(body[0].Context.FlattenToEnd()));
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 2);
            Assert.That(body[0] is ReturnStatement);
            Assert.That(body[1] is DebuggerNode);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(appendDebugger));
        }

        [Test]
        public void ConciseInsert()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert a debugger statement before the first statement, it should no longer be concise
            body.Insert(0, new DebuggerNode(body[0].Context.FlattenToStart()));
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 2);
            Assert.That(body[0] is DebuggerNode);
            Assert.That(body[1] is ReturnStatement);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(insertDebugger));
        }

        [Test]
        public void ConciseInsertRange()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we insert two statements before the first statement, it should no longer be concise
            body.InsertRange(0, new AstNode[] { GetLookupToFirstParameter(body.Parent as FunctionObject), new DebuggerNode(body[0].Context.FlattenToStart()) });
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 3);
            Assert.That(body[0] is LookupExpression);
            Assert.That(body[1] is DebuggerNode);
            Assert.That(body[2] is ReturnStatement);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(insertTwo));
        }

        [Test]
        public void ConciseClear()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we clear the arrow body, it should no longer be concise
            body.Clear();
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(emptyBody));
        }

        [Test]
        public void ConciseRemoveLast()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we remove the last statement in the arrow body, it should no longer be concise
            body.RemoveLast();
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(emptyBody));
        }

        [Test]
        public void ConciseRemoveAt()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we remove the first statement in the arrow body, it should no longer be concise
            body.RemoveAt(0);
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(emptyBody));
        }

        [Test]
        public void ConciseReplaceWithNull()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the only statement with null, it should no longer be concise
            body.ReplaceChild(body[0], null);
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 0);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(emptyBody));
        }

        [Test]
        public void ConciseReplaceWithNonExpression()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the expression with a non-expression, it should no longer be concise
            body.ReplaceChild(body[0], new DebuggerNode(body[0].Context));
            Assert.That(!body.IsConcise);
            Assert.That(body.Count == 1);
            Assert.That(body[0] is DebuggerNode);
            Assert.That(!body[0].IsExpression);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(justDebugger));
        }

        [Test]
        public void ConciseReplaceWithExpression()
        {
            var settings = new CodeSettings();
            var code = GetParsedArrowFunctionCode(settings);
            var body = GetArrowFunctionBody(code);

            // if we replace the expression with another expression, it should still be concise
            body.ReplaceChild(body[0], GetLookupToFirstParameter(body.Parent as FunctionObject));
            Assert.That(body.IsConcise);
            Assert.That(body.Count == 1);
            Assert.That(body[0] is LookupExpression);
            Assert.That(body[0].IsExpression);

            // make sure output is what we expect
            var minified = OutputVisitor.Apply(code, settings);
            Assert.That(minified, Is.EqualTo(justLookup));
        }

        BlockStatement GetParsedArrowFunctionCode(CodeSettings settings)
        {
            var parser = new JSParser();
            return parser.Parse(source, settings);
        }

        BlockStatement GetArrowFunctionBody(BlockStatement block)
        {
            // there should be a block, containing a var, containing a vardecl, which has an initializer that's a FunctionObject,
            // that is an arrow funtion with a body that is concise.
            var arrowFunction = (block[0] as VarDeclaration).IfNotNull(v => v[0].Initializer as FunctionObject);
            Assert.That(arrowFunction, Is.Not.Null);
            Assert.That(arrowFunction.FunctionType == FunctionType.ArrowFunction);
            Assert.That(arrowFunction.Body.Count == 1);
            Assert.That(!(arrowFunction.Body[0] is ReturnStatement));
            Assert.That(arrowFunction.Body[0].IsExpression);
            Assert.That(arrowFunction.Body.IsConcise);

            return arrowFunction.Body;
        }

        LookupExpression GetLookupToFirstParameter(FunctionObject function)
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

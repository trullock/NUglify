using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
    [TestFixture]
    public class ES2015
    {
        [Test]
        public void ConstDeclaration()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BlockScopeVariables()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BlockScopeFunctions()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ExpressionBodies()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ExpressionBodiesWithDestructuring()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void StatementBodies()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DefaultParameters()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void RestParameters()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void SpreadOperator()
        {
            TestHelper.Instance.RunTest("-rename:all");
        }

        [Test]
        public void StringInterpolation()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void TaggedTemplates()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void MethodProperties()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ArrayMatching()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ObjectMatching()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ParameterContextMatching()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void DestructuringAssignment()
        {
            TestHelper.Instance.RunTest();
        }

        //[Test]
        public void Modules()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ClassDefinition()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ClassInheritance()
        {
            TestHelper.Instance.RunTest();
        }

        //[Test]
        public void ClassInheritanceAssignment()
        {
            TestHelper.Instance.RunTest();
        }

        //[Test]
        public void ClassInheritanceExpressions()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void BaseClassAccessor()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void StaticMembers()
        {
            TestHelper.Instance.RunTest();
        }

        //[Test]
        public void IteratorsForEach()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void GeneratorFunction()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void GeneratorFunctionDirect()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void GeneratorMatching()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void GeneratorControlFlow()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void GeneratorMethods()
        {
            TestHelper.Instance.RunTest();
        }

        //[Test]
        public void Proxying()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ObjectSpreadOperator()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ConciseMethods()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void ConciseProperties()
        {
            TestHelper.Instance.RunTest();
        }
    }
}

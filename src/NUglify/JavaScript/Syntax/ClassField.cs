using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public class ClassField : AstNode
    {
	    BindingIdentifier binding;
	    
        public bool IsStatic { get; set; }

        public SourceContext StaticContext { get; set; }

        public BindingIdentifier Binding
        {
            get => binding;
            set => ReplaceNode(ref binding, value);
        }
        public string Name { get; set; }
        public ArrayLiteral ComputedName { get; set; }

        public override bool IsDeclaration => false;

        public ClassField(SourceContext functionContext)
            : base(functionContext)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor != null)
	            visitor.Visit(this);
        }

        public bool IsReferenced => true;
        public AstNode Initializer { get; set; }
    }
}
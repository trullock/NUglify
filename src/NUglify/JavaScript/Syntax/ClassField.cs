using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public class ClassField : AstNode
    {
	    BindingIdentifier m_binding;
	    AstNodeList m_parameters;
	    BlockStatement m_body;

        public bool IsStatic { get; set; }

        public SourceContext StaticContext { get; set; }

        public BindingIdentifier Binding
        {
            get => m_binding;
            set => ReplaceNode(ref m_binding, value);
        }
        public string Name { get; set; }

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
    }
}
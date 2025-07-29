using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUglify.JavaScript.Syntax
{
    public class ImportMetaCustomNode : CustomNode
    {
        public ImportMetaCustomNode(SourceContext context)
            : base(context) { }

        public override string ToCode()
            => $"import.{Context.Code}";
    }
}

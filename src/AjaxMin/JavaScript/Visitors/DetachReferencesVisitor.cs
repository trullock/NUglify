// DetachReferencesVisitor.cs
//
// Copyright 2012 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUglify.JavaScript.Syntax;

namespace NUglify.JavaScript.Visitors
{
    public class DetachReferencesVisitor : TreeVisitor
    {
        // singleton
        private static readonly DetachReferencesVisitor s_instance = new DetachReferencesVisitor();

        private DetachReferencesVisitor()
        {
        }

        public static void Apply(AstNode node)
        {
            if (node != null)
            {
                node.Accept(s_instance); 
            }
        }

        public static void Apply(params AstNode[] nodes)
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Accept(s_instance);
                }
            }
        }

        public override void Visit(LookupExpression node)
        {
            // only lookups need to be detached.
            if (node != null)
            {
                // if the node's vairable field is set, remove it from the
                // field's list of references.
                var variableField = node.VariableField;
                if (variableField != null)
                {
                    variableField.References.Remove(node);
                }
            }
        }
    }
}

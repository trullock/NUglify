// call.cs
//
// Copyright 2010 Microsoft Corporation
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

using System;
using System.Collections.Generic;
using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public sealed class CallExpression : Expression
    {
        AstNode m_function;
        AstNodeList m_arguments;

        public AstNode Function
        {
            get => m_function;
            set => ReplaceNode(ref m_function, value);
        }

        public AstNodeList Arguments
        {
            get => m_arguments;
            set => ReplaceNode(ref m_arguments, value);
        }

        public bool IsConstructor { get; set; }
        public bool InBrackets { get; set; }
        public bool OptionalChaining { get; set; }
        public bool IsTaggedTemplateLiteral { get; set; }


        public CallExpression(SourceContext context)
            : base(context)
        {
        }

        public override OperatorPrecedence Precedence =>
            // new-operator is the unary precedence; () and [] are  field access
            /*IsConstructor ? OperatorPrecedence.Unary :*/OperatorPrecedence.FieldAccess;

        public override bool IsExpression
        {
            get
            {
                // normally this would be an expression. BUT we want to check for a
                // call to a member function that is in the "onXXXX" pattern and passing
                // parameters. This is because of a bug in IE that will throw a script error 
                // if you call a native event handler like onclick and pass in a parameter
                // IN A LOGICAL EXPRESSION. For some reason, the simple statement:
                // elem.onclick(e) will work, but elem&&elem.onclick(e) will not. So treat
                // calls to any member operator where the property name starts with "on" and
                // we are passing in arguments as if it were NOT an expression, and it won't
                // get combined.
                var callMember = Function as MemberExpression;
                if (callMember != null
                    && callMember.Name.StartsWith("on", StringComparison.Ordinal)
                    && Arguments.Count > 0)
                    // popped positive -- don't treat it like an expression.
                    return false;

                // otherwise it's okay -- it's an expression and can be combined.
                return true;
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor?.Visit(this);
        }

        public override IEnumerable<AstNode> Children => EnumerateNonNullNodes(Function, Arguments);


        public override bool ReplaceChild(AstNode oldNode, AstNode newNode)
        {
            if (Function == oldNode)
            {
                Function = newNode;
                return true;
            }

            if (Arguments == oldNode)
            {
                if (newNode == null)
                {
                    // remove it
                    Arguments = null;
                    return true;
                }
                else
                {
                    // if the new node isn't an AstNodeList, ignore it
                    var newList = newNode as AstNodeList;
                    if (newList != null)
                    {
                        Arguments = newList;
                        return true;
                    }
                }
            }

            return false;
        }

        // the function is on the left
        public override AstNode LeftHandSide =>  Function.LeftHandSide;

        public override bool IsEquivalentTo(AstNode otherNode)
        {
            // a call node is equivalent to another call node if the function and the arguments
            // are all equivalent (and be sure to check for brackets and constructor)
            var otherCall = otherNode as CallExpression;
            return otherCall != null
                   && InBrackets == otherCall.InBrackets
                   && IsConstructor == otherCall.IsConstructor
                   && OptionalChaining == otherCall.OptionalChaining
                   && Function.IsEquivalentTo(otherCall.Function)
                   && Arguments.IsEquivalentTo(otherCall.Arguments);
        }
    }
}
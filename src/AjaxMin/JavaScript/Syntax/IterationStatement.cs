// IterationStatement.cs
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

namespace NUglify.JavaScript.Syntax
{
    public abstract class IterationStatement : Statement
    {
        private BlockStatement m_body;

        public BlockStatement Body
        {
            get { return m_body; }
            set
            {
                ReplaceNode<BlockStatement>(ref m_body, value);
            }
        }

        protected IterationStatement(SourceContext context)
            : base(context)
        {
        }
    }
}

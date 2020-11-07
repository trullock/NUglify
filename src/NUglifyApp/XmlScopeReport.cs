// DefaultScopeReport.cs
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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using NUglify.Helpers;
using NUglify.JavaScript;
using NUglify.JavaScript.Syntax;
using NUglify.JavaScript.Visitors;

namespace NUglify
{
    public sealed class XmlScopeReport : IScopeReport
    {
	    XmlWriter m_writer;
	    bool m_useReferenceCounts;

        #region IScopeReport Members

        public string Name
        {
            get { return "Xml"; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="lower-case by design")]
        public void CreateReport(TextWriter writer, GlobalScope globalScope, bool useReferenceCounts)
        {
            if (globalScope != null)
            {
                m_useReferenceCounts = useReferenceCounts;

                // start the global scope
                m_writer = XmlWriter.Create(writer, new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true });
                m_writer.WriteStartElement("global");

                // recursively process each child scope
                foreach (var childScope in globalScope.ChildScopes)
                {
                    ProcessScope(childScope);
                }

                // process any undefined references
                if (globalScope.UndefinedReferences != null && globalScope.UndefinedReferences.Count > 0)
                {
                    m_writer.WriteStartElement("undefined");

                    foreach (var undefined in globalScope.UndefinedReferences)
                    {
                        m_writer.WriteStartElement("reference");
                        m_writer.WriteAttributeString("name", undefined.Name);
                        m_writer.WriteAttributeString("type", undefined.ReferenceType.ToString().ToLowerInvariant());
                        if (undefined.LookupNode != null && undefined.LookupNode.Context != null)
                        {
                            OutputContextPosition(undefined.LookupNode.Context);
                        }
                        else
                        {
                            m_writer.WriteAttributeString("srcLine", undefined.Line.ToStringInvariant());
                            m_writer.WriteAttributeString("srcCol", (undefined.Column + 1).ToStringInvariant());
                        }

                        m_writer.WriteEndElement();
                    }

                    m_writer.WriteEndElement();
                }

                m_writer.WriteEndElement();
                m_writer.Flush();
                m_writer = null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_writer != null)
            {
                m_writer.Flush();
                m_writer = null;
            }
        }

        #endregion

        #region private methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "lower-case by design")]
        void ProcessScope(ActivationObject scope)
        {
            switch (scope.ScopeType)
            {
                case ScopeType.Block:
                case ScopeType.Lexical:
                case ScopeType.None:
                    // must be generic block scope
                    m_writer.WriteStartElement("block");
                    if (scope.UseStrict)
                    {
                        m_writer.WriteAttributeString("strict", "true");
                    }
                    break;

                case ScopeType.Class:
                    m_writer.WriteStartElement("class");
                    if (!scope.ScopeName.IsNullOrWhiteSpace())
                    {
                        m_writer.WriteAttributeString("src", scope.ScopeName);
                    }

                    if (scope.UseStrict)
                    {
                        m_writer.WriteAttributeString("strict", "true");
                    }
                    break;

                case ScopeType.Catch:
                    var catchScope = (CatchScope)scope;
                    m_writer.WriteStartElement("catch");
                    if (scope.UseStrict)
                    {
                        m_writer.WriteAttributeString("strict", "true");
                    }

                    foreach (var bindingIdentifier in BindingsVisitor.Bindings(catchScope.CatchParameter))
                    {
                        m_writer.WriteStartElement("catchvar");
                        m_writer.WriteAttributeString("src", bindingIdentifier.Name);

                        OutputContextPosition(bindingIdentifier.Context);

                        var catchVariable = bindingIdentifier.VariableField;
                        if (catchVariable != null)
                        {
                            if (catchVariable.CrunchedName != null)
                            {
                                m_writer.WriteAttributeString("min", catchVariable.CrunchedName);
                            }

                            if (m_useReferenceCounts)
                            {
                                m_writer.WriteAttributeString("refcount", catchVariable.RefCount.ToStringInvariant());
                            }
                        }

                        m_writer.WriteEndElement();
                    }
                    break;

                case ScopeType.Module:
                    m_writer.WriteStartElement("module");
                    if (!scope.ScopeName.IsNullOrWhiteSpace())
                    {
                        m_writer.WriteAttributeString("name", scope.ScopeName);
                    }
                    
                    if (scope.UseStrict)
                    {
                        m_writer.WriteAttributeString("strict", "true");
                    }

                    (scope as ModuleScope).IfNotNull(m =>
                        {
                            m_writer.WriteAttributeString("default", m.HasDefaultExport ? "true" : "false");
                            if (m.IsNotComplete)
                            {
                                m_writer.WriteAttributeString("incomplete", "true");
                            }
                        });
                    break;

                case ScopeType.Function:
                    var functionScope = (FunctionScope)scope;
                    m_writer.WriteStartElement("function");

                    // for source name, use the scope name
                    if (!scope.ScopeName.IsNullOrWhiteSpace())
                    {
                        m_writer.WriteAttributeString("src", scope.ScopeName);
                    }

                    var functionObject = functionScope.Owner as FunctionObject;
                    if (functionObject != null)
                    {
                        if (functionObject.Binding == null || functionObject.Binding.Name.IsNullOrWhiteSpace())
                        {
                            if (!functionObject.NameGuess.IsNullOrWhiteSpace())
                            {
                                // strip enclosing quotes
                                m_writer.WriteAttributeString("guess", functionObject.NameGuess.Trim('\"'));
                            }
                        }
                        else
                        {
                            if (functionObject.Binding.VariableField != null
                                && functionObject.Binding.VariableField.CrunchedName != null)
                            {
                                m_writer.WriteAttributeString("min", functionObject.Binding.VariableField.CrunchedName);
                            }
                        }

                        m_writer.WriteAttributeString("type", functionObject.FunctionType.ToString().ToLowerInvariant());
                        OutputContextPosition(functionObject.Context);

                        if (m_useReferenceCounts
                            && functionObject.Binding != null
                            && functionObject.Binding.VariableField != null)
                        {
                            var refCount = functionObject.Binding.VariableField.RefCount;
                            m_writer.WriteAttributeString("refcount", refCount.ToStringInvariant());

                            if (refCount == 0
                                && functionObject.FunctionType == FunctionType.Declaration
                                && functionObject.Binding.VariableField.FieldType == FieldType.Local)
                            {
                                // local function declaration with zero references? unreachable code!
                                m_writer.WriteAttributeString("unreachable", "true");
                            }
                        }

                        if (scope.UseStrict)
                        {
                            m_writer.WriteAttributeString("strict", "true");
                        }

                        // add the arguments
                        m_writer.WriteStartElement("arguments");
                        if (functionObject.ParameterDeclarations != null)
                        {
                            foreach (var bindingIdentifier in BindingsVisitor.Bindings(functionObject.ParameterDeclarations))
                            {
                                m_writer.WriteStartElement("argument");

                                m_writer.WriteAttributeString("src", bindingIdentifier.Name);
                                if (bindingIdentifier.VariableField.IfNotNull(v => v.CrunchedName != null))
                                {
                                    m_writer.WriteAttributeString("min", bindingIdentifier.VariableField.CrunchedName);
                                }

                                OutputContextPosition(bindingIdentifier.Context);
                                if (m_useReferenceCounts)
                                {
                                    bindingIdentifier.VariableField.IfNotNull(v => m_writer.WriteAttributeString("refcount", v.RefCount.ToStringInvariant()));
                                }

                                m_writer.WriteEndElement();
                            }
                        }

                        m_writer.WriteEndElement();
                    }
                    break;

                case ScopeType.Global:
                    Debug.Assert(scope is GlobalScope);
                    Debug.Fail("shouldn't get here!");
                    m_writer.WriteStartElement("global");
                    break;

                case ScopeType.With:
                    Debug.Assert(scope is WithScope);
                    m_writer.WriteStartElement("with");

                    // with-scopes should never be strict because the with-statement is not allowed in strict code
                    if (scope.UseStrict)
                    {
                        m_writer.WriteAttributeString("strict", "true");
                    }
                    break;
            }

            // process the defined and referenced fields
            ProcessFields(scope);

            // recursively process each child scope
            foreach (var childScope in scope.ChildScopes)
            {
                ProcessScope(childScope);
            }

            // close the element
            m_writer.WriteEndElement();
        }

        void ProcessFields(ActivationObject scope)
        {
            // split fields into defined and referenced lists
            var definedFields = new List<JSVariableField>();
            var referencedFields = new List<JSVariableField>();
            foreach (var field in scope.NameTable.Values)
            {
                // if the field has no outer field reference, it is defined in this scope.
                // otherwise we're just referencing a field defined elsewhere
                if (!field.IsOuterReference)
                {
                    switch (field.FieldType)
                    {
                        case FieldType.Global:
                            if (scope is GlobalScope)
                            {
                                definedFields.Add(field);
                            }
                            else
                            {
                                referencedFields.Add(field);
                            }
                            break;

                        case FieldType.Local:
                            // defined within this scope
                            definedFields.Add(field);
                            break;

                        case FieldType.Argument:
                            // ignore the scope's arguments because we handle them separately
                            break;

                        case FieldType.CatchError:
                            // ignore the catch-scope's error parameter because we handle it separately
                            break;

                        case FieldType.Arguments:
                            if (field.RefCount > 0)
                            {
                                referencedFields.Add(field);
                            }
                            break;

                        case FieldType.Super:
                            referencedFields.Add(field);
                            break;

                        case FieldType.UndefinedGlobal:
                        case FieldType.Predefined:
                        case FieldType.WithField:
                            referencedFields.Add(field);
                            break;

                        case FieldType.GhostFunction:
                        case FieldType.GhostCatch:
                            // ignore the ghost fields when reporting
                            break;
                    }
                }
                else if (!field.IsPlaceholder)
                {
                    // we are an outer reference and we are not a placeholder,
                    // so this scope actually references the outer field.
                    referencedFields.Add(field);
                }
            }

            if (definedFields.Count > 0)
            {
                m_writer.WriteStartElement("defines");
                foreach (var field in definedFields)
                {
                    ProcessField(field, true);
                }

                m_writer.WriteEndElement();
            }

            if (referencedFields.Count > 0)
            {
                m_writer.WriteStartElement("references");
                foreach (var field in referencedFields)
                {
                    ProcessField(field, false);
                }

                m_writer.WriteEndElement();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="lower-case by design")]
        void ProcessField(JSVariableField field, bool isDefined)
        {
            // save THIS field's refcount value because we will
            // be adjusting hte field pointer to be the outermost field
            // and we want to report THIS field's refcount, not the overall.
            var refCount = field.RefCount;
            var isGhost = false;

            // make sure we're at the outer-most field
            var isOuter = false;
            if (!isDefined)
            {
                while (field.OuterField != null)
                {
                    isOuter = true;
                    field = field.OuterField;
                }
            }

            m_writer.WriteStartElement("field");

            if (field.IsExported)
            {
                m_writer.WriteAttributeString("exported", "true");    
            }

            var typeValue = field.FieldType.ToString();
            switch (field.FieldType)
            {
                case FieldType.Argument:
                case FieldType.CatchError:
                case FieldType.WithField:
                    if (isOuter)
                    {
                        typeValue = "Outer " + typeValue;
                    }
                    break;

                case FieldType.Local:
                    if (isOuter)
                    {
                        typeValue = "Outer ";
                    }
                    else
                    {
                        typeValue = string.Empty;
                        if (field.IsPlaceholder || !field.IsDeclared)
                        {
                            isGhost = true;
                        }
                    }

                    if (field.IsFunction)
                    {
                        typeValue += "Function";
                    }
                    else
                    {
                        typeValue += "Variable";
                    }
                    break;

                case FieldType.Arguments:
                case FieldType.Global:
                case FieldType.UndefinedGlobal:
                case FieldType.GhostCatch:
                case FieldType.GhostFunction:
                case FieldType.Predefined:
                case FieldType.Super:
                    break;
            }

            m_writer.WriteAttributeString("type", typeValue.ToLowerInvariant());
            m_writer.WriteAttributeString("src", field.Name);
            if (field.CrunchedName != null)
            {
                m_writer.WriteAttributeString("min", field.CrunchedName);
            }

            OutputContextPosition(field.OriginalContext);

            if (m_useReferenceCounts)
            {
                m_writer.WriteAttributeString("refcount", refCount.ToStringInvariant());
            }

            if (field.IsAmbiguous)
            {
                m_writer.WriteAttributeString("ambiguous", "true");
            }

            if (field.IsGenerated)
            {
                m_writer.WriteAttributeString("generated", "true");
            }

            if (isGhost)
            {
                m_writer.WriteAttributeString("ghost", "true");
            }

            m_writer.WriteEndElement();
        }

        #endregion

        void OutputContextPosition(SourceContext context)
        {
            if (context != null)
            {
                m_writer.WriteAttributeString("srcLine", context.StartLineNumber.ToStringInvariant());
                m_writer.WriteAttributeString("srcCol", (context.StartColumn + 1).ToStringInvariant());
                if (context.OutputLine > 0)
                {
                    m_writer.WriteAttributeString("dstLine", context.OutputLine.ToStringInvariant());
                    m_writer.WriteAttributeString("dstCol", (context.OutputColumn + 1).ToStringInvariant());
                }
            }
        }
    }
}

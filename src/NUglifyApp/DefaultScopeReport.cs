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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUglify.Helpers;
using NUglify.JavaScript;
using NUglify.JavaScript.Syntax;

namespace NUglify
{
    public sealed class DefaultScopeReport : IScopeReport
    {
        #region fields

        TextWriter m_writer;
        bool m_useReferenceCounts;

        #endregion

        #region IScopeReport Members

        public string Name
        {
            get { return "Default"; }
        }

        public void CreateReport(TextWriter writer, GlobalScope globalScope, bool useReferenceCounts)
        {
            if (writer != null && globalScope != null)
            {
                m_writer = writer;
                m_useReferenceCounts = useReferenceCounts;

                // output global scope report
                WriteScopeReport(globalScope);

                // generate a flat array of function scopes ordered by context line start
                var scopes = GetAllFunctionScopes(globalScope);

                // for each function scope, output a scope report
                foreach (ActivationObject scope in scopes)
                {
                    WriteScopeReport(scope);
                }

                // write the unreferenced global report
                WriteUnrefedReport(globalScope);
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

        ActivationObject[] GetAllFunctionScopes(GlobalScope globalScope)
        {
            // create a list to hold all the scopes
            List<ActivationObject> scopes = new List<ActivationObject>();

            // recursively add all the function scopes to the list
            AddScopes(scopes, globalScope);

            // sort the scopes by starting line (from the context)
            scopes.Sort(ScopeComparer.Instance);

            // return as an array
            return scopes.ToArray();
        }

        void AddScopes(List<ActivationObject> list, ActivationObject parentScope)
        {
            // for each child scope...
            foreach (ActivationObject scope in parentScope.ChildScopes)
            {
                // add the scope to the list if it's not a globalscopes
                if (!(scope is GlobalScope))
                {
                    list.Add(scope);
                }

                // recurse...
                AddScopes(list, scope);
            }
        }

        void WriteScopeReport(ActivationObject scope)
        {
            // output the function header
            if (scope is GlobalScope)
            {
                WriteProgress(NUglify.GlobalObjectsHeader);
            }
            else
            {
                ModuleScope moduleScope;
                if (scope is FunctionScope)
                {
                    WriteFunctionHeader(scope.Owner as FunctionObject, scope.IsKnownAtCompileTime, scope.UseStrict);
                }
                else if ((moduleScope = scope as ModuleScope) != null)
                {
                    WriteModuleHeader(moduleScope);
                }
                else
                {
                    string blockType;
                    switch (scope.ScopeType)
                    {
                        case ScopeType.Catch:
                            blockType = NUglify.BlockTypeCatch;
                            break;

                        case ScopeType.With:
                            blockType = NUglify.BlockTypeWith;
                            break;

                        case ScopeType.Class:
                            blockType = NUglify.BlockTypeClass.FormatInvariant(scope.ScopeName.IfNullOrWhiteSpace(NUglify.AnonymousName));
                            break;

                        case ScopeType.Block:
                        case ScopeType.Lexical:
                            blockType = NUglify.BlockTypeLexical;
                            break;

                        case ScopeType.Module:
                        case ScopeType.Function:
                        case ScopeType.Global:
                        case ScopeType.None:
                        default:
                            blockType = string.Empty;
                            System.Diagnostics.Debug.Fail("shouldn't get here");
                            break;
                    }

                    WriteBlockHeader(scope as BlockScope, blockType);
                }
            }

            // get all the fields in the scope
            List<JSVariableField> scopeFields = new List<JSVariableField>(scope.NameTable.Values);

            // sort the fields
            scopeFields.Sort(FieldComparer.Instance);

            // iterate over all the fields
            foreach (JSVariableField variableField in scopeFields)
            {
                // don't report placeholder fields or fields with the SpecialName attribute that aren't referenced
                if (!variableField.IsPlaceholder
                    && (variableField.Attributes != FieldAttributes.SpecialName || variableField.IsReferenced))
                {
                    WriteMemberReport(variableField);
                }
            }
        }

        void WriteModuleHeader(ModuleScope moduleScope)
        {
            var blockType = NUglify.BlockTypeModule.FormatInvariant(moduleScope.ScopeName.IfNullOrWhiteSpace(NUglify.ModuleNameImplicit));
            string scopeFlags = null;
            var sb = StringBuilderPool.Acquire();
            try
            {
                if (!moduleScope.IsKnownAtCompileTime)
                {
                    sb.Append('[');
                    sb.Append(NUglify.NotKnown);
                    sb.Append(']');
                }

                if (moduleScope.UseStrict)
                {
                    sb.Append(NUglify.ScopeIsStrictFlag);
                }

                if (moduleScope.IsNotComplete)
                {
                    sb.Append(NUglify.ModuleIncompleteFlag);
                }

                scopeFlags = sb.ToString();
            }
            finally
            {
                sb.Release();
            }

            WriteProgress();
            WriteProgress(NUglify.BlockScopeHeader.FormatInvariant(
              blockType,
              moduleScope.Owner.Context.StartLineNumber,
              moduleScope.Owner.Context.StartColumn + 1,
              scopeFlags));

            if (moduleScope.HasDefaultExport)
            {
                // when there's a default export, we want to flag a line under the module
                // header that indicates that it's okay to bind to the default export.
                WriteProgress(NUglify.ModuleHasDefaultExport);
            }
        }

        void WriteBlockHeader(BlockScope blockScope, string blockType)
        {
            string scopeFlags = null;
            var sb = StringBuilderPool.Acquire();
            try
            {
                if (!blockScope.IsKnownAtCompileTime)
                {
                    sb.Append('[');
                    sb.Append(NUglify.NotKnown);
                    sb.Append(']');
                }

                if (blockScope.UseStrict)
                {
                    sb.Append(NUglify.ScopeIsStrictFlag);
                }

                scopeFlags = sb.ToString();
            }
            finally
            {
                sb.Release();
            }

            WriteProgress();
            WriteProgress(NUglify.BlockScopeHeader.FormatInvariant(
              blockType,
              blockScope.Owner.Context.StartLineNumber,
              blockScope.Owner.Context.StartColumn + 1,
              scopeFlags));
        }

        //TYPE "NAME" - Starts at line LINE, col COLUMN STATUS [crunched to CRUNCH]
        //
        //TYPE: Function, Function getter, Function setter
        //STATUS: '', Unknown, Unreachable
        void WriteFunctionHeader(FunctionObject funcObj, bool isKnown, bool useStrict)
        {
            // get the crunched value (if any)
            string crunched = string.Empty;
            var functionField = funcObj.Binding.IfNotNull(b => b.VariableField);
            if (functionField != null && functionField.CrunchedName != null)
            {
                crunched = NUglify.CrunchedTo.FormatInvariant(functionField.CrunchedName, functionField.RefCount);
            }

            // get the status if the function
            string status = null;
            var statusBuilder = StringBuilderPool.Acquire();
            try
            {
                if (!isKnown)
                {
                    statusBuilder.Append('[');
                    statusBuilder.Append(NUglify.NotKnown);
                }
                if (funcObj.EnclosingScope.Parent is GlobalScope)
                {
                    // global function.
                    // if this is a named function expression, we still want to know if it's
                    // referenced by anyone
                    if (funcObj.FunctionType == FunctionType.Expression
                        && funcObj.Binding != null
                        && !funcObj.Binding.Name.IsNullOrWhiteSpace())
                    {
                        // output a comma separator if not the first item, otherwise 
                        // open the square bracket
                        if (statusBuilder.Length > 0)
                        {
                            statusBuilder.Append(", ");
                        }
                        else
                        {
                            statusBuilder.Append('[');
                        }
                        statusBuilder.Append(NUglify.FunctionInfoReferences.FormatInvariant(
                            funcObj.Binding.VariableField.IfNotNull(v => v.RefCount)
                            ));
                    }
                }
                else if (!funcObj.IsReferenced && m_useReferenceCounts)
                {
                    // local function that isn't referenced -- unreachable!
                    // output a comma separator if not the first item, otherwise 
                    // open the square bracket
                    if (statusBuilder.Length > 0)
                    {
                        statusBuilder.Append(", ");
                    }
                    else
                    {
                        statusBuilder.Append('[');
                    }

                    statusBuilder.Append(NUglify.Unreachable);
                }

                if (statusBuilder.Length > 0)
                {
                    statusBuilder.Append(']');
                }

                if (useStrict)
                {
                    statusBuilder.Append(NUglify.ScopeIsStrictFlag);
                }

                status = statusBuilder.ToString();
            }
            finally
            {
                statusBuilder.Release();
            }

            string functionType;
            switch (funcObj.FunctionType)
            {
                case FunctionType.Getter:
                    functionType = NUglify.FunctionTypePropGet;
                    break;

                case FunctionType.Setter:
                    functionType = NUglify.FunctionTypePropSet;
                    break;

                case FunctionType.Expression:
                    functionType = NUglify.FunctionTypeExpression;
                    break;

                case FunctionType.ArrowFunction:
                    functionType = NUglify.FunctionTypeArrow;
                    break;

                case FunctionType.Method:
                    functionType = NUglify.FunctionTypeMethod;
                    break;

                default:
                    functionType = NUglify.FunctionTypeFunction;
                    break;
            }

            var functionName = funcObj.Binding.IfNotNull(b => b.Name);
            if (functionName.IsNullOrWhiteSpace())
            {
                functionName = !funcObj.NameGuess.IsNullOrWhiteSpace()
                    ? '"' + funcObj.NameGuess + '"'
                    : NUglify.AnonymousName;
            }

            // output
            WriteProgress();
            WriteProgress(NUglify.FunctionHeader.FormatInvariant(
                functionType,
                functionName,
                funcObj.Context.StartLineNumber,
                funcObj.Context.StartColumn + 1,
                status,
                crunched,
                funcObj.IsGenerator ? NUglify.FunctionTypeGenerator : string.Empty));
        }

        // NAME [SCOPE TYPE] [crunched to CRUNCH]
        //
        // SCOPE: global, local, outer, ''
        // TYPE: var, function, argument, arguments array, possibly undefined
        void WriteMemberReport(JSVariableField variableField)
        {
            // don't report arguments fields that aren't referenced because
            // we add those fields to the function scopes automatically
            if (variableField.FieldType != FieldType.Arguments || variableField.RefCount > 0)
            {
                string scope = string.Empty;
                string type = string.Empty;
                string crunched = string.Empty;
                string name = variableField.Name;
                if (variableField.IsLiteral)
                {
                    name = variableField.FieldValue.ToString();
                }

                // calculate the crunched label
                if (variableField.CrunchedName != null)
                {
                    crunched = NUglify.CrunchedTo.FormatInvariant(variableField.CrunchedName, variableField.RefCount);
                }
                else
                {
                    crunched = NUglify.MemberInfoReferences.FormatInvariant(variableField.RefCount);
                }

                // get the field's default scope and type
                GetFieldScopeType(variableField, out scope, out type);
                if (variableField.FieldType == FieldType.WithField)
                {
                    // if the field is a with field, we won't be using the crunched field (since
                    // those fields can't be crunched), so let's overload it with what the field
                    // could POSSIBLY be if the with object doesn't have a property of that name
                    if (variableField.OuterField != null)
                    {
                        // make sure we get the OUTERMOST outer ield
                        var outerField = variableField.OuterField;
                        while (outerField.OuterField != null)
                        {
                            outerField = outerField.OuterField;
                        }

                        string outerScope;
                        string outerType;
                        GetFieldScopeType(outerField, out outerScope, out outerType);
                        if (!outerScope.IsNullOrWhiteSpace() || !outerType.IsNullOrWhiteSpace())
                        {
                            crunched = NUglify.MemberInfoWithPossibly.FormatInvariant(outerScope, outerType);
                        }
                    }
                    else
                    {
                        // no outer field. Then it must be a lexically-declared field inside the with-statement's
                        // block. These are tricky! At run-time if you try to declare a function, const, or let
                        // inside the with-scope, and the with-object already has a property with that name, the
                        // declaration will throw a run-time error.
                        crunched = variableField.IsFunction ? NUglify.MemberInfoWithLexFunc : NUglify.MemberInfoWithLexDecl;
                    }
                }

                var definedLocation = string.Empty;
                var definedContext = GetFirstDeclaration(variableField);
                if (definedContext != null)
                {
                    definedLocation = NUglify.MemberInfoDefinedLocation.FormatInvariant(definedContext.StartLineNumber, definedContext.StartColumn + 1);
                }

                // format the entire string
                WriteProgress(NUglify.MemberInfoFormat.FormatInvariant(
                    name,
                    scope,
                    type,
                    crunched,
                    definedLocation
                    ));
            }
        }

        static SourceContext GetFirstDeclaration(JSVariableField variableField)
        {
            // only local fields that actually correspond to a declaration get the declaration
            // added to the declarations collection -- inner references don't get the declaration,
            // and neither do ghosted variables. So starting from this variable, walk up the 
            // outer-reference chain until we find on with at least one declaration. If we don't
            // find one, that's fine -- there probably isn't a declaration for it (predefined, for example).
            while (variableField != null && variableField.Declarations.Count == 0)
            {
                variableField = variableField.OuterField;
            }

            if (variableField != null)
            {
                foreach (var declaration in variableField.Declarations)
                {
                    // we only care about the FIRST declaration, so return the context of the name
                    return declaration.Context;
                }
            }

            // if we get here, there were no declarations
            return null;
        }

        static void GetFieldScopeType(JSVariableField variableField, out string scope, out string type)
        {
            // default scope is blank
            scope = string.Empty;
            switch (variableField.FieldType)
            {
                case FieldType.Argument:
                    type = NUglify.MemberInfoTypeArgument;
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    break;

                case FieldType.Arguments:
                    type = NUglify.MemberInfoTypeArguments;
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    break;

                case FieldType.CatchError:
                    type = NUglify.MemberInfoTypeCatchEror;
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    break;

                case FieldType.Super:
                    type = NUglify.MemberInfoTypeSuper;
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    break;

                case FieldType.GhostCatch:
                case FieldType.GhostFunction:
                default:
                    // ghost fields -- ignore
                    type = string.Empty;
                    break;

                case FieldType.Global:
                case FieldType.UndefinedGlobal:
                    if ((variableField.Attributes & FieldAttributes.RTSpecialName) == FieldAttributes.RTSpecialName)
                    {
                        // this is a special "global." It might not be a global, but something referenced
                        // in a with scope somewhere down the line.
                        type = NUglify.MemberInfoPossiblyUndefined;
                    }
                    else if (variableField.FieldValue is FunctionObject && variableField.GhostedField != null)
                    {
                        type = NUglify.MemberInfoFunctionExpression;
                    }
                    else
                    {
                        goto case FieldType.Local;
                    }
                    break;

                case FieldType.Local:
                    // type string
                    if (variableField.FieldValue is FunctionObject)
                    {
                        if (variableField.GhostedField == null)
                        {
                            type = NUglify.MemberInfoFunction;
                        }
                        else
                        {
                            type = NUglify.MemberInfoFunctionExpression;
                        }
                    }
                    else if (variableField.FieldValue is ClassNode)
                    {
                        type = NUglify.MemberInfoClass;
                    }
                    else if (variableField.IsLiteral)
                    {
                        type = NUglify.MemberInfoLiteral;
                    }
                    else if (variableField.InitializationOnly)
                    {
                        type = NUglify.MemberInfoConst;
                    }
                    else
                    {
                        type = NUglify.MemberInfoVar;
                    }

                    // scope string
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    else if (variableField.FieldType == FieldType.Global || variableField.FieldType == FieldType.UndefinedGlobal)
                    {
                        scope = NUglify.MemberInfoScopeGlobal;
                    }
                    else
                    {
                        scope = NUglify.MemberInfoScopeLocal;
                    }
                    break;

                case FieldType.Predefined:
                    scope = NUglify.MemberInfoScopeGlobalObject;
                    type = variableField.IsFunction
                        ? NUglify.MemberInfoBuiltInMethod
                        : NUglify.MemberInfoBuiltInProperty;
                    break;

                case FieldType.WithField:
                    type = NUglify.MemberInfoWithField;
                    if (variableField.IsOuterReference)
                    {
                        scope = NUglify.MemberInfoScopeOuter;
                    }
                    break;
            }

            if (variableField.IsExported)
            {
                scope = NUglify.MemberInfoScopeExported + scope;
            }
        }

        void WriteUnrefedReport(GlobalScope globalScope)
        {
            if (globalScope.UndefinedReferences != null && globalScope.UndefinedReferences.Count > 0)
            {
                // sort the undefined reference exceptions
                var undefinedList = new List<UndefinedReference>(globalScope.UndefinedReferences);
                undefinedList.Sort((left, right) =>
                    {
                        // first do the right thing if one or both are null
                        if (left == null && right == null)
                        {
                            // both null -- equal
                            return 0;
                        }

                        if (left == null)
                        {
                            // left is null, right is not -- left is less
                            return -1;
                        }

                        if (right == null)
                        {
                            // left is not null, right is -- left is more
                            return 1;
                        }

                        // neither are null
                        int comparison = string.Compare(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
                        if (comparison == 0)
                        {
                            comparison = left.Line - right.Line;
                            if (comparison == 0)
                            {
                                comparison = left.Column - right.Column;
                            }
                        }

                        return comparison;
                    });

                // write the report
                WriteProgress();
                WriteProgress(NUglify.UndefinedGlobalHeader);
                foreach (UndefinedReference ex in undefinedList)
                {
                    WriteProgress(NUglify.UndefinedInfo.FormatInvariant(
                      ex.Name,
                      ex.Line,
                      ex.Column,
                      ex.ReferenceType.ToString()
                      ));
                }
            }
        }

        #endregion

        #region output methods

        void WriteProgress()
        {
            WriteProgress(string.Empty);
        }

        void WriteProgress(string format, params object[] args)
        {
            try
            {
                m_writer.WriteLine(format, args);
            }
            catch (FormatException)
            {
                m_writer.WriteLine(format);
            }
        }

        #endregion

        #region Comparer classes

        class ScopeComparer : IComparer<ActivationObject>
        {
            // singleton instance
            public static readonly IComparer<ActivationObject> Instance = new ScopeComparer();

            // private constructor -- use singleton
            ScopeComparer() { }

            #region IComparer<ActivationObject> Members

            public int Compare(ActivationObject left, ActivationObject right)
            {
                int comparison = 0;
                SourceContext leftContext = GetContext(left);
                SourceContext rightContext = GetContext(right);
                if (leftContext == null)
                {
                    // if they're both null, return 0 (equal)
                    // otherwise just the left is null, so we want it at the end, so
                    // return 1 to indicate that it goes after the right context
                    return (rightContext == null ? 0 : 1);
                }
                else if (rightContext == null)
                {
                    // return -1 to indicate that the right context (null) goes after the left
                    return -1;
                }

                // compare their start lines
                comparison = leftContext.StartLineNumber - rightContext.StartLineNumber;
                if (comparison == 0)
                {
                    comparison = leftContext.StartColumn - rightContext.StartColumn;
                    if (comparison == 0)
                    {
                        // starts at the same place. It could be the named-function expression
                        // scopes - there's one just for the name that contains the real function
                        // scope so the function expression name doesn't pollute the containing scope.
                        // that one should go before the regular one
                        if (IsFunctionExpressionName(left))
                        {
                            comparison = -1;
                        }
                        else if (IsFunctionExpressionName(right))
                        {
                            comparison = 1;
                        }
                    }
                }
                return comparison;
            }

            static bool IsFunctionExpressionName(ActivationObject scope)
            {
                // for this to be a function expression name scope:
                // 1. the owner must not be null (well, it shouldn't ever be null)
                // 2. the owner must have its own scope (all function objects should point to their own scope)
                // 3. and this scope's owner's scope must point to a scope different than the current scope.
                return scope.Owner.IfNotNull(o => o.HasOwnScope && o.EnclosingScope != scope);
            }

            static SourceContext GetContext(ActivationObject obj)
            {
                // return the owner context, or null
                return obj.IfNotNull(s => s.Owner.Context);
            }

            #endregion
        }

        class FieldComparer : IComparer<JSVariableField>
        {
            // singleton instance
            public static readonly IComparer<JSVariableField> Instance = new FieldComparer();

            // private constructor -- use singleton
            FieldComparer() { }

            #region IComparer<JSVariableField> Members

            /// <summary>
            /// Local Argument fields first
            /// Local Arguments object
            /// Local Fields defined
            /// Local Functions defined
            /// Outer Argument fields
            /// Outer Arguments object
            /// Outer fields referenced
            /// Outer Functions referenced
            /// Global fields referenced
            /// Global functions referenced
            /// </summary>
            /// <param name="x">left-hand object</param>
            /// <param name="y">right-hand object</param>
            /// <returns>&gt;0 left before right, &lt;0 right before left</returns>
            public int Compare(JSVariableField left, JSVariableField right)
            {
                int comparison = 0;
                if (left != null && right != null)
                {
                    // compare type class
                    comparison = GetOrderIndex(left) - GetOrderIndex(right);
                    if (comparison == 0)
                    {
                        // sort alphabetically
                        comparison = string.Compare(
                          left.Name,
                          right.Name,
                          StringComparison.OrdinalIgnoreCase
                          );
                    }
                }
                return comparison;
            }

            #endregion

            static FieldOrder GetOrderIndex(JSVariableField obj)
            {
                if (obj.FieldType == FieldType.Argument || obj.FieldType == FieldType.CatchError)
                {
                    return obj.IsOuterReference ? FieldOrder.OuterArgumentReferenced : FieldOrder.LocalArgument;
                }
                if (obj.FieldType == FieldType.Arguments)
                {
                    return obj.IsOuterReference ? FieldOrder.OuterArgumentsObject : FieldOrder.LocalArgumentsObject;
                }

                if (obj.FieldType == FieldType.Super)
                {
                    return FieldOrder.LocalSuperReference;
                }

                if (obj.IsExported)
                {
                    if (obj.OuterField == null)
                    {
                        return FieldOrder.ExportedDeclaration;
                    }

                    return FieldOrder.ExportedReferences;
                }

                if (obj.FieldType == FieldType.Global)
                {
                    return (
                      obj.FieldValue is FunctionObject
                      ? FieldOrder.GlobalFunctionReferenced
                      : FieldOrder.GlobalFieldReferenced
                      );
                }

                if (obj.FieldType == FieldType.Predefined)
                {
                    return FieldOrder.Predefined;
                }

                if (obj.FieldValue is FunctionObject)
                {
                    // function
                    return obj.IsOuterReference
                        ? FieldOrder.OuterFunctionReferenced
                        : FieldOrder.LocalFunctionDefined;
                }

                // field
                return obj.IsOuterReference
                    ? FieldOrder.OuterFieldReferenced
                    : FieldOrder.LocalFieldDefined;
            }

            enum FieldOrder : int
            {
                LocalArgument = 0,
                LocalArgumentsObject,
                LocalSuperReference,
                ExportedDeclaration,
                ExportedReferences,
                LocalFieldDefined,
                LocalFunctionDefined,
                OuterArgumentReferenced,
                OuterArgumentsObject,
                OuterFieldReferenced,
                OuterFunctionReferenced,
                GlobalFieldReferenced,
                GlobalFunctionReferenced,
                Predefined,
                Other
            }
        }

        #endregion
    }
}

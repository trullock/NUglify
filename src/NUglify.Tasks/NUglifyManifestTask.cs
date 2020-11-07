// NUglifyMAnifestTask.cs
//
// Copyright 2013 Microsoft Corporation
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
using System.Globalization;
using System.IO;
using System.Text;
using NUglify.Css;
using NUglify.Helpers;
using NUglify.JavaScript;
using NUglify.JavaScript.Visitors;

namespace NUglify
{
    /// <summary>
    /// MSBuild task for NUglify manifest files
    /// </summary>
    public class NUglifyManifestTask : NUglifyManifestBaseTask
    {
        #region public properties

        /// <summary>
        /// Whether to treat warnings as errors
        /// </summary>
        public bool TreatWarningsAsErrors { get; set; }

        #endregion

        #region base task overrides

        protected override void GenerateJavaScript(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            if (uglifyCommandParser == null)
            {
                throw new ArgumentNullException("uglifyCommandParser");
            }

            try
            {
                var settings = uglifyCommandParser.JSSettings;

                // process the resources for this output group into the settings list
                // if there are any to be processed
                if (outputGroup != null && settings != null
                    && outputGroup.Resources.IfNotNull(rs => rs.Count > 0))
                {
                    outputGroup.ProcessResourceStrings(settings.ResourceStrings, null);
                }

                // then process the javascript output group
                ProcessJavaScript(
                    inputGroups,
                    uglifyCommandParser,
                    outputPath,
                    outputGroup.IfNotNull(og => og.SymbolMap),
                    outputEncoding);
            }
            catch (ArgumentException ex)
            {
                // processing the resource strings could throw this exception
                Log.LogError(ex.Message);
            }
        }

        protected override void GenerateStyleSheet(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            if (uglifyCommandParser == null)
            {
                throw new ArgumentNullException("uglifyCommandParser");
            }

            ProcessStylesheet(
                inputGroups,
                uglifyCommandParser,
                outputPath,
                outputEncoding);
        }

        #endregion

        #region code processing methods

        void ProcessJavaScript(IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, SymbolMap symbolMap, Encoding outputEncoding)
        {
            var settings = uglifyCommandParser.JSSettings;
            TextWriter mapWriter = null;
            ISourceMap sourceMap = null;
            try
            {
                // if we want a symbols map, we need to set it up now
                if (symbolMap != null && !settings.PreprocessOnly)
                {
                    // if we specified the path, use it. Otherwise just use the output path with
                    // ".map" appended to the end. Eg: output.js => output.js.map
                    var symbolMapPath = symbolMap.Path.IsNullOrWhiteSpace()
                        ? outputPath + ".map"
                        : symbolMap.Path;

                    // create the map writer and the source map implementation.
                    // look at the Name attribute and implement the proper one.
                    // the encoding needs to be UTF-8 WITHOUT a BOM or it won't work.
                    if (!FileWriteOperation(symbolMapPath, uglifyCommandParser.Clobber, () =>
                        {
                            mapWriter = new StreamWriter(symbolMapPath, false, new UTF8Encoding(false));
                            sourceMap = SourceMapFactory.Create(mapWriter, symbolMap.Name);
                            if (sourceMap != null)
                            {
                                // if we get here, the symbol map now owns the stream and we can null it out so
                                // we don't double-close it
                                mapWriter = null;
                                settings.SymbolsMap = sourceMap;

                                // copy some property values
                                sourceMap.SourceRoot = symbolMap.SourceRoot.IfNullOrWhiteSpace(null);
                                sourceMap.SafeHeader = symbolMap.SafeHeader.GetValueOrDefault(false);

                                // start the package
                                sourceMap.StartPackage(outputPath, symbolMapPath);
                            }

                            return true;
                        }))
                    {
                        // could not write file
                        Log.LogError(Strings.CouldNotWriteOutputFile, symbolMapPath);
                    }
                }

                // save the original term settings. We'll make sure to set this back again
                // for the last item in the group, but we'll make sure it's TRUE for all the others.
                var originalTermSetting = settings.TermSemicolons;
                var currentSourceOrigin = SourceOrigin.Project;

                var parser = new JSParser();
                parser.CompilerError += (sender, ea) =>
                {
                    // if the input group isn't project, then we only want to report sev-0 errors.
                    // regardless, don't show any errors that have a severity lower (greater numeric value)
                    // than the warning level specified.
                    if ((currentSourceOrigin == SourceOrigin.Project || ea.Error.Severity == 0)
                        && ea.Error.Severity <= uglifyCommandParser.WarningLevel)
                    {
                        LogContextError(ea.Error);
                    }
                };

                var outputBuilder = new StringBuilder(8192);
                using (var writer = new StringWriter(outputBuilder, CultureInfo.InvariantCulture))
                {
                    for (var inputGroupIndex = 0; inputGroupIndex < inputGroups.Count; ++inputGroupIndex)
                    {
                        var inputGroup = inputGroups[inputGroupIndex];
                        currentSourceOrigin = inputGroup.Origin;

                        // for all but the last item, we want the term-semicolons setting to be true.
                        // but for the last entry, set it back to its original value
                        settings.TermSemicolons = inputGroupIndex < inputGroups.Count - 1 ? true : originalTermSetting;

                        if (settings.PreprocessOnly)
                        {
                            parser.EchoWriter = writer;

                            if (inputGroupIndex > 0)
                            {
                                // not the first group, so output the appropriate newline
                                // sequence before we output the group.
                                writer.Write(settings.LineTerminator);
                            }
                        }
                        else
                        {
                            // not preprocess-only, so make sure the echo writer is null
                            parser.EchoWriter = null;
                        }

                        // parse the input
                        var block = parser.Parse(inputGroup.Source, settings);
                        if (block != null && !settings.PreprocessOnly)
                        {
                            if (inputGroupIndex > 0)
                            {
                                // not the first group, so output the appropriate newline
                                // sequence before we output the group.
                                writer.Write(settings.LineTerminator);
                            }

                            // minify the AST to the output
                            if (settings.Format == JavaScriptFormat.JSON)
                            {
                                if (!JsonOutputVisitor.Apply(writer, block, settings))
                                {
                                    Log.LogError(Strings.InvalidJSONOutput);
                                }
                            }
                            else
                            {
                                OutputVisitor.Apply(writer, block, settings);
                            }
                        }
                    }
                }

                // write output
                if (!Log.HasLoggedErrors)
                {
                    if (!FileWriteOperation(outputPath, uglifyCommandParser.Clobber, () =>
                        {
                            using (var writer = new StreamWriter(outputPath, false, outputEncoding))
                            {
                                // write the combined minified code
                                writer.Write(outputBuilder.ToString());

                                if (!settings.PreprocessOnly)
                                {
                                    // give the map (if any) a chance to add something
                                    settings.SymbolsMap.IfNotNull(m => m.EndFile(
                                        writer,
                                        settings.LineTerminator));
                                }
                            }

                            return true;
                        }))
                    {
                        // could not write file
                        Log.LogError(Strings.CouldNotWriteOutputFile, outputPath);
                    }
                }
                else
                {
                    Log.LogWarning(Strings.DidNotMinify, outputPath, Strings.ThereWereErrors);
                    if (File.Exists(outputPath))
                    {
                        File.Delete(outputPath);
                    }
                }
            }
            finally
            {
                if (sourceMap != null)
                {
                    mapWriter = null;
                    settings.SymbolsMap = null;
                    sourceMap.EndPackage();
                    sourceMap.Dispose();
                }

                if (mapWriter != null)
                {
                    mapWriter.Close();
                }
            }
        }

        void ProcessStylesheet(IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding encoding)
        {
            var outputBuilder = new StringBuilder(8192);
            foreach (var inputGroup in inputGroups)
            {
                // create and setup parser
                var parser = new CssParser();
                parser.Settings = uglifyCommandParser.CssSettings;
                parser.JSSettings = uglifyCommandParser.JSSettings;
                parser.CssError += (sender, ea) =>
                {
                    // if the input group is not project, then only report sev-0 errors
                    // regardless, don't show any errors that have a severity lower (greater numeric value)
                    // than the warning level specified.
                    if ((inputGroup.Origin == SourceOrigin.Project || ea.Error.Severity == 0)
                        && ea.Error.Severity <= uglifyCommandParser.WarningLevel)
                    {
                        LogContextError(ea.Error);
                    }
                };

                // minify input
                outputBuilder.Append(parser.Parse(inputGroup.Source));
            }

            // write output
            if (!Log.HasLoggedErrors)
            {
                if (!FileWriteOperation(outputPath, uglifyCommandParser.Clobber, () =>
                    {
                        using (var writer = new StreamWriter(outputPath, false, encoding))
                        {
                            writer.Write(outputBuilder.ToString());
                        }

                        return true;
                    }))
                {
                    // could not write file
                    Log.LogError(Strings.CouldNotWriteOutputFile, outputPath);
                }
            }
            else
            {
                Log.LogWarning(Strings.DidNotMinify, outputPath, Strings.ThereWereErrors);
            }
        }

        #endregion

        #region Logging methods

        /// <summary>
        /// Call this method to log an error using a ContextError object
        /// </summary>
        /// <param name="error">Error to log</param>
        void LogContextError(UglifyError error)
        {
            // log it either as an error or a warning
            if (TreatWarningsAsErrors || error.Severity < 2)
            {
                Log.LogError(
                    error.Subcategory,  // subcategory 
                    error.ErrorCode,    // error code
                    error.HelpKeyword,  // help keyword
                    error.File,         // file
                    error.StartLine,    // start line
                    error.StartColumn,  // start column
                    error.EndLine > error.StartLine ? error.EndLine : 0,      // end line
                    error.EndLine > error.StartLine || error.EndColumn > error.StartColumn ? error.EndColumn : 0,    // end column
                    error.Message       // message
                    );
            }
            else
            {
                Log.LogWarning(
                    error.Subcategory,  // subcategory 
                    error.ErrorCode,    // error code
                    error.HelpKeyword,  // help keyword
                    error.File,         // file
                    error.StartLine,    // start line
                    error.StartColumn,  // start column
                    error.EndLine > error.StartLine ? error.EndLine : 0,      // end line
                    error.EndLine > error.StartLine || error.EndColumn > error.StartColumn ? error.EndColumn : 0,    // end column
                    error.Message       // message
                    );
            }
        }

        #endregion
    }
}

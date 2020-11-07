// MainClass-JS.cs
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using NUglify.Helpers;
using NUglify.JavaScript;
using NUglify.JavaScript.Syntax;
using NUglify.JavaScript.Visitors;

namespace NUglify
{
    public partial class MainClass
    {
        #region file processing

        int ProcessJSFile(IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, StringBuilder outputBuilder)
        {
            var returnCode = 0;
            var settings = uglifyCommandParser.JSSettings;
            var currentSourceOrigin = SourceOrigin.Project;

            // blank line before
            WriteProgress();

            // create our parser object and hook up some events
            var parser = new JSParser();
            parser.UndefinedReference += OnUndefinedReference;
            parser.CompilerError += (sender, ea) =>
            {
                var error = ea.Error;
                if (currentSourceOrigin == SourceOrigin.Project || error.Severity == 0)
                {
                    // ignore severity values greater than our severity level
                    // also ignore errors that are in our ignore list (if any)
                    if (error.Severity <= uglifyCommandParser.WarningLevel)
                    {
                        // we found an error
                        m_errorsFound = true;

                        // write the error out
                        WriteError(error.ToString());
                    }
                }
            };

            // output visitor requires a text writer, so make one from the string builder
            using (var writer = new StringWriter(outputBuilder, CultureInfo.InvariantCulture))
            {
                var outputIndex = 0;
                var originalTermSetting = settings.TermSemicolons;
                for (var inputGroupIndex = 0; inputGroupIndex < inputGroups.Count; ++inputGroupIndex)
                {
                    var inputGroup = inputGroups[inputGroupIndex];
                    currentSourceOrigin = inputGroup.Origin;

                    // for all but the last item, we want the term-semicolons setting to be true.
                    // but for the last entry, set it back to its original value
                    settings.TermSemicolons = inputGroupIndex < inputGroups.Count - 1 ? true : originalTermSetting;

                    // if this is preprocess-only or echo-input, then set up the writer as the echo writer for the parser
                    if (settings.PreprocessOnly || m_echoInput)
                    {
                        parser.EchoWriter = writer;
                        if (inputGroupIndex > 0)
                        {
                            // separate subsequent input groups with an appropriate line terminator
                            writer.Write(settings.LineTerminator);
                            writer.Write(';');
                            writer.Write(settings.LineTerminator);
                        }
                    }
                    else
                    {
                        // not a preprocess-only or echo - make sure the echo writer is null
                        parser.EchoWriter = null;
                    }

                    // parse the input code. Don't use a source context because it should already be
                    // in the source using ///#SOURCE comments as we assembled the input groups.
                    var scriptBlock = parser.Parse(inputGroup.Source, settings);

                    if (m_errorsFound)
                    {
                        WriteProgress();
                    }

                    if (m_outputTimer)
                    {
                        OutputTimingPoints(parser, inputGroupIndex, inputGroups.Count);
                    }

                    if (!settings.PreprocessOnly && !m_echoInput)
                    {
                        if (scriptBlock != null)
                        {
                            if (outputIndex++ > 0)
                            {
                                // separate subsequent input groups with an appropriate line terminator
                                writer.Write(settings.LineTerminator);
                            }

                            // crunch the output and write it to debug stream, but make sure
                            // the settings we use to output THIS chunk are correct
                            if (settings.Format == JavaScriptFormat.JSON)
                            {
                                if (!JsonOutputVisitor.Apply(writer, scriptBlock, settings))
                                {
                                    returnCode = 1;
                                }
                            }
                            else
                            {
                                OutputVisitor.Apply(writer, scriptBlock, settings);
                            }
                        }
                        else
                        {
                            // no code?
                            WriteProgress(NUglify.NoParsedCode);
                        }
                    }
                }

                // give the symbols map a chance to write something at the bottom of the source file
                // (and if this isn't preprocess-only or echo)
                if (settings.SymbolsMap != null && !settings.PreprocessOnly && !m_echoInput)
                {
                    settings.SymbolsMap.EndFile(writer, settings.LineTerminator);
                }
            }

            if (uglifyCommandParser.AnalyzeMode)
            {
                // blank line before
                WriteProgress();

                // output our report
                CreateReport(parser.GlobalScope, uglifyCommandParser);
            }

            return returnCode;
        }

        void OutputTimingPoints(JSParser parser, int groupIndex, int groupCount)
        {
            // frequency is ticks per second, so if we divide by 1000.0, then we will have a
            // double-precision value indicating the ticks per millisecond. Divide this into the
            // number of ticks we measure, and we'll get the milliseconds in double-precision.
            var frequency = Stopwatch.Frequency / 1000.0;

            // step names
            var stepNames = new[] { NUglify.StepParse, NUglify.StepResolve, NUglify.StepReorder, 
                                                NUglify.StepAnalyzeNode, NUglify.StepAnalyzeScope, NUglify.StepAutoRename, 
                                                NUglify.StepEvaluateLiterals, NUglify.StepFinalPass, NUglify.StepValidateNames };

            // and output other steps to debug
            var stepCount = parser.TimingPoints.Count;
            var latestTimingPoint = 0L;
            var previousTimingPoint = 0L;
            var message = string.Empty;
            var sb = StringBuilderPool.Acquire();
            try
            {
                for (var ndx = stepCount - 1; ndx >= 0; --ndx)
                {
                    if (parser.TimingPoints[ndx] != 0)
                    {
                        // 1-based step index
                        var stepIndex = stepCount - ndx;
                        latestTimingPoint = parser.TimingPoints[ndx];
                        var deltaMS = (latestTimingPoint - previousTimingPoint) / frequency;
                        previousTimingPoint = latestTimingPoint;

                        sb.AppendFormat(NUglify.Culture, NUglify.TimerStepFormat, stepIndex, deltaMS, stepNames[stepIndex - 1]);
                        sb.AppendLine();
                    }
                }

                message = sb.ToString();
            }
            finally
            {
                sb.Release();
            }

            var timerFormat = groupCount > 1 ? NUglify.TimerMultiFormat : NUglify.TimerFormat;
            var timerMessage = string.Format(CultureInfo.CurrentUICulture, timerFormat, groupIndex + 1, latestTimingPoint / frequency);
            Debug.WriteLine(timerMessage);
            Debug.Write(message);
            WriteProgress(timerMessage);
            WriteProgress(message);
        }

        #endregion

        #region CreateJSFromResourceStrings method

        static string CreateJSFromResourceStrings(ResourceStrings resourceStrings)
        {
            var sb = StringBuilderPool.Acquire();
            try
            {
                // start the var statement using the requested name and open the initializer object literal
                sb.Append("var ");
                sb.Append(resourceStrings.Name);
                sb.Append("={");

                // we're going to need to insert commas between each pair, so we'll use a boolean
                // flag to indicate that we're on the first pair. When we output the first pair, we'll
                // set the flag to false. When the flag is false, we're about to insert another pair, so
                // we'll add the comma just before.
                bool firstItem = true;

                // loop through all items in the collection
                foreach (var keyPair in resourceStrings.NameValuePairs)
                {
                    // if this isn't the first item, we need to add a comma separator
                    if (!firstItem)
                    {
                        sb.Append(',');
                    }
                    else
                    {
                        // next loop is no longer the first item
                        firstItem = false;
                    }

                    // append the key as the name, a colon to separate the name and value,
                    // and then the value
                    // must quote if not valid JS identifier format, or if it is, but it's a keyword
                    // (use strict mode just to be safe)
                    string propertyName = keyPair.Key;
                    if (!JSScanner.IsValidIdentifier(propertyName) || JSScanner.IsKeyword(propertyName, true))
                    {
                        sb.Append("\"");
                        // because we are using quotes for the delimiters, replace any instances
                        // of a quote character (") with an escaped quote character (\")
                        sb.Append(propertyName.Replace("\"", "\\\""));
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(propertyName);
                    }
                    sb.Append(':');

                    // make sure the Value is properly escaped, quoted, and whatever we
                    // need to do to make sure it's a proper JS string.
                    // pass false for whether this string is an argument to a RegExp constructor.
                    // pass false for whether to use W3Strict formatting for character escapes (use maximum browser compatibility)
                    // pass true for ecma strict mode
                    string stringValue = ConstantWrapper.EscapeString(
                        keyPair.Value,
                        false,
                        false,
                        true
                        );
                    sb.Append(stringValue);
                }

                // close the object literal and return the string
                sb.AppendLine("};");
                return sb.ToString();
            }
            finally
            {
                sb.Release();
            }
        }

        #endregion

        #region Variable Renaming method

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        void ProcessRenamingFile(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                try
                {
                    using (var reader = XmlReader.Create(fileReader))
                    {
                        // let the manifest factory do all the heavy lifting of parsing the XML
                        // into config objects
                        var config = ManifestFactory.Create(reader);
                        if (config != null)
                        {
                            // add any rename pairs
                            foreach (var pair in config.RenameIdentifiers)
                            {
                                uglifyCommandParser.JSSettings.AddRenamePair(pair.Key, pair.Value);
                            }

                            // add any no-rename identifiers
                            uglifyCommandParser.JSSettings.SetNoAutoRenames(config.NoRenameIdentifiers);
                        }
                    }
                }
                catch (XmlException e)
                {
                    // throw an error indicating the XML error
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw new NotSupportedException(NUglify.InputXmlError.FormatInvariant(e.Message));
                }
            }
        }

        #endregion
        
        #region reporting methods

        void CreateReport(GlobalScope globalScope, UglifyCommandParser uglifyCommandParser)
        {
            string reportText;
            using (var writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                using (IScopeReport scopeReport = CreateScopeReport(uglifyCommandParser))
                {
                    scopeReport.CreateReport(writer, globalScope, uglifyCommandParser.JSSettings.MinifyCode);
                }
                reportText = writer.ToString();
            }

            if (!string.IsNullOrEmpty(reportText))
            {
                if (string.IsNullOrEmpty(uglifyCommandParser.ReportPath))
                {
                    // no report path specified; send to console
                    WriteProgress(reportText);
                    WriteProgress();
                }
                else
                {
                    // report path specified -- write to the file.
                    // don't append; use UTF-8 as the output format.
                    // let any exceptions bubble up.
                    using (var writer = new StreamWriter(uglifyCommandParser.ReportPath, false, new UTF8Encoding(false)))
                    {
                        writer.Write(reportText);
                    }
                }
            }
        }

        static IScopeReport CreateScopeReport(UglifyCommandParser uglifyCommandParser)
        {
            // check the switch parser for a report format.
            // At this time we only have two: XML or DEFAULT. If it's XML, use
            // the XML report; all other values use the default report.
            // No error checking at this time. 
            if (string.CompareOrdinal(uglifyCommandParser.ReportFormat, "XML") == 0)
            {
                return new XmlScopeReport();
            }

            return new DefaultScopeReport();
        }

        #endregion

        #region Error-handling Members

        void OnUndefinedReference(object sender, UndefinedReferenceEventArgs e)
        {
            var parser = sender as JSParser;
            if (parser != null)
            {
                parser.GlobalScope.AddUndefinedReference(e.Reference);
            }
        }

        #endregion
    }
}

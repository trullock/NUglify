// MainClass.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Text;
using System.Xml;
using NUglify.Helpers;
using NUglify.JavaScript;

namespace NUglify
{
    /// <summary>
    /// Application entry point
    /// </summary>
    public partial class MainClass
    {
        #region common fields

        // default resource object name if not specified
        const string c_defaultResourceObjectName = "Strings";

        // prefix for usage messages that tell that method to not create an error string
        // from the message, but the output it directly, as-is
        const string c_rawMessagePrefix = "RAWUSAGE";

        /// <summary>
        /// whether to output the results of the timer to the console
        /// </summary>
        bool m_outputTimer;

        /// <summary>
        /// This field is initially false, and it set to true if any errors were
        /// found parsing the javascript. The return value for the application
        /// will be set to non-zero if this flag is true.
        /// Use the -W argument to limit the severity of errors caught by this flag.
        /// </summary>
        bool m_errorsFound;// = false;

        /// <summary>
        /// Set to true when header is written
        /// </summary>
        bool m_headerWritten;

        /// <summary>
        /// configuration mode
        /// </summary>
        string m_configuration;

        /// <summary>whether to suppress output of the parsed code</summary>
        bool m_noOutput;

        /// <summary>Whether to skip the size-comparison output</summary>
        bool m_skipSizeComparisons;

        #endregion

        #region private static fields

        /// <summary>
        /// Output mode
        /// </summary>
        static bool s_silentMode;

        #endregion

        #region common settings

        /// <summary>
        /// object to turn the command-line into settings object
        /// </summary>
        UglifyCommandParser uglifyCommandParser;

        // simply echo the input code, not the crunched code
        bool m_echoInput;// = false;

        /// <summary>
        /// MAnifest file built from input XML or from command-line input/output file(s)
        /// </summary>
        Manifest m_manifest;

        /// <summary>
        /// SymbolMap settings stored from command-line
        /// </summary>
        SymbolMap m_symbolMap;// = null;

        /// <summary>
        /// Input type hint from the switches: possibly JS or CSS
        /// </summary>
        CodeType m_inputTypeHint = CodeType.Unknown;

        /// <summary>
        /// Whether or not we are outputting the crunched code to one or more files (false) or to stdout (true)
        /// </summary>
        bool m_outputToStandardOut;// = false;

        /// <summary>
        /// Optional file name of the destination file. 
        /// An empty destination outputs to STDOUT. A folder path can be used with
        /// an XML manifest to specify the root output folder for relative output paths.
        /// </summary>
        string m_outputFile = string.Empty;

        /// <summary>
        /// Optionally specify an XML file that indicates the input and output file(s)
        /// instead of specifying a single output and the input file(s) on the command line.
        /// </summary>
        string m_xmlInputFile;// = null;

        #endregion

        #region properties

        /// <summary>
        /// Gets the one output group for the implicit manifest created from command-line parameters.
        /// Create the implicit manifest if one hasn't been created yet.
        /// </summary>
        OutputGroup ImplicitManifestOutputGroup
        {
            get
            {
                if (m_manifest == null)
                {
                    m_manifest = new Manifest();
                    m_manifest.Outputs.Add(new OutputGroup());
                }

                return m_manifest.Outputs[0];
            }
        }

        #endregion

        #region startup code

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            int retVal;
            if (args == null || args.Length == 0)
            {
                // no args -- we don't know whether to to parse JS or CSS,
                // so no sense waiting for input from STDIN. Output a simple
                // header that displays a message telling how to get more info.
                Console.Out.WriteLine(GetHeaderString());
                Console.Out.WriteLine(NUglify.MiniUsageMessage);
                retVal = 0;
            }
            else
            {
                try
                {
                    var app = new MainClass(args);
                    retVal = app.Run();
                }
                catch (NotSupportedException e)
                {
                    Usage(e);
                    retVal = 1;
                }
            }

            return retVal;
        }

        #endregion

        #region Constructor

        MainClass(string[] args)
        {
            // process the arguments
            ProcessArgs(args);
        }

        #endregion

        #region ProcessArgs method

        void ProcessArgs(string[] args)
        {
            if (args.Length == 1 && string.Compare(args[0], "HELP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // special case "ajaxmin help" to be the usage message
                throw new NotSupportedException(string.Empty);
            }

            // create the switch parser and hook the events we care about
            uglifyCommandParser = new UglifyCommandParser();
            uglifyCommandParser.UnknownParameter += OnUnknownParameter;

            uglifyCommandParser.CssOnlyParameter += (sender, ea) => { InputTypeHint(CodeType.StyleSheet); };
            uglifyCommandParser.JSOnlyParameter += (sender, ea) => { InputTypeHint(CodeType.JavaScript); };
            uglifyCommandParser.InvalidSwitch += (sender, ea) =>
            {
                if (ea.ParameterPart == null)
                {
                    // if there's no parameter, then the switch required an arg
                    throw new NotSupportedException(NUglify.SwitchRequiresArg.FormatInvariant(ea.SwitchPart));
                }
                else
                {
                    // otherwise the arg was invalid
                    throw new NotSupportedException(NUglify.InvalidSwitchArg.FormatInvariant(ea.ParameterPart, ea.SwitchPart));
                }
            };

            // and go
            uglifyCommandParser.Parse(args);

            // if we are going to use an xml file for input, we don't care about finding out which
            // code path to take (JS or CSS) at this point. The XML file can contain either or both.
            if (string.IsNullOrEmpty(m_xmlInputFile))
            {
                // not XML input; just command-line parameters.
                // we need to know what type we want to process. check for input file extensions.
                var outputGroup = ImplicitManifestOutputGroup;
                if (outputGroup.Inputs.Count > 0)
                {
                    // check the extensions of the files -- they can definitively tell us 
                    // what input type we want.
                    foreach (var inputFile in outputGroup.Inputs)
                    {
                        string extension = Path.GetExtension(inputFile.Path).ToUpperInvariant();
                        switch (outputGroup.CodeType)
                        {
                            case CodeType.Unknown:
                                // we don't know yet. If the extension is JS or CSS set to the
                                // appropriate input type
                                if (extension == ".JS")
                                {
                                    outputGroup.CodeType = CodeType.JavaScript;
                                }
                                else if (extension == ".CSS")
                                {
                                    outputGroup.CodeType = CodeType.StyleSheet;
                                }
                                break;

                            case CodeType.JavaScript:
                                // we know we are JS -- if we find a CSS file, throw an error
                                if (extension == ".CSS")
                                {
                                    throw new NotSupportedException(NUglify.ConflictingInputType);
                                }
                                break;

                            case CodeType.StyleSheet:
                                // we know we are CSS -- if we find a JS file, throw an error
                                if (extension == ".JS")
                                {
                                    throw new NotSupportedException(NUglify.ConflictingInputType);
                                }
                                break;
                        }

                        // while we're at it, check for existence, too
                        // (directories aren't valid for command-line input files)
                        if (!File.Exists(inputFile.Path))
                        {
                            throw new NotSupportedException(NUglify.SourceFileNotExist.FormatInvariant(inputFile.Path));
                        }
                    }

                    // if we have input files but we don't know the type by now, 
                    // then throw an exception
                    if (outputGroup.CodeType == CodeType.Unknown)
                    {
                        // we still don't know -- check the hint from the switches.
                        if (m_inputTypeHint != CodeType.Unknown && m_inputTypeHint != CodeType.Mix)
                        {
                            // either JS or CSS. use the hint.
                            outputGroup.CodeType = m_inputTypeHint;
                        }
                        else
                        {
                            // no switch hint or it's a mix, and we can't tell from the files. Error.
                            throw new NotSupportedException(NUglify.UnknownInputType);
                        }
                    }
                }
                else
                {
                    // no input files. Check the hint from the switches.
                    if (m_inputTypeHint == CodeType.Unknown || m_inputTypeHint == CodeType.Mix)
                    {
                        // can't tell; throw an exception
                        throw new NotSupportedException(NUglify.UnknownInputType);
                    }

                    // use the hint
                    outputGroup.CodeType = m_inputTypeHint;
                }
            }
        }

        void OnUnknownParameter(object sender, UnknownParameterEventArgs ea)
        {
            bool flag;
            if (ea.SwitchPart != null)
            {
                // see if the switch is okay
                switch (ea.SwitchPart)
                {
                    case "CONFIG":
                        if (ea.ParameterPart == null)
                        {
                            throw new NotSupportedException(NUglify.InvalidSwitchArg.FormatInvariant(ea.SwitchPart, ea.ParameterPart));
                        }

                        m_configuration = ea.ParameterPart;
                        break;

                    case "ECHO":
                    case "I": // <-- old style
                        // ignore any arguments
                        m_echoInput = true;

                        // -pretty and -echo are not compatible
                        if (uglifyCommandParser.AnalyzeMode)
                        {
                            throw new NotSupportedException(NUglify.PrettyAndEchoArgs);
                        }
                        break;

                    case "HELP":
                    case "?":
                        // just show usage
                        throw new NotSupportedException(string.Empty);

                    case "MAP":
                        if (!string.IsNullOrEmpty(m_xmlInputFile))
                        {
                            throw new NotSupportedException(NUglify.MapAndXmlArgs);
                        }

                        // next argument is the output path
                        // cannot have two map arguments
                        if (m_symbolMap != null && !string.IsNullOrEmpty(m_symbolMap.Path))
                        {
                            throw new NotSupportedException(NUglify.MultipleMapArg);
                        }

                        if (ea.Index >= ea.Arguments.Count - 1)
                        {
                            throw new NotSupportedException(NUglify.MapArgNeedsPath);
                        }

                        if (m_symbolMap == null)
                        {
                            m_symbolMap = new SymbolMap();
                        }

                        m_symbolMap.Path = ea.Arguments[++ea.Index];

                        // save the map implementation name, if any
                        if (!ea.ParameterPart.IsNullOrWhiteSpace())
                        {
                            m_symbolMap.Name = ea.ParameterPart;
                        }
                        break;

                    case "MAPROOT":
                        if (ea.Index >= ea.Arguments.Count - 1)
                        {
                            throw new NotSupportedException(NUglify.MapArgNeedsPath);
                        }

                        if (m_symbolMap == null)
                        {
                            m_symbolMap = new SymbolMap();
                        }

                        m_symbolMap.SourceRoot = ea.Arguments[++ea.Index];
                        break;

                    case "MAPSAFE":
                        if (m_symbolMap == null)
                        {
                            m_symbolMap = new SymbolMap();
                        }

                        if (ea.ParameterPart == null)
                        {
                            // default if specified is true
                            m_symbolMap.SafeHeader = true;
                        }
                        else
                        {
                            // there is. parse the boolean flag part
                            bool safeFlag;
                            if (UglifyCommandParser.BooleanSwitch(ea.ParameterPart.ToUpperInvariant(), true, out safeFlag))
                            {
                                m_symbolMap.SafeHeader = safeFlag;
                            }
                            else
                            {
                                // invalid argument switch
                                throw new NotSupportedException(NUglify.InvalidArgument.FormatInvariant(ea.Arguments[ea.Index]));
                            }
                        }
                        break;

                    case "MODERN":
                        // some special settings for modern apps
                        // kill a couple optimizations
                        uglifyCommandParser.JSSettings.KillSwitch |= (long)(
                            TreeModifications.CombineAdjacentExpressionStatements | TreeModifications.BooleanLiteralsToNotOperators);

                        // and skip the size-comparison output
                        m_skipSizeComparisons = true;
                        break;

                    case "NOSIZE":
                        // putting -nosize on the command line means we want to suppress the size-comparison
                        // analysis that we normally output
                        m_skipSizeComparisons = true;
                        break;

                    case "OUT":
                    case "O": // <-- old style
                        // cannot have two out arguments. If we've already seen an out statement,
                        // either we will have an output file or the no-output flag will be set
                        if (!string.IsNullOrEmpty(m_outputFile) || m_noOutput)
                        {
                            throw new NotSupportedException(NUglify.MultipleOutputArg);
                        }
                        else
                        {
                            // first instance of the -out switch. 
                            // First check to see if there's a flag on the output switch
                            if (!string.IsNullOrEmpty(ea.ParameterPart))
                            {
                                // there is. See if it's a boolean false. If it is, then we want no output 
                                // and we don't follow this switch with an output path.
                                bool outputSwitch;
                                if (UglifyCommandParser.BooleanSwitch(ea.ParameterPart.ToUpperInvariant(), true, out outputSwitch))
                                {
                                    // the no-output flag is the opposite of the boolean flag
                                    m_noOutput = !outputSwitch;
                                }
                                else
                                {
                                    // invalid argument switch
                                    throw new NotSupportedException(NUglify.InvalidArgument.FormatInvariant(ea.Arguments[ea.Index]));
                                }
                            }

                            // if we still want output, then the next argument is the output path
                            if (!m_noOutput)
                            {
                                if (ea.Index >= ea.Arguments.Count - 1)
                                {
                                    throw new NotSupportedException(NUglify.OutputArgNeedsPath);
                                }

                                m_outputFile = ea.Arguments[++ea.Index];
                            }
                        }
                        break;

                    case "RENAME":
                        if (ea.ParameterPart == null)
                        {
                            // there are no other parts after -rename
                            // the next argument should be a filename from which we can pull the
                            // variable renaming information
                            if (ea.Index >= ea.Arguments.Count - 1)
                            {
                                // must be followed by an encoding
                                throw new NotSupportedException(NUglify.RenameArgMissingParameterOrFilePath.FormatInvariant(ea.SwitchPart));
                            }

                            // the renaming file is specified as the NEXT argument
                            string renameFilePath = ea.Arguments[++ea.Index];

                            // and it needs to exist
                            EnsureInputFileExists(renameFilePath);

                            // process the renaming file
                            ProcessRenamingFile(renameFilePath);
                        }
                        break;

                    case "RES":
                    case "R": // <-- old style
                        // -res:id path
                        // must have resource file on next parameter
                        if (ea.Index >= ea.Arguments.Count - 1)
                        {
                            throw new NotSupportedException(NUglify.ResourceArgNeedsPath);
                        }

                        // the resource file name is the NEXT argument
                        var resourceFile = ea.Arguments[++ea.Index];
                        EnsureInputFileExists(resourceFile);

                        // create the resource strings object from the file name
                        // will throw an error if not a supported file type
                        var resourceStrings = ProcessResourceFile(resourceFile);

                        // set the object name from the parameter part
                        if (!string.IsNullOrEmpty(ea.ParameterPart))
                        {
                            // must be a series of JS identifiers separated by dots: IDENT(.IDENT)*
                            // if any part doesn't match a JAvaScript identifier, throw an error
                            var parts = ea.ParameterPart.Split('.');
                            foreach (var part in parts)
                            {
                                if (!JSScanner.IsValidIdentifier(part))
                                {
                                    throw new NotSupportedException(NUglify.ResourceArgInvalidName.FormatInvariant(part));
                                }
                            }

                            // if we got here, then everything is valid; save the name portion
                            resourceStrings.Name = ea.ParameterPart;
                        }
                        else
                        {
                            // no name specified -- use Strings as the default
                            // (not recommended)
                            resourceStrings.Name = c_defaultResourceObjectName;
                        }

                        // add it to the settings objects
                        uglifyCommandParser.JSSettings.AddResourceStrings(resourceStrings);
                        uglifyCommandParser.CssSettings.AddResourceStrings(resourceStrings);

                        break;

                    case "SILENT":
                    case "S": // <-- old style
                        // ignore any argument part
                        s_silentMode = true;
                        m_skipSizeComparisons = true;
                        break;

                    case "TIME":
                    case "TIMER":
                    case "TIMES":
                        // putting the timer switch on the command line without any arguments
                        // is the same as putting -timer:true and perfectly valid.
                        if (ea.ParameterPart == null)
                        {
                            m_outputTimer = true;
                        }
                        else if (UglifyCommandParser.BooleanSwitch(ea.ParameterPart.ToUpperInvariant(), true, out flag))
                        {
                            m_outputTimer = flag;
                        }
                        else
                        {
                            throw new NotSupportedException(NUglify.InvalidSwitchArg.FormatInvariant(ea.SwitchPart, ea.ParameterPart));
                        }
                        break;

                    case "VERBOSE":
                        // ignore any argument part
                        s_silentMode = false;
                        break;

                    case "VERSION":
                        // the user just wants the version number
                        string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                        // the special prefix tells the Usage method to not create an error
                        // out of the text and just output it as-is
                        s_silentMode = true;
                        throw new NotSupportedException(c_rawMessagePrefix + version);

                    case "XML":
                    case "X": // <-- old style
                        if (m_symbolMap != null)
                        {
                            throw new NotSupportedException(NUglify.MapAndXmlArgs);
                        }

                        if (!string.IsNullOrEmpty(m_xmlInputFile))
                        {
                            throw new NotSupportedException(NUglify.MultipleXmlArgs);
                        }

                        // cannot have input files from the command line
                        if (m_manifest != null)
                        {
                            throw new NotSupportedException(NUglify.XmlArgHasInputFiles);
                        }

                        if (ea.Index >= ea.Arguments.Count - 1)
                        {
                            throw new NotSupportedException(NUglify.XmlArgNeedsPath);
                        }

                        // the xml file name is the NEXT argument
                        m_xmlInputFile = ea.Arguments[++ea.Index];

                        // and it must exist
                        EnsureInputFileExists(m_xmlInputFile);
                        break;

                    case "CL":
                    case "CS":
                    case "V":
                    case "3":
                        // just ignore -- for backwards-compatibility
                        break;

                    default:
                        // truly an unknown parameter -- throw a usage error
                        throw new NotSupportedException(NUglify.InvalidArgument.FormatInvariant(ea.Arguments[ea.Index]));
                }
            }
            else
            {
                // no switch -- then this must be an input file!
                // cannot coexist with XML file
                if (!string.IsNullOrEmpty(m_xmlInputFile))
                {
                    throw new NotSupportedException(NUglify.XmlArgHasInputFiles);
                }

                ImplicitManifestOutputGroup.Inputs.Add(new InputFile { Path = ea.Arguments[ea.Index] });
            }
        }

        void InputTypeHint(CodeType inputTypeHint)
        {
            switch (m_inputTypeHint)
            {
                case CodeType.Unknown:
                    // if we don't know yet, make the assumption
                    m_inputTypeHint = inputTypeHint;
                    break;

                case CodeType.JavaScript:
                case CodeType.StyleSheet:
                    // if what we've had before doesn't mesh with what we have now,
                    // then we have a mix of switches
                    if (m_inputTypeHint != inputTypeHint)
                    {
                        m_inputTypeHint = CodeType.Mix;
                    }
                    break;

                case CodeType.Mix:
                    // a mix is a mix
                    break;
            }
        }

        static void EnsureInputFileExists(string fileName)
        {
            // make sure it exists
            if (!File.Exists(fileName))
            {
                // file doesn't exist -- is it a folder?
                if (Directory.Exists(fileName))
                {
                    // cannot be a folder
                    throw new NotSupportedException(NUglify.SourceFileIsFolder.FormatInvariant(fileName));
                }
                else
                {
                    // just plain doesn't exist
                    throw new NotSupportedException(NUglify.SourceFileNotExist.FormatInvariant(fileName));
                }
            }
        }

        #endregion

        #region Usage method

        static void Usage(NotSupportedException e)
        {
            string fileName = Path.GetFileName(
              Assembly.GetExecutingAssembly().Location
              );

            // only output the header if we aren't supposed to be silent
            if (!s_silentMode)
            {
                Console.Error.WriteLine(GetHeaderString());
            }

            // if we have a message, then only output the mini-usage message that
            // tells the user how to get the full usage text. It's getting too long and
            // obscuring the error messages
            if (!e.Message.IsNullOrWhiteSpace())
            {
                if (!s_silentMode)
                {
                    Console.Error.WriteLine(NUglify.MiniUsageMessage);
                    Console.Error.WriteLine();
                }

                if (e.Message.StartsWith(c_rawMessagePrefix, StringComparison.Ordinal))
                {
                    Console.Out.WriteLine(e.Message.Substring(c_rawMessagePrefix.Length));
                }
                else
                {
                    Console.Error.WriteLine(CreateBuildError(
                        null,
                        null,
                        "AM-USAGE", // NON-LOCALIZABLE error code
                        e.Message));
                }
            }
            else if (!s_silentMode)
            {
                Console.Error.WriteLine(NUglify.Usage.FormatInvariant(fileName));
            }
        }

        #endregion

        #region Run method

        int Run()
        {
            int retVal = 0;
            if (m_xmlInputFile.IsNullOrWhiteSpace())
            {
                // not from an XML file. 
                var outputGroup = ImplicitManifestOutputGroup;
                outputGroup.Path = m_outputFile;

                // add symbol map info if we have any.
                // have to have at least one of these two fields.
                if (m_symbolMap != null)
                {
                    outputGroup.SymbolMap = m_symbolMap;
                }

                try
                {
                    // validate and normalize the manifest
                    m_manifest.ValidateAndNormalize(Environment.CurrentDirectory, Environment.CurrentDirectory, true);
                }
                catch (FileNotFoundException ex)
                {
                    // throw an error indicating the file-not-found error. The file name should already be
                    // in the error message.
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    throw new NotSupportedException(ex.Message + ex.FileName.IfNotNull(s => " " + s).IfNullOrWhiteSpace(string.Empty));
                }
                catch (XmlException ex)
                {
                    // throw an error indicating the XML error
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    throw new NotSupportedException(NUglify.InputXmlError.FormatInvariant(ex.Message));
                }
            }
            else
            {
                // processing the XML will take care of the validation as well.
                m_manifest = ProcessXmlFile(m_xmlInputFile, m_outputFile);
            }

            if (m_manifest.Outputs.Count > 0)
            {
                // if there are any default arguments, then we are coming from an XML file that has 
                // a default set of arguments. Apply them on top of the arguments we parsed from 
                // the command line
                var defaultArguments = m_manifest.GetConfigArguments(m_configuration);
                if (!defaultArguments.IsNullOrWhiteSpace())
                {
                    // parse the default arguments right on top of the ones we parsed from the command-line
                    uglifyCommandParser.Parse(defaultArguments);
                }

                // if any one crunch group is writing to stdout, then we need to make sure
                // that no progress or informational messages go to stdout or we will output 
                // invalid JavaScript/CSS. Loop through the crunch groups and if any one is
                // outputting to stdout, set the appropriate flag.
                foreach (var outputGroup in m_manifest.Outputs)
                {
                    if (outputGroup.Path.IsNullOrWhiteSpace())
                    {
                        // set the flag; no need to check any more
                        m_outputToStandardOut = true;
                        break;
                    }
                }

                // loop through all the crunch groups
                retVal = this.ProcessOutputGroups(m_manifest.Outputs);
            }
            else
            {
                // no crunch groups
                throw new NotSupportedException(NUglify.NoInput);
            }

            return retVal;
        }

        int ProcessOutputGroups(IList<OutputGroup> outputGroups)
        {
            var retVal = 0;
            var ndxGroup = 0;

            foreach (var outputGroup in outputGroups)
            {
                ++ndxGroup;
                var minifiedCode = 1;

                // create clones of the overall settings to which we will then apply
                // our changes for this current crunch group
                var switchParser = uglifyCommandParser.Clone();
                switchParser.Parse(outputGroup.GetConfigArguments(m_configuration));

                TextWriter symbolMapWriter = null;
                ISourceMap sourceMap = null;
                try
                {
                    if (outputGroup.SymbolMap != null)
                    {
                        retVal = this.ClobberFileAndExecuteOperation(
                            outputGroup.SymbolMap.Path, (path) =>
                            {
                                // the spec says UTF-8, but Chrome fails to load the map if there's a BOM.
                                // So make sure the BOM doesn't get written.
                                symbolMapWriter = new StreamWriter(path, false, new UTF8Encoding(false));
                            });

                        if (retVal != 0)
                        {
                            return retVal;
                        }
                    }

                    if (symbolMapWriter != null)
                    {
                        // which implementation to instantiate?
                        // if we don't know, implement the XML writer, since that was the first one before names
                        // were introduced.
                        sourceMap = SourceMapFactory.Create(symbolMapWriter, outputGroup.SymbolMap.Name ?? ScriptSharpSourceMap.ImplementationName);
                        if (sourceMap != null)
                        {
                            // if we get here, the symbol map implementation now owns the stream and we can
                            // set it to null so we don't double-dispose it.
                            symbolMapWriter = null;

                            // set some properties used by some of the implementations
                            sourceMap.SourceRoot = outputGroup.SymbolMap.SourceRoot;
                            sourceMap.SafeHeader = outputGroup.SymbolMap.SafeHeader.GetValueOrDefault(false);

                            // start off the package
                            switchParser.JSSettings.SymbolsMap = sourceMap;
                            sourceMap.StartPackage(outputGroup.Path, outputGroup.SymbolMap.Path);
                        }
                    }

                    // process the output group
                    minifiedCode = this.ProcessOutputGroup(outputGroup, switchParser);
                }
                finally
                {
                    if (sourceMap != null)
                    {
                        switchParser.JSSettings.SymbolsMap = null;
                        sourceMap.EndPackage();
                        sourceMap.Dispose();
                    }

                    if (symbolMapWriter != null)
                    {
                        symbolMapWriter.Close();
                        symbolMapWriter = null;
                    }
                }

                // if the result contained an error...
                if (minifiedCode != 0)
                {
                    // if we're processing more than one group, we should output an
                    // error message indicating that this group encountered an error
                    if (outputGroups.Count > 1)
                    {
                        // non-localized string, so format is not in the resources
                        string errorCode = "AM{0:D4}".FormatInvariant(minifiedCode);

                        // if there is an output file name, use it.
                        if (!string.IsNullOrEmpty(outputGroup.Path))
                        {
                            this.WriteError(
                                outputGroup.Path,
                                NUglify.OutputFileErrorSubCat,
                                errorCode,
                                NUglify.OutputFileError.FormatInvariant(minifiedCode));
                        }
                        else if (!string.IsNullOrEmpty(this.m_xmlInputFile))
                        {
                            // use the XML file as the location, and the index of the group for more info
                            // inside the message
                            this.WriteError(
                                this.m_xmlInputFile,
                                NUglify.OutputGroupErrorSubCat,
                                errorCode,
                                NUglify.OutputGroupError.FormatInvariant(ndxGroup, minifiedCode));
                        }
                        else
                        {
                            // no output file name, and not from an XML file. If it's not from an XML
                            // file, then there really should only be one crunch group.
                            // but just in case, use "stdout" as the output file and the index of the group 
                            // in the list (which should probably just be zero)
                            this.WriteError(
                                "stdout",
                                NUglify.OutputGroupErrorSubCat,
                                errorCode,
                                NUglify.OutputGroupError.FormatInvariant(ndxGroup, minifiedCode));
                        }
                    }

                    // return the error. Only the last one will be used
                    retVal = minifiedCode;
                }
            }

            return retVal;
        }

        #endregion

        #region ProcessCrunchGroup method

        int ProcessOutputGroup(OutputGroup outputGroup, UglifyCommandParser uglifyCommandParser)
        {
            int retVal = 0;

            // if there are no input groups, we'll be reading from STDIN and should output our
            // headers right now.
            WriteProgress();
            if (outputGroup.Inputs.Count == 0)
            {
                // output status message so user knows input coming from stdin
                WriteProgress(NUglify.MinifyFromStdIn);
            }
            else if (outputGroup.Inputs.Count == 1)
            {
                // minifying a single input group. Simple message to let user know what's going on
                WriteProgress(NUglify.MinifySingleInput.FormatInvariant(outputGroup.Inputs[0].Path));
            }
            else
            {
                // combining and minifying. More complex messaging.
                WriteProgress(NUglify.MinifyingMultipleInputs);
                foreach (var input in outputGroup.Inputs)
                {
                    WriteProgress("\t{0}", input.Path);
                }
            }

            // combine all the source files into a single string, delimited with ///#SOURCE comments so we can track
            // back to the original files. Also, add all the raw input to the echo builder.
            var inputGroups = outputGroup.ReadInputGroups(uglifyCommandParser.EncodingInputName);

            // we are always going to build the combined raw sources so we can run some comparison calcs on them.
            var rawBuilder = new StringBuilder(8192);

            // for calculation purposes, we're going to want to calculate the length of 
            // all the raw sources, and combine them together
            var sourceLength = 0L;
            foreach (var inputGroup in inputGroups)
            {
                // add the raw source length and the raw source
                // to the echo builder
                sourceLength += inputGroup.RawSource.Length;
                rawBuilder.Append(inputGroup.RawSource);
            }

            // if the crunch group has any resource strings objects, we need to add them to the back
            // of the settings list
            var hasCrunchSpecificResources = outputGroup.Resources.Count > 0;

            // create a string builder we'll dump our output into
            var outputBuilder = new StringBuilder(8192);

            // we're just echoing the input -- so if this is a JS output file,
            // we want to output a JS version of all resource dictionaries at the top
            // of the file.
            if (m_echoInput
                && outputGroup.CodeType == CodeType.JavaScript
                && uglifyCommandParser.JSSettings.ResourceStrings.Count > 0)
            {
                foreach (var resourceStrings in uglifyCommandParser.JSSettings.ResourceStrings)
                {
                    string resourceObject = CreateJSFromResourceStrings(resourceStrings);
                    outputBuilder.Append(resourceObject);
                }
            }

            switch (outputGroup.CodeType)
            {
                case CodeType.StyleSheet:
                    if (hasCrunchSpecificResources)
                    {
                        // add to the CSS list
                        outputGroup.ProcessResourceStrings(uglifyCommandParser.CssSettings.ResourceStrings, null);
                    }

                    retVal = ProcessCssFile(inputGroups, uglifyCommandParser, outputBuilder);
                    break;

                case CodeType.JavaScript:
                    if (hasCrunchSpecificResources)
                    {
                        // add to the JS list
                        outputGroup.ProcessResourceStrings(uglifyCommandParser.JSSettings.ResourceStrings, c_defaultResourceObjectName);
                    }

                    retVal = ProcessJSFile(inputGroups, uglifyCommandParser, outputBuilder);
                    break;

                default:
                    throw new NotSupportedException(NUglify.UnknownInputType);
            }

            // if we are pretty-printing, add a newline
            if (uglifyCommandParser.PrettyPrint)
            {
                outputBuilder.AppendLine();
            }

            string outputCode = outputBuilder.ToString();

            // use the output group encoding. If none specified, use the default output encoding.
            var encodingOutput = outputGroup.GetEncoding(uglifyCommandParser.EncodingOutputName);

            // now write the final output file
            if (outputGroup.Path.IsNullOrWhiteSpace())
            {
                // no output file specified - send to STDOUT
                // if the code is empty, don't bother outputting it to the console
                if (!outputCode.IsNullOrWhiteSpace())
                {
                    // however, for some reason when I set the output encoding it
                    // STILL doesn't call the EncoderFallback to Unicode-escape characters
                    // not supported by the encoding scheme. So instead we need to run the
                    // translation outselves. Still need to set the output encoding, though,
                    // so the translated bytes get displayed properly in the console.
                    byte[] encodedBytes = encodingOutput.GetBytes(outputCode);

                    // only output the size analysis if we aren't echoing the input
                    // and we haven't said we want to skip it.
                    if (!m_echoInput && !s_silentMode && !m_skipSizeComparisons)
                    {
                        // calculate the percentage saved
                        var percentage = Math.Round((1 - ((double)encodedBytes.Length) / sourceLength) * 100, 1);
                        WriteProgress(NUglify.SavingsMessage.FormatInvariant(
                                            sourceLength,
                                            encodedBytes.Length,
                                            percentage
                                            ));

                        // calculate how much gzip on the unminified, combined original source might be
                        long gzipLength = CalculateGzipSize(encodingOutput.GetBytes(rawBuilder.ToString()));
                        percentage = Math.Round((1 - ((double)gzipLength) / sourceLength) * 100, 1);
                        WriteProgress(NUglify.SavingsGzipSourceMessage.FormatInvariant(gzipLength, percentage));

                        // calculate how much gzip on the minified output might be
                        gzipLength = CalculateGzipSize(encodedBytes);
                        percentage = Math.Round((1 - ((double)gzipLength) / sourceLength) * 100, 1);
                        WriteProgress(NUglify.SavingsGzipMessage.FormatInvariant(gzipLength, percentage));

                        // blank line after
                        WriteProgress();
                    }

                    // send to console out -- if we even want any output
                    if (!m_noOutput)
                    {
                        // set the console encoding
                        try
                        {
                            // try setting the appropriate output encoding
                            Console.OutputEncoding = encodingOutput;
                        }
                        catch (IOException e)
                        {
                            // sometimes they will error, in which case we'll just set it to ascii
                            Debug.WriteLine(e.ToString());

                            // see if we know what kind of fallback we need
                            EncoderFallback fallback = null;
                            if (outputGroup.CodeType == CodeType.JavaScript)
                            {
                                fallback = new JsEncoderFallback();
                            }
                            else if (outputGroup.CodeType == CodeType.StyleSheet)
                            {
                                fallback = new CssEncoderFallback();
                            }

                            if (fallback != null)
                            {
                                // try it again -- but this time if it fails, just leave the stream
                                // to its default encoding.
                                try
                                {
                                    var asciiClone = (Encoding)Encoding.ASCII.Clone();
                                    asciiClone.EncoderFallback = fallback;
                                    Console.OutputEncoding = asciiClone;
                                }
                                catch (IOException)
                                {
                                    // ignore
                                }
                            }
                        }

                        Console.Out.Write(Console.OutputEncoding.GetChars(encodedBytes));
                        WriteProgress();
                    }
                }
            }
            else
            {
                retVal = this.ClobberFileAndExecuteOperation(
                    outputGroup.Path, (path) =>
                    {
                        // create the output file using the given encoding
                        using (StreamWriter outputStream = new StreamWriter(
                           path,
                           false,
                           encodingOutput
                           ))
                        {
                            outputStream.Write(outputCode);
                        }

                        // only output the size analysis if there is actually some output to measure
                        // and we're not echoing the input
                        if (File.Exists(path) && !m_echoInput && !s_silentMode && !m_skipSizeComparisons)
                        {
                            // get the size of the resulting file
                            FileInfo crunchedFileInfo = new FileInfo(path);
                            long crunchedLength = crunchedFileInfo.Length;
                            if (crunchedLength > 0)
                            {
                                // calculate the percentage saved by minification
                                var percentage = Math.Round((1 - ((double)crunchedLength) / sourceLength) * 100, 1);
                                WriteProgress(NUglify.SavingsMessage.FormatInvariant(
                                    sourceLength,
                                    crunchedLength,
                                    percentage));

                                // calculate how much gzip on the unminified, combined original source might be
                                long gzipLength = CalculateGzipSize(encodingOutput.GetBytes(rawBuilder.ToString()));
                                percentage = Math.Round((1 - ((double)gzipLength) / sourceLength) * 100, 1);
                                WriteProgress(NUglify.SavingsGzipSourceMessage.FormatInvariant(gzipLength, percentage));

                                // calculate how much gzip on the minified output might be
                                gzipLength = CalculateGzipSize(File.ReadAllBytes(path));
                                percentage = Math.Round((1 - ((double)gzipLength) / sourceLength) * 100, 1);
                                WriteProgress(NUglify.SavingsGzipMessage.FormatInvariant(gzipLength, percentage));

                                // blank line after
                                WriteProgress();
                            }
                        }
                    });
            }

            if (retVal == 0 && m_errorsFound)
            {
                // make sure we report an error
                retVal = 1;
            }
            return retVal;
        }

        int ClobberFileAndExecuteOperation(string filePath, Action<string> operation)
        {
            int retVal = 0;

            // send output to file
            try
            {
                // make sure the destination folder exists
                FileInfo fileInfo = new FileInfo(filePath);
                DirectoryInfo destFolder = new DirectoryInfo(fileInfo.DirectoryName);
                if (!destFolder.Exists)
                {
                    destFolder.Create();
                }

                // if the file doesn't exist, we write.
                // else (it does exist)
                //      determine read-only state
                //      if not readonly and clobber is not noclobber, we write
                //      else if it is readonly and clobber is clobber, we change flag and write
                var doWrite = !File.Exists(filePath);
                if (!doWrite)
                {
                    // file exists. determine read-only status
                    var isReadOnly = (File.GetAttributes(filePath) & FileAttributes.ReadOnly) != 0;
                    if (!isReadOnly && uglifyCommandParser.Clobber != ExistingFileTreatment.Preserve)
                    {
                        // file exists, it's not read-only, and we don't have noclobber set.
                        // noclobber will never write over an existing file, but auto will
                        // write over an existing file that doesn't have read-only set.
                        doWrite = true;
                    }
                    else if (isReadOnly && uglifyCommandParser.Clobber == ExistingFileTreatment.Overwrite)
                    {
                        // file exists, it IS read-only, and we want to clobber.
                        // noclobber will never write over an existing file, and auto
                        // won't write over a read-only file. But clobber writes over anything.
                        File.SetAttributes(
                            filePath,
                            (File.GetAttributes(filePath) & ~FileAttributes.ReadOnly));
                        doWrite = true;
                    }
                }

                if (doWrite)
                {
                    operation(filePath);
                }
                else
                {
                    retVal = 1;
                    WriteError("AM-IO", NUglify.NoClobberError.FormatInvariant(filePath));
                }
            }
            catch (ArgumentException e)
            {
                retVal = 1;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                WriteError("AM-PATH", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                retVal = 1;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                WriteError("AM-AUTH", e.Message);
            }
            catch (PathTooLongException e)
            {
                retVal = 1;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                WriteError("AM-PATH", e.Message);
            }
            catch (SecurityException e)
            {
                retVal = 1;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                WriteError("AM-SEC", e.Message);
            }
            catch (IOException e)
            {
                retVal = 1;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                WriteError("AM-IO", e.Message);
            }

            return retVal;
        }

        #endregion

        #region ProcessXmlFile method

        static Manifest ProcessXmlFile(string xmlPath, string outputFolder)
        {
            Manifest manifest = null;
            try
            {
                manifest = ManifestUtilities.ReadManifestFile(xmlPath);
                manifest.ValidateAndNormalize(Path.GetDirectoryName(xmlPath), outputFolder, true);
            }
            catch (FileNotFoundException ex)
            {
                // throw an error indicating the XML error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new NotSupportedException(NUglify.InputXmlError.FormatInvariant(ex.Message + ex.FileName.IfNotNull(s => " " + s).IfNullOrWhiteSpace(string.Empty)));
            }
            catch (XmlException ex)
            {
                // throw an error indicating the XML error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new NotSupportedException(NUglify.InputXmlError.FormatInvariant(ex.Message));
            }

            return manifest;
        }

        #endregion

        #region resource processing

        ResourceStrings ProcessResourceFile(string resourceFileName)
        {
            WriteProgress(
                NUglify.ReadingResourceFile.FormatInvariant(Path.GetFileName(resourceFileName))
                );

            // which method we call to process the resources depends on the file extension
            // of the resources path given to us.
            switch (Path.GetExtension(resourceFileName).ToUpperInvariant())
            {
                case ".RESX":
                    // process the resource file as a RESX xml file
                    return ProcessResXResources(resourceFileName);

                case ".RESOURCES":
                    // process the resource file as a compiles RESOURCES file
                    return ProcessResources(resourceFileName);

                default:
                    // no other types are supported
                    throw new NotSupportedException(NUglify.ResourceArgInvalidType);
            }
        }

        static ResourceStrings ProcessResources(string resourceFileName)
        {
            // default return object is null, meaning we are outputting the JS code directly
            // and don't want to replace any referenced resources in the sources
            var resourceStrings = new ResourceStrings();
            using (ResourceReader reader = new ResourceReader(resourceFileName))
            {
                foreach (DictionaryEntry item in reader)
                {
                    resourceStrings[item.Key.ToString()] = item.Value.ToString();
                }
            }

            return resourceStrings.Count == 0 ? null : resourceStrings;
        }

        static ResourceStrings ProcessResXResources(string resourceFileName)
        {
            // default return object is null, meaning we are outputting the JS code directly
            // and don't want to replace any referenced resources in the sources
            var resourceStrings = new ResourceStrings();
            using (ResXResourceReader reader = new ResXResourceReader(resourceFileName))
            {
                foreach(DictionaryEntry item in reader)
                {
                    resourceStrings[item.Key.ToString()] = item.Value.ToString();
                }
            }

            return resourceStrings.Count == 0 ? null : resourceStrings;
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Write an empty progress line
        /// </summary>
        void WriteProgress()
        {
            WriteProgress(string.Empty);
        }

        /// <summary>
        /// Writes a progress string to the stderr stream.
        /// if in SILENT mode, writes to debug stream, not stderr!!!!
        /// </summary>
        /// <param name="format">format string</param>
        /// <param name="args">optional arguments</param>
        void WriteProgress(string format, params object[] args)
        {
            if (!s_silentMode)
            {
                // if we are writing all output to one or more files, then progress messages will go
                // to stdout. If we are sending any minified output to stdout, then progress messages will
                // goto stderr; in that case, use the -silent option to suppress progress messages
                // from the stderr stream.
                var outputStream = m_outputToStandardOut ? Console.Error : Console.Out;

                // if we haven't yet output our header, do so now
                if (!m_headerWritten)
                {
                    // the header string will end with its own line-terminator, so we 
                    // don't need to call WriteLine
                    outputStream.Write(GetHeaderString());
                    m_headerWritten = true;
                }

                try
                {
                    outputStream.WriteLine(format, args);
                }
                catch (FormatException)
                {
                    // not enough args -- so don't use any
                    outputStream.WriteLine(format);
                }
            }
            else
            {
                // silent -- output to debug only
                try
                {
                    Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
                }
                catch (FormatException)
                {
                    // something wrong with the number of args -- don't use any
                    Debug.WriteLine(format);
                }
            }
        }

        /// <summary>
        /// Always write the string to stderr, even in silent mode
        /// </summary>
        /// <param name="message">text to write</param>
        void WriteError(string message)
        {
            // don't output the header if in silent mode
            if (!s_silentMode && !m_headerWritten)
            {
                // the header string will end with its own line-terminator, so we 
                // don't need to call WriteLine
                Console.Error.Write(GetHeaderString());
                m_headerWritten = true;
            }

            // output the error message
            Console.Error.WriteLine(message);
        }

        /// <summary>
        /// Always writes string to stderr, even if in silent mode
        /// </summary>
        /// <param name="location">optional location string, uses assembly name if not provided</param>
        /// <param name="subcategory">optional subcategory</param>
        /// <param name="code">non-localized error code</param>
        /// <param name="message">localized error message</param>
        void WriteError(string location, string subcategory, string code, string message)
        {
            // output the formatted error message
            WriteError(CreateBuildError(location, subcategory, code, message));
        }

        /// <summary>
        /// Always writes string to stderr, even if in silent mode.
        /// Use default location and subcategory values.
        /// </summary>
        /// <param name="code">non-localized error code</param>
        /// <param name="message">localized error message</param>
        void WriteError(string code, string message)
        {
            // output the formatted error message, passing null for location and subcategory
            WriteError(null, null, code, message);
        }

        /// <summary>
        /// Output a build error in a style consistent with MSBuild/Visual Studio standards
        /// so that the error gets properly picked up as a build-breaking error and displayed
        /// in the error pane
        /// </summary>
        /// <param name="location">source file(line,col), or empty for general tool error</param>
        /// <param name="subcategory">optional localizable subcategory (such as severity message)</param>
        /// <param name="code">non-localizable code indicating the error -- cannot contain spaces</param>
        /// <param name="format">localized text for error, can contain format placeholders</param>
        /// <param name="args">optional arguments for the format string</param>
        static string CreateBuildError(string location, string subcategory, string code, string message)
        {
            // if we didn't specify a location string, just use the name of this tool
            if (string.IsNullOrEmpty(location))
            {
                location = Path.GetFileName(
                    Assembly.GetExecutingAssembly().Location
                    );
            }

            // code cannot contain any spaces. If there are, trim it 
            // and replace any remaining spaces with underscores
            if (code.IndexOf(' ') >= 0)
            {
                code = code.Trim().Replace(' ', '_');
            }

            // if subcategory isn't null or empty and doesn't already end in a space, add it
            if (string.IsNullOrEmpty(subcategory))
            {
                // we are null or empty. empty is okay -- we can leave it along. But let's
                // turn nulls into emptys, too
                if (subcategory == null)
                {
                    subcategory = string.Empty;
                }
            }
            else if (!subcategory.EndsWith(" ", StringComparison.Ordinal))
            {
                // we are not empty and we don't end in a space -- add one now
                subcategory += " ";
            }
            // else we are not empty and we already end in a space, so all is good

            return "{0}: {1}{2} {3}: {4}".FormatInvariant(
                location, // not localized
                subcategory, // localizable, optional
                "error", // NOT localized
                code, // not localized, cannot contain spaces
                message // localizable with optional arguments
                );
        }

        static string GetHeaderString()
        {
            var description = string.Empty;
            var copyright = string.Empty;
            var product = string.Empty;

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var attr in assembly.GetCustomAttributes(false))
            {
                var attrType = attr.GetType();
                if (attrType == typeof(AssemblyDescriptionAttribute))
                {
                    description = ((AssemblyDescriptionAttribute)attr).Description;
                }
                else if (attrType == typeof(AssemblyCopyrightAttribute))
                {
                    copyright = ((AssemblyCopyrightAttribute)attr).Copyright;
                    copyright = copyright.Replace("©", "(c)");
                }
                else if (attrType == typeof(AssemblyProductAttribute))
                {
                    product = ((AssemblyProductAttribute)attr).Product;
                }
            }

            var assemblyName = assembly.GetName();

            // combine the information for output
            var sb = StringBuilderPool.Acquire();
            try
            {
                sb.AppendLine("{0} (version {1})".FormatInvariant(string.IsNullOrEmpty(product) ? assemblyName.Name : product, assemblyName.Version));
                if (!string.IsNullOrEmpty(description)) { sb.AppendLine(description); }
                if (!string.IsNullOrEmpty(copyright)) { sb.AppendLine(copyright); }
                return sb.ToString();
            }
            finally
            {
                sb.Release();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification="incorrect; gzipstream constructor does not close outer stream when third parameter is true")]
        static long CalculateGzipSize(byte[] bytes)
        {
            using(var memoryStream = new MemoryStream())
            {
                // the third parameter tells the GZIP stream to leave the base stream open so it doesn't
                // dispose of it when it gets disposed. This is needed because we need to dispose the 
                // GZIP stream before it will write ANY of its data.
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.Position;
            }
        }

        #endregion
    }
}
// NUglifyManifestBaseTask.cs
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
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUglify.Helpers;

namespace NUglify
{
    public abstract class NUglifyManifestBaseTask : Task
    {
        #region protected properties

        /// <summary>
        /// Whether to throw errors if an input file is missing. Default is true.
        /// </summary>
        protected bool ThrowInputMissingErrors { get; set; }

        /// <summary>
        /// Whether to ignore any source map output
        /// </summary>
        protected bool IgnoreSourceMapOutput { get; set; }

        #endregion

        #region public properties

        /// <summary>
        /// Default NUglify switches to use for the project
        /// </summary>
        public string ProjectDefaultSwitches { get; set; }

        /// <summary>
        /// Root folder to manifest input file relative paths (if different than the location of the manifest file)
        /// </summary>
        public string InputFolder { get; set; }

        /// <summary>
        /// Root folder to manifest output file relative paths (if different than the location of the manifest file)
        /// </summary>
        public string OutputFolder { get; set; }

        /// <summary>
        /// List of manifest files to process
        /// </summary>
        public ITaskItem[] Manifests { get; set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public string Configuration { get; set; }

        #endregion

        #region constructor

        protected NUglifyManifestBaseTask()
        {
            // defaults
            ThrowInputMissingErrors = true;
            IgnoreSourceMapOutput = false;
        }

        #endregion

        #region execute method

        public override bool Execute()
        {
            if (Manifests != null && Manifests.Length > 0)
            {
                // create the project default settings
                // with a default warning level of maxvalue so everything is reported unless otherwise changed
                // with a -warn switch
                var projectDefaultSettings = new UglifyCommandParser
                    {
                        WarningLevel = int.MaxValue
                    };
                if (!ProjectDefaultSwitches.IsNullOrWhiteSpace())
                {
                    projectDefaultSettings.Parse(ProjectDefaultSwitches);
                }

                // each task item represents an ajaxmin manifest file: an XML file that
                // has settings and one or more output files, each comprised of one or more
                // input files. To execute this process, we will read the XML manifest and
                // execute NUglify for each output group.
                // won't bother executing NUglify is the file time for the output file
                // is greater than all its inputs.
                foreach (var taskItem in Manifests)
                {
                    ProcessManifest(taskItem, projectDefaultSettings);
                }
            }

            // we succeeded if there have been no errors logged
            return !Log.HasLoggedErrors;
        }

        #endregion

        #region manifest processing

        void ProcessManifest(ITaskItem taskItem, UglifyCommandParser projectDefaultSettings)
        {
            // save the manifest folder - paths within the manifest will be relative to it
            // if there are no InputFolder or OutputFolder values
            var manifestFolder = Path.GetDirectoryName(taskItem.ItemSpec);
            var manifestModifiedTime = File.GetLastWriteTimeUtc(taskItem.ItemSpec);

            // process the XML file into objects
            Manifest manifest = null;
            try
            {
                // read the manifest in
                manifest = ManifestUtilities.ReadManifestFile(taskItem.ItemSpec);

                // if an input folder was specified and it exists, use that as the root
                // for all input files. Otherwise use the manifest folder path.
                var inputFolder = (this.InputFolder.IsNullOrWhiteSpace() || !Directory.Exists(this.InputFolder))
                    ? manifestFolder
                    : this.InputFolder;

                // validate and normalize all paths. 
                manifest.ValidateAndNormalize(inputFolder, this.OutputFolder, this.ThrowInputMissingErrors);
            }
            catch (FileNotFoundException ex)
            {
                Log.LogError(ex.Message + ex.FileName.IfNotNull(s => " " + s).IfNullOrWhiteSpace(string.Empty));
            }
            catch (XmlException ex)
            {
                Log.LogError(ex.Message);
            }

            if (manifest != null)
            {
                // create the default settings for this configuration, if there are any, based on
                // the project default settings.
                var defaultSettings = ParseConfigSettings(manifest.GetConfigArguments(this.Configuration), projectDefaultSettings);

                // for each output group
                foreach (var outputGroup in manifest.Outputs)
                {
                    // get the file info for the output file. It should already be normalized.
                    var outputFileInfo = new FileInfo(outputGroup.Path);

                    // the symbol map is an OPTIONAL output, so if we don't want one, we ignore it.
                    // but if we do, we need to check for its existence and filetimes, just like 
                    // the regular output file
                    var symbolsFileInfo = outputGroup.SymbolMap.IfNotNull(sm => new FileInfo(sm.Path));

                    ProcessOutputGroup(outputGroup, outputFileInfo, symbolsFileInfo, defaultSettings, manifestModifiedTime);
                }
            }
        }

        /// <summary>
        /// Process an output group. Override this method if the task doesn't want to check the input file times against
        /// the output file times (or existence) and call the GenerateOutput methods.
        /// </summary>
        /// <param name="outputGroup">the OutputGroup being processed</param>
        /// <param name="outputFileInfo">FileInfo for the desired output file</param>
        /// <param name="symbolsFileInfo">FileInfo for the optional desired symbol file</param>
        /// <param name="defaultSettings">default settings for this output group</param>
        /// <param name="manifestModifiedTime">modified time for the manifest</param>
        protected virtual void ProcessOutputGroup(OutputGroup outputGroup, FileInfo outputFileInfo, FileInfo symbolsFileInfo, UglifyCommandParser defaultSettings, DateTime manifestModifiedTime)
        {
            // check the file times -- if any of the inputs are newer than any output (or if any outputs don't exist),
            // then generate the output files
            if (AnyInputsAreNewerThanOutputs(outputGroup, outputFileInfo, symbolsFileInfo, manifestModifiedTime))
            {
                // get the settings to use -- take the configuration for this output group
                // and apply them over the default settings
                var settings = ParseConfigSettings(outputGroup.GetConfigArguments(this.Configuration), defaultSettings);

                GenerateOutputFiles(outputGroup, outputFileInfo, settings);
            }
            else
            {
                // none of the inputs are newer than the output -- we're good.
                Log.LogMessage(Strings.SkippedOutputFile, outputFileInfo.IfNotNull(fi => fi.Name) ?? string.Empty);
            }
        }

        protected abstract void GenerateJavaScript(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding);

        protected abstract void GenerateStyleSheet(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding);

        void GenerateOutputFiles(OutputGroup outputGroup, FileInfo outputFileInfo, UglifyCommandParser uglifyCommandParser)
        {
            // create combined input source
            var inputGroups = outputGroup.ReadInputGroups(uglifyCommandParser.EncodingInputName);
            if (inputGroups.Count > 0)
            {
                switch (outputGroup.CodeType)
                {
                    case CodeType.JavaScript:
                        // call the virtual function to generate the JavaScript output file from the inputs
                        GenerateJavaScript(outputGroup, inputGroups, uglifyCommandParser, outputFileInfo.FullName, outputGroup.GetEncoding(uglifyCommandParser.EncodingOutputName));
                        break;

                    case CodeType.StyleSheet:
                        // call the virtual function to generate the stylesheet output file from the inputs
                        GenerateStyleSheet(outputGroup, inputGroups, uglifyCommandParser, outputFileInfo.FullName, outputGroup.GetEncoding(uglifyCommandParser.EncodingOutputName));
                        break;

                    case CodeType.Unknown:
                        Log.LogError(Strings.UnknownCodeType);
                        break;
                }
            }
            else
            {
                // no input! write an empty output file
                if (!FileWriteOperation(outputFileInfo.FullName, uglifyCommandParser.Clobber, () =>
                    {
                        using (var stream = outputFileInfo.Create())
                        {
                            // write nothing; just create the empty file
                            return true;
                        }
                    }))
                {
                    // could not write file
                    Log.LogError(Strings.CouldNotWriteOutputFile, outputFileInfo.FullName);
                }
            }
        }

        /// <summary>
        /// Check the file times of the input files and the output files. If the output files don't exist, or if any of the input files are
        /// newer than the already-existing output files, generate the output files again by calling the virtual method GenerateOutputFiles.
        /// </summary>
        /// <param name="outputGroup">the OutputGroup being processed</param>
        /// <param name="outputFileInfo">FileInfo for the desired output file</param>
        /// <param name="symbolsFileInfo">FileInfo for the optional desired symbol file</param>
        /// <param name="manifestModifiedTime">modified time for the manifest</param>
        bool AnyInputsAreNewerThanOutputs(OutputGroup outputGroup, FileInfo outputFileInfo, FileInfo symbolsFileInfo, DateTime manifestModifiedTime)
        {
            // build the output files
            var processGroup = false;
            var codeType = outputGroup.CodeType;

            if (!outputFileInfo.Exists
                || (!IgnoreSourceMapOutput && symbolsFileInfo != null && !symbolsFileInfo.Exists))
            {
                // one or more outputs don't exist, so we need to process this group
                processGroup = true;
            }
            else
            {
                // output exists. we need to check to see if it's older than
                // any of its input files, and if not, there's no need to process
                // this group. get the filetime of the output file.
                var outputFileTime = outputFileInfo.LastWriteTimeUtc;

                // if we don't want a symbol map, then ignore that output. But if we
                // do and it doesn't exist, then we want to process the group. If we
                // do and it does, then check its filetime and set out output filetime
                // to be the earliest of the two (output or symbols)
                if (!IgnoreSourceMapOutput && symbolsFileInfo != null)
                {
                    var symbolsFileTime = symbolsFileInfo.LastWriteTimeUtc;
                    if (symbolsFileTime < outputFileTime)
                    {
                        outputFileTime = symbolsFileTime;
                    }
                }

                // first check the time of the manifest file itself. If it's newer than the output
                // time, then we need to process. Otherwise we need to check each input source file.
                // also rebuild if they're equal, just in case.
                if (manifestModifiedTime >= outputFileTime)
                {
                    // the manifest itself has been changed after the last output that was generated,
                    // so yes: we need to process this group.
                    processGroup = true;
                }
                else
                {
                    // check filetime of each input file, and if ANY one is newer, 
                    // then we will want to set the process-group flag and stop checking
                    foreach (var input in outputGroup.Inputs)
                    {
                        var fileInfo = new FileInfo(input.Path);
                        if (fileInfo.Exists)
                        {
                            // equal time also means process the output, because that is just SO close to
                            // the input being newer. Plus, someone might have the input be the output, so
                            // the times will be equal and we will want to process it.
                            if (fileInfo.LastWriteTimeUtc >= outputFileTime)
                            {
                                processGroup = true;
                                break;
                            }
                        }
                        else
                        {
                            // file doesn't exist -- check to see if it's a directory
                            var folderInfo = new DirectoryInfo(fileInfo.FullName);
                            if (folderInfo.Exists)
                            {
                                // not a FILE, it's a FOLDER of files.
                                // in order to specify an input folder, we need to have had the right type attribute
                                // on the output group so we know what kind of files to look for
                                if (codeType == CodeType.Unknown)
                                {
                                    // log an error, then bail because we won't be able to do anything anyway
                                    // since we don't know what kind of code we are processing and we don't know which
                                    // files to include from this folder.
                                    Log.LogError(Strings.DirectorySourceRequiresCodeType);
                                    return false;
                                }
                                else
                                {
                                    // recursively check all the files in the folder with the proper extension for the code type.
                                    // if anything pops positive, we know we want to process the group so bail early.
                                    processGroup = CheckFolderInputFileTimes(folderInfo, ExtensionsFromCodeType(codeType), outputFileTime);
                                    if (processGroup)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // do the same to any resource file, there are any (and we don't already know we
                // want to process this group)
                if (!processGroup && outputGroup.Resources.Count > 0)
                {
                    foreach (var resource in outputGroup.Resources)
                    {
                        var fileInfo = new FileInfo(resource.Path);
                        if (fileInfo.Exists && fileInfo.LastWriteTimeUtc >= outputFileTime)
                        {
                            processGroup = true;
                            break;
                        }
                    }
                }
            }

            return processGroup;
        }

        #endregion

        #region helper methods

        protected static TResult FileWriteOperation<TResult>(string filePath, ExistingFileTreatment treatment, Func<TResult> operation)
        {
            // the default value by default
            var result = default(TResult);

            // send output to file
            try
            {
                // make sure the destination folder exists
                var fileInfo = new FileInfo(filePath);
                var destFolder = new DirectoryInfo(fileInfo.DirectoryName);
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
                    // file exists, so doWrite will be false.
                    // if our clobber state is set to preserve, then we don't want to write.
                    if (treatment != ExistingFileTreatment.Preserve)
                    {
                        // determine read-only status
                        var isReadOnly = (File.GetAttributes(filePath) & FileAttributes.ReadOnly) != 0;
                        if (!isReadOnly && treatment != ExistingFileTreatment.Preserve)
                        {
                            // file exists, it's not read-only, and we don't have noclobber set.
                            // noclobber will never write over an existing file, but auto will
                            // write over an existing file that doesn't have read-only set.
                            doWrite = true;
                        }
                        else if (isReadOnly && treatment == ExistingFileTreatment.Overwrite)
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
                }

                if (doWrite && operation != null)
                {
                    result = operation();
                }
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            catch (UnauthorizedAccessException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            catch (PathTooLongException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            catch (SecurityException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            return result;
        }

        protected static UglifyCommandParser ParseConfigSettings(string arguments, UglifyCommandParser defaults)
        {
            // clone the default switch settings, parse the arguments on top of the clone,
            // and then return the clone.
            var switchParser = defaults.IfNotNull(d => d.Clone(), new UglifyCommandParser());
            switchParser.Parse(arguments);
            return switchParser;
        }

        static bool CheckFolderInputFileTimes(DirectoryInfo folderInfo, string extensions, DateTime outputFileTime)
        {
            // get all the files in this folder
            foreach (var fileInfo in folderInfo.GetFiles())
            {
                // check to see if .ext. is in the list of extensions. The trailing period is needed as an "end of extension"
                // marker, since an extension can't have a period anywhere but as the first character. So the list of extensions
                // will be period-delimited and end in a period.
                if (extensions.IndexOf(fileInfo.Extension.ToUpperInvariant() + '.', StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // extension is good -- check the input time. If it's later than the output,
                    // bail early because we now know we want to process the output group.
                    if (fileInfo.LastWriteTimeUtc >= outputFileTime)
                    {
                        return true;
                    }
                }
            }

            // then recurse any subfolders
            foreach (var subFolder in folderInfo.GetDirectories())
            {
                // bail early if anyting returns true (an input is more recent than the output)
                if (CheckFolderInputFileTimes(subFolder, extensions, outputFileTime))
                {
                    return true;
                }
            }

            // if we get here, nothing is newer
            return false;
        }

        static string ExtensionsFromCodeType(CodeType codeType)
        {
            // list of extensions ends in a period so we can search for .ext. and be sure
            // to not get any substrings. For instance, if we just has ".css" and searched
            // for .cs, we'd get a match. But if the list is ".css." and we search for ".cs."
            // then we wouldn't match.
            switch (codeType)
            {
                case CodeType.JavaScript:
                    return ".JS.";

                case CodeType.StyleSheet:
                    return ".CSS.";

                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}

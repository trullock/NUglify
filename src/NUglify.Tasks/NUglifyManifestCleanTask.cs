// NUglifyManifestCleanTask.cs
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
using System.Text;
using Microsoft.Build.Framework;
using NUglify.Helpers;

namespace NUglify
{
    /// <summary>
    /// Use this task to clean the outputs for a given set of Manifest files
    /// </summary>
    public class NUglifyManifestCleanTask : NUglifyManifestBaseTask
    {
        #region constructor

        public NUglifyManifestCleanTask()
        {
            // we don't want to throw errors if an input file doesn't exist -- we just want to delete 
            // all the output files.
            this.ThrowInputMissingErrors = false;
        }

        #endregion

        #region base task overrides 

        /// <summary>
        /// Process an output group by deleting the output files if they exist.
        /// </summary>
        /// <param name="outputGroup">the OutputGroup being processed</param>
        /// <param name="outputFileInfo">FileInfo for the desired output file</param>
        /// <param name="symbolsFileInfo">FileInfo for the optional desired symbol file</param>
        /// <param name="defaultSettings">default settings for this output group</param>
        /// <param name="manifestModifiedTime">modified time for the manifest</param>
        protected override void ProcessOutputGroup(OutputGroup outputGroup, FileInfo outputFileInfo, FileInfo symbolsFileInfo, UglifyCommandParser defaultSettings, DateTime manifestModifiedTime)
        {
            // get the settings to use -- take the configuration for this output group
            // and apply them over the default settings
            var settings = ParseConfigSettings(outputGroup.GetConfigArguments(this.Configuration), defaultSettings);

            // we really only care about the clobber setting -- if the file is read-only, don't bother deleting it
            // unless we have the clobber setting. If the current setting is Preserve, then we want to change it to
            // Auto because it makes no sense to not delete a non-readonly file during a "clean"
            var clobber = settings.Clobber == ExistingFileTreatment.Preserve 
                ? ExistingFileTreatment.Auto 
                : settings.Clobber;

            // we don't care about the inputs, we just want to delete the outputs and be done
            if (outputFileInfo != null)
            {
                if (!FileWriteOperation(outputFileInfo.FullName, clobber, () =>
                    {
                        outputFileInfo.IfNotNull(fi =>
                            {
                                if (fi.Exists)
                                {
                                    Log.LogMessage(MessageImportance.Normal, Strings.DeletingFile, fi.FullName);
                                    fi.Delete();
                                }
                            });
                        return true;
                    }))
                {
                    // can't delete the file - not an error; just informational
                    Log.LogMessage(MessageImportance.Normal, Strings.CouldNotDeleteOutputFile, outputFileInfo.FullName);
                }
            }

            if (symbolsFileInfo != null)
            {
                if (!FileWriteOperation(symbolsFileInfo.FullName, clobber, () =>
                    {
                        symbolsFileInfo.IfNotNull(fi =>
                            {
                                if (fi.Exists)
                                {
                                    Log.LogMessage(MessageImportance.Normal, Strings.DeletingFile, fi.FullName);
                                    fi.Delete();
                                }
                            });
                        return true;
                    }))
                {
                    // can't delete the file - not an error; just informational
                    Log.LogMessage(MessageImportance.Normal, Strings.CouldNotDeleteOutputFile, symbolsFileInfo.FullName);
                }
            }
        }

        protected override void GenerateJavaScript(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            // shouldn't get called because we override the ProcessOutputGroup method
            throw new NotImplementedException();
        }

        protected override void GenerateStyleSheet(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            // shouldn't get called because we override the ProcessOutputGroup method
            throw new NotImplementedException();
        }

        #endregion
    }
}

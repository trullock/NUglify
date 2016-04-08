// NUglifyBundleTask.cs
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

using System.Collections.Generic;
using System.IO;
using System.Text;
using NUglify.Helpers;

namespace NUglify
{
    /// <summary>
    /// Use this task to bundle input files to output files based on a Manifest file without processing the contents
    /// </summary>
    public class NUglifyBundleTask : NUglifyManifestBaseTask
    {
        #region constructor

        public NUglifyBundleTask()
        {
            // we don't care about the symbols file because we won't be creating one.
            // we're just going to bundle the files and not process them; we won't have
            // enough info to generate one, even if we wanted to.
            IgnoreSourceMapOutput = true;
        }

        #endregion

        protected override void GenerateJavaScript(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            // create the output file, clobbering any existing content
            if (!FileWriteOperation(outputPath, uglifyCommandParser.IfNotNull(p => p.Clobber), () =>
                {
                    using (var writer = new StreamWriter(outputPath, false, outputEncoding))
                    {
                        if (inputGroups != null && inputGroups.Count > 0)
                        {
                            // for each input file, copy to the output, separating them with a newline, a semicolon, and another newline
                            var addSeparator = false;
                            foreach (var inputGroup in inputGroups)
                            {
                                if (addSeparator)
                                {
                                    writer.WriteLine();
                                    writer.WriteLine(';');
                                }
                                else
                                {
                                    addSeparator = true;
                                }

                                writer.Write(inputGroup.Source);
                            }
                        }
                    }

                    return true;
                }))
            {
                // could not write file
                Log.LogError(Strings.CouldNotWriteOutputFile, outputPath);
            }
        }

        protected override void GenerateStyleSheet(OutputGroup outputGroup, IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, string outputPath, Encoding outputEncoding)
        {
            if (!FileWriteOperation(outputPath, uglifyCommandParser.IfNotNull(p => p.Clobber), () =>
                {
                    // create the output file, clobbering any existing content
                    using (var writer = new StreamWriter(outputPath, false, outputEncoding))
                    {
                        if (inputGroups != null && inputGroups.Count > 0)
                        {
                            // for each input file, copy to the output, separating them with a newline (don't need a semicolon like JavaScript does)
                            var addSeparator = false;
                            foreach (var inputGroup in inputGroups)
                            {
                                if (addSeparator)
                                {
                                    writer.WriteLine();
                                }
                                else
                                {
                                    addSeparator = true;
                                }

                                writer.Write(inputGroup.Source);
                            }
                        }
                    }

                    return true;
                }))
            {
                // could not write file
                Log.LogError(Strings.CouldNotWriteOutputFile, outputPath);
            }
        }
    }
}

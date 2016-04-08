// ManifestUtilities.cs
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NUglify.Helpers;

#if NET_20

namespace System.Runtime.CompilerServices
{
    // Summary:
    //     Indicates that a method is an extension method, or that a class or assembly
    //     contains extension methods.
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class ExtensionAttribute : Attribute
    {
        // Summary:
        //     Initializes a new instance of the System.Runtime.CompilerServices.ExtensionAttribute
        //     class.
        public ExtensionAttribute() { }
    }
}

#endif

namespace NUglify
{
    public static class ManifestUtilities
    {
        #region private static fields

        /// <summary>
        /// regular expression used to determine if a source file ends in a semicolon (optionally followed by whitespace)
        /// </summary>
        private static Regex s_endsInSemicolon = new Regex(@";\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion

        /// <summary>
        /// Read a manifest file from a file
        /// </summary>
        /// <param name="xmlPath">path to the file</param>
        /// <returns>nanifest read</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static Manifest ReadManifestFile(string xmlPath)
        {
            Manifest manifest = null;

            // create the file reader
            using (var fileReader = new StreamReader(xmlPath))
            {
                // create the xml reader from the file string using these settings
                var settings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                };

                using (var reader = XmlReader.Create(fileReader, settings))
                {
                    manifest = ManifestFactory.Create(reader);
                }
            }

            return manifest;
        }

        /// <summary>
        /// Normalize the various paths for the manifest given the optional input and optional output folders
        /// </summary>
        /// <param name="manifest">manifest object</param>
        /// <param name="manifestPath">relative input folder; use current folder if null</param>
        /// <param name="outputFolder">relative output folder; same as input folder if null</param>
        /// <param name="ignoreInput">whether to ignore input files or ensure their existance</param>
        public static void ValidateAndNormalize(this Manifest manifest, string inputFolder, string outputFolder, bool throwInputErrors)
        {
            if (manifest != null)
            {
                // if an input folder wasn't specified, use the system current directory
                inputFolder = inputFolder ?? Environment.CurrentDirectory;

                foreach (var outputGroup in manifest.Outputs)
                {
                    // if the output group doesn't have a path, then we'll just send the
                    // result to stdout.
                    if (!outputGroup.Path.IsNullOrWhiteSpace())
                    {
                        // normalize the output path if it's relative.
                        // if we were passed an output folder, we'll use that as the root folder, 
                        // otherwise use the manifest folder.
                        outputGroup.Path = NormalizePath(outputFolder, inputFolder, outputGroup.Path);

                        // if the type wasn't explicitly specified, see if we can glean that information
                        // from the output file's extension
                        if (outputGroup.CodeType == CodeType.Unknown)
                        {
                            outputGroup.CodeType = InferCodeType(outputGroup.Path);
                        }
                    }

                    // resolve all resource paths and validate file existence
                    foreach (var resource in outputGroup.Resources)
                    {
                        // resource path must exist
                        if (resource.Path.IsNullOrWhiteSpace())
                        {
                            // throw an error
                            throw new XmlException(ManifestStrings.MissingResourcePath);
                        }

                        // normalize relative only to the manifest, not the output folder
                        resource.Path = NormalizePath(null, inputFolder, resource.Path);

                        // if the resource isn't optional, then we need to test to see if
                        // it exists. If not, then that's an error
                        if (!resource.Optional)
                        {
                            if (!File.Exists(resource.Path) && throwInputErrors)
                            {
                                // throw an error
                                throw new FileNotFoundException(ManifestStrings.ResourceFileNotFound, resource.Path);
                            }
                        }
                    }

                    // resolve symbol map path.
                    // don't need to validate existence because it's an output file
                    var symbolMap = outputGroup.SymbolMap;
                    if (symbolMap != null)
                    {
                        if (symbolMap.Path.IsNullOrWhiteSpace())
                        {
                            // no path -- take the output path and just add ".map" to it. The output
                            // path should already be fully resolved.
                            symbolMap.Path = outputGroup.Path + ".map";
                        }
                        else
                        {
                            // resolve it using output folder and manifest folder.
                            symbolMap.Path = NormalizePath(outputFolder, inputFolder, symbolMap.Path);
                        }
                    }

                    foreach (var inputFile in outputGroup.Inputs)
                    {
                        // input path must exist (can't pull from stdin for manifests)
                        if (inputFile.Path.IsNullOrWhiteSpace())
                        {
                            // throw an error
                            throw new XmlException(ManifestStrings.MissingInputPath);
                        }

                        // normalize relative only to the manifest, not the output folder
                        inputFile.Path = NormalizePath(null, inputFolder, inputFile.Path);

                        // if the input file isn't optional, then we need to test to see if
                        // it exists. It's an error if it doesn't.
                        if (!inputFile.Optional)
                        {
                            // if it's not a file and it's not a folder...
                            if (!File.Exists(inputFile.Path) && !Directory.Exists(inputFile.Path) && throwInputErrors)
                            {
                                // throw an error
                                throw new FileNotFoundException(ManifestStrings.InputFileNotFound, inputFile.Path);
                            }
                        }

                        // if we don't know the type of the output yet, then see if we can glean 
                        // that information from the input files
                        if (outputGroup.CodeType == CodeType.Unknown)
                        {
                            outputGroup.CodeType = InferCodeType(inputFile.Path);

                            // if the code type is still undetermined and the path is a folder,
                            // then try looking at the files within
                            if (outputGroup.CodeType == CodeType.Unknown && Directory.Exists(inputFile.Path))
                            {
                                outputGroup.CodeType = InferFolderCodeType(inputFile.Path);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Given the current configuration, return the appropriate argument string
        /// </summary>
        /// <param name="manifest">manifest object</param>
        /// <param name="configuration">current configuration</param>
        /// <returns>argument string, or empty string if not found</returns>
        public static string GetConfigArguments(this Manifest manifest, string configuration)
        {
            return manifest == null ? string.Empty : GetConfigArguments(manifest.DefaultArguments, configuration);
        }

        /// <summary>
        /// Given the current configuration, return the appropriate argument string
        /// </summary>
        /// <param name="outputGroup">output group object</param>
        /// <param name="configuration">current configuration</param>
        /// <returns>argument string, or empty string if not found</returns>
        public static string GetConfigArguments(this OutputGroup outputGroup, string configuration)
        {
            return outputGroup == null ? string.Empty : GetConfigArguments(outputGroup.Arguments, configuration);
        }

        /// <summary>
        /// Create ResourceString objects from all the input resources for an output group and add them to a list
        /// </summary>
        /// <param name="outputGroup">output group</param>
        /// <param name="resourceStringList">resource strings list</param>
        /// <param name="defaultResourceObjectName">optional default resource object name</param>
        public static void ProcessResourceStrings(this OutputGroup outputGroup, IList<ResourceStrings> resourceStringList, string defaultResourceObjectName)
        {
            if (outputGroup != null && resourceStringList != null)
            {
                foreach (var resource in outputGroup.Resources)
                {
                    // create the resource strings object from the resources file.
                    var resourceStrings = ProcessResourceFile(resource.Path);

                    // if there is no name specified in the resource element, use the default.
                    resourceStrings.Name = resource.Name.IfNullOrWhiteSpace(defaultResourceObjectName);

                    // add it to the given list.
                    resourceStringList.Add(resourceStrings);
                }
            }
        }

        /// <summary>
        /// Walk the list of inputs for a given output group, and create a list of input groups
        /// consisting of the source and origin. Consecutive project inputs are concatenated together,
        /// and external inputs are kept separate.
        /// </summary>
        /// <param name="outputGroup">output group</param>
        /// <param name="defaultEncodingName">default encoding name to use if none specified</param>
        /// <param name="rawInputBuilder">output the raw source to this builder, no added comments</param>
        /// <param name="sourceLength">length of the raw source</param>
        /// <returns>list of input groups consisting of source code and origin</returns>
        public static IList<InputGroup> ReadInputGroups(this OutputGroup outputGroup, string defaultEncodingName)
        {
            var inputGroups = new List<InputGroup>();
            if (outputGroup != null)
            {
                if (outputGroup.Inputs.Count == 0)
                {
                    try
                    {
                        // try setting the input encoding - sometimes this fails!
                        Console.InputEncoding = GetInputEncoding(defaultEncodingName);
                    }
                    catch (IOException e)
                    {
                        // error setting the encoding input; just use whatever the default is
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }

                    var sourceComment = outputGroup.CodeType == CodeType.StyleSheet
                        ? "/*/#SOURCE 1 1 stdin */\r\n"
                        : "///#SOURCE 1 1 stdin\r\n";

                    // read from stdin, append to the builder, then create a single project input group
                    // from the build code.
                    var sourceCode = Console.In.ReadToEnd();
                    inputGroups.Add(new InputGroup
                        {
                            RawSource = sourceCode,
                            Source = sourceComment + sourceCode,
                            Origin = SourceOrigin.Project
                        });
                }
                else
                {
                    // start off saying the previous item ends in a semicolon so we don't add one before the first file
                    var fileReadBuilder = new FileReadBuilder { EndsInSemicolon = true };
                    for (var ndx = 0; ndx < outputGroup.Inputs.Count; ++ndx)
                    {
                        var inputFile = outputGroup.Inputs[ndx];
                        if (inputFile.Origin == SourceOrigin.External)
                        {
                            // we found an external input file.
                            // if we were building up project input files, dump what we have into a new group and clear it out
                            if (fileReadBuilder.Length > 0)
                            {
                                inputGroups.Add(new InputGroup
                                    {
                                        Source = fileReadBuilder.ToString(),
                                        RawSource = fileReadBuilder.ToRawString(),
                                        Origin = SourceOrigin.Project
                                    });
                            }

                            // read the content of the external file, passing in false for whether the previous file
                            // ended in a semicolon so that one will ALWAYS get prepended. Add a new input group for the
                            // content by itself, and then always reset the semicolon flag to false so that the NEXT code 
                            // will be separated from the external code with another semicolon.
                            fileReadBuilder.Clear();
                            ReadFile(inputFile, fileReadBuilder, outputGroup.CodeType, defaultEncodingName);
                            inputGroups.Add(new InputGroup
                                {
                                    Source = fileReadBuilder.ToString(),
                                    RawSource = fileReadBuilder.ToRawString(),
                                    Origin = SourceOrigin.External
                                });

                            fileReadBuilder.Clear();
                            fileReadBuilder.EndsInSemicolon = false;
                        }
                        else
                        {
                            // read the project input file into the group builder, with context.
                            ReadFile(inputFile, fileReadBuilder, outputGroup.CodeType, defaultEncodingName);
                        }
                    }

                    // if there is anything left, add it as a new input group now
                    if (fileReadBuilder.Length > 0)
                    {
                        inputGroups.Add(new InputGroup
                            {
                                Source = fileReadBuilder.ToString(),
                                RawSource = fileReadBuilder.ToRawString(),
                                Origin = SourceOrigin.Project
                            });
                    }
                }
            }

            return inputGroups;
        }

        /// <summary>
        /// Get an encoding to use for the given input file
        /// </summary>
        /// <param name="outputGroup">output file</param>
        /// <param name="defaultEncodingName">default encoding name to use if none specified</param>
        /// <returns>encoding; UTF8 WITHOUT THE BOM is the default if nothing else specified</returns>
        public static Encoding GetEncoding(this OutputGroup outputGroup, string defaultEncodingName)
        {
            Encoding encoding = null;
            if (outputGroup != null)
            {
                // if none specified on the output group, use the default
                var encodingName = outputGroup.EncodingName.IfNullOrWhiteSpace(defaultEncodingName);
                if (!encodingName.IsNullOrWhiteSpace())
                {
                    try
                    {
                        // try to create an encoding from the encoding name
                        // using special encoder fallback determined earlier, or a default
                        // encoder fallback that uses the UNICODE "replace character" for
                        // things it doesn't understand, and a decoder replacement fallback 
                        // that also uses the UNICODE "replacement character" for things it doesn't understand.
                        encoding = Encoding.GetEncoding(
                            encodingName,
                            GetEncoderFallback(outputGroup.CodeType),
                            new DecoderReplacementFallback("\uFFFD"));
                    }
                    catch (ArgumentException e)
                    {
                        // eat the exception and just go with UTF-8
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                }
            }

            // the default output is UTF-8 WITHOUT the BOM if we are outputting to a file,
            // or ASCII if we are outputting to STDOUT
            if (encoding == null)
            {
                if (outputGroup == null || outputGroup.Path.IsNullOrWhiteSpace())
                {
                    // no output group or outputting to stdout (no output path)
                    encoding = (Encoding)Encoding.ASCII.Clone();
                    encoding.EncoderFallback = GetEncoderFallback(outputGroup.IfNotNull(g => g.CodeType));
                }
                else
                {
                    // outputting to file, use UTF-8 WITHOUT the BOM.
                    // don't need a fallback encoder for UTF-8.
                    encoding = new UTF8Encoding(false);
                }
            }

            return encoding;
        }

        /// <summary>
        /// Get an encoding to use for the given input file
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="defaultEncodingName">default encoding name to use if none specified</param>
        /// <returns>encoding; UTF8 is the default if nothing else specified</returns>
        public static Encoding GetEncoding(this InputFile inputFile, string defaultEncodingName)
        {
            // if none specified on the output group, use the default
            var encodingName = inputFile != null && !inputFile.EncodingName.IsNullOrWhiteSpace()
                ? inputFile.EncodingName
                : defaultEncodingName;

            return GetInputEncoding(encodingName);
        }

        #region helper methods

        private static EncoderFallback GetEncoderFallback(CodeType codeType)
        {
            if (codeType == CodeType.JavaScript)
            {
                return new JsEncoderFallback();
            }
            else if (codeType == CodeType.StyleSheet)
            {
                return new CssEncoderFallback();
            }

            return new EncoderReplacementFallback("\uFFFD");
        }

        private static Encoding GetInputEncoding(string encodingName)
        {
            Encoding encoding = null;
            if (!encodingName.IsNullOrWhiteSpace())
            {
                try
                {
                    // try to create an encoding from the encoding name
                    // using encoder/decoder replacement fallbacks that use the UNICODE
                    // "replacement character" for things it doesn't understand.
                    encoding = Encoding.GetEncoding(
                        encodingName,
                        new EncoderReplacementFallback("\uFFFD"),
                        new DecoderReplacementFallback("\uFFFD"));
                }
                catch (ArgumentException e)
                {
                    // eat the exception and just go with UTF-8
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
            }

            return encoding ?? new UTF8Encoding();
        }

        private static CodeType InferCodeType(string path)
        {
            if (!path.IsNullOrWhiteSpace())
            {
                switch (Path.GetExtension(path).ToUpperInvariant())
                {
                    case ".JS":
                    case ".JSCRIPT":
                    case ".JAVASCRIPT":
                        // infer JavaScript
                        return CodeType.JavaScript;

                    case ".CSS":
                    case ".SCSS":
                    case ".SASS":
                        // infer stylesheet
                        return CodeType.StyleSheet;
                }
            }

            // nope; can't tell.
            return CodeType.Unknown;
        }

        private static CodeType InferFolderCodeType(string path)
        {
            if (!path.IsNullOrWhiteSpace())
            {
                // see if there are any matches for .JS or .CSS
                var hasJS = Directory.GetFiles(path, "*.js").Length > 0;
                var hasCSS = Directory.GetFiles(path, "*.css").Length > 0;

                // JS and no CSS -- assume this is a JS folder
                if (hasJS && !hasCSS) return CodeType.JavaScript;

                // CSS and no JS -- assume this is a CSS folder
                if (hasCSS && !hasJS) return CodeType.StyleSheet;
            }

            // otherwise we STILL don't know
            return CodeType.Unknown;
        }

        private static string NormalizePath(string outputFolder, string manifestFolder, string path)
        {
            // if we have a value and it's a relative path...
            if (!string.IsNullOrEmpty(path) && !Path.IsPathRooted(path))
            {
                if (string.IsNullOrEmpty(outputFolder))
                {
                    // no output folder; make it relative to the manifest file
                    path = Path.Combine(manifestFolder, path);
                }
                else
                {
                    // make it relative to the output folder
                    path = Path.Combine(outputFolder, path);
                }
            }

            return path;
        }

        private static string GetConfigArguments(IDictionary<string, string> configArguments, string configuration)
        {
            // try getting the current configuration
            string arguments = null;
            if (configArguments != null)
            {
                // if the current configuration is null or whitespace
                // OR if we try getting the current configuration and it failed...
                if (configuration.IsNullOrWhiteSpace() || !configArguments.TryGetValue(configuration, out arguments))
                {
                    // ...try the default (empty configuration)
                    configArguments.TryGetValue(string.Empty, out arguments);
                }
            }

            // make sure we don't return null
            return arguments ?? string.Empty;
        }

        #endregion

        #region file reading helpers

        private static void ReadFile(InputFile inputFile, FileReadBuilder fileReadBuilder, CodeType codeType, string defaultEncodingName)
        {
            if (inputFile != null)
            {
                var encoding = inputFile.GetEncoding(defaultEncodingName);
                if (File.Exists(inputFile.Path))
                {
                    // file exists -- read it into the string builder with the appropriate context comments added
                    ReadFileWithContext(fileReadBuilder, inputFile.Path, codeType, encoding);
                }
                else if (Directory.Exists(inputFile.Path))
                {
                    // right now, just look for .JS for JavaScript and .CSS for Stylesheets.
                    // if we don't know what type, ask for everything!
                    var searchPattern = codeType == CodeType.JavaScript
                        ? "*.js"
                        : codeType == CodeType.StyleSheet
                        ? "*.css"
                        : "*.*";

                    ReadAllFilesWithContext(fileReadBuilder, inputFile.Path, searchPattern, codeType, encoding);
                }
            }
        }

        private static void ReadAllFilesWithContext(FileReadBuilder fileReadBuilder, string folderPath, string searchPattern, CodeType codeType, Encoding encoding)
        {
            // get all the files in the folder path that match the search pattern
            foreach (var filePath in Directory.GetFiles(folderPath, searchPattern))
            {
                // read each one into the string builder with context
                ReadFileWithContext(fileReadBuilder, filePath, codeType, encoding);
            }
        }

        private static void ReadFileWithContext(FileReadBuilder fileReadBuilder, string filePath, CodeType codeType, Encoding encoding)
        {
            if (fileReadBuilder.Length > 0)
            {
                // start a new line so any previous single-line comments are terminated.
                fileReadBuilder.AnnotateLine();
            }

            // if the previous file didn't end in a semicolon, add one now.
            // it doesn't hurt to have an extra semicolon in JavaScript, and our CSS Parser has been
            // tweaked to ignore extraneous semicolons as well.
            if (!fileReadBuilder.EndsInSemicolon && codeType != CodeType.StyleSheet)
            {
                fileReadBuilder.Annotate(';');
            }

            // output a special comment that NUglify will pick up so any errors will 
            // have the proper file context. The CSS parser has been tweaked to look for
            // this comment as well.
            if (codeType == CodeType.StyleSheet)
            {
                fileReadBuilder.Annotate("/*/#source 1 1 ");
            }
            else
            {
                fileReadBuilder.Annotate("///#source 1 1 ");
            }

            fileReadBuilder.Annotate(filePath);
            if (codeType == CodeType.StyleSheet)
            {
                fileReadBuilder.Annotate(" */");
            }

            fileReadBuilder.AnnotateLine();

            // now read all the file source and add it to the combined input.
            // it doesn't matter which encoder fallback we use -- we'll be DECODING, and we always use a simple replacement for that.
            // so just ask for a JS encoding here.
            using (var reader = new StreamReader(filePath, encoding))
            {
                // read all the content, add it to the builder, and set the semicolon flag
                var fileContent = reader.ReadToEnd();
                fileReadBuilder.AppendContent(fileContent);
                fileReadBuilder.EndsInSemicolon = s_endsInSemicolon.IsMatch(fileContent);
            }
        }

        private class FileReadBuilder
        {
            private StringBuilder m_annotatedContent;
            private StringBuilder m_rawContent;

            public bool EndsInSemicolon { get; set; }
            public long Length { get { return m_rawContent.Length; } }

            public FileReadBuilder()
            {
                m_annotatedContent = new StringBuilder(8192);
                m_rawContent = new StringBuilder(8192);
            }

            public void AppendContent(string text)
            {
                m_annotatedContent.Append(text);
                m_rawContent.Append(text);
            }


            [Localizable(false)]
            public void Annotate(string text)
            {
                m_annotatedContent.Append(text);
            }

            public void Annotate(char character)
            {
                m_annotatedContent.Append(character);
            }

            public void AnnotateLine()
            {
                m_annotatedContent.AppendLine();
            }

            public void Clear()
            {
                // clear the builder, set the length to zero,
                // but leave the semicolon state as-is.
                m_annotatedContent.Length = 0;
                m_rawContent.Length = 0;
            }

            public string ToRawString()
            {
                return m_rawContent.ToString();
            }

            public override string ToString()
            {
                return m_annotatedContent.ToString();
            }
        }

        #endregion

        #region resource processing

        private static ResourceStrings ProcessResourceFile(string resourcePath)
        {
            // which method we call to process the resources depends on the file extension
            // of the resources path given to us.
            switch (Path.GetExtension(resourcePath).ToUpperInvariant())
            {
                case ".RESX":
                    // process the resource file as a RESX xml file
                    return ProcessResXResources(resourcePath);

                case ".RESOURCES":
                    // process the resource file as a compiles RESOURCES file
                    return ProcessResources(resourcePath);

                default:
                    // no other types are supported
                    throw new ArgumentException(ManifestStrings.InvalidResourcePath);
            }
        }

        private static ResourceStrings ProcessResources(string resourceFileName)
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

        private static ResourceStrings ProcessResXResources(string resourceFileName)
        {
            // default return object is null, meaning we are outputting the JS code directly
            // and don't want to replace any referenced resources in the sources
            var resourceStrings = new ResourceStrings();
            using (ResXResourceReader reader = new ResXResourceReader(resourceFileName))
            {
                foreach (DictionaryEntry item in reader)
                {
                    resourceStrings[item.Key.ToString()] = item.Value.ToString();
                }
            }

            return resourceStrings.Count == 0 ? null : resourceStrings;
        }

        #endregion
    }

    /// <summary>
    /// Class representing an input group, the source from a contiguous set of
    /// input elements with project origin, or a single external origin.
    /// </summary>
    public class InputGroup
    {
        public string Source { get; set; }
        
        public string RawSource { get; set; }

        public SourceOrigin Origin { get; set; }
    }
}

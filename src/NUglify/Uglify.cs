// Uglify.cs
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
using System.Globalization;
using System.IO;
using NUglify.Css;
using NUglify.Helpers;
using NUglify.Html;
using NUglify.JavaScript;
using NUglify.JavaScript.Visitors;

namespace NUglify
{
    /// <summary>
    /// Uglify class for quick minification of JavaScript or Stylesheet code without needing to
    /// access or modify any abstract syntax tree nodes. Just put in source code and get our minified
    /// code as strings.
    /// </summary>
    public sealed class Uglify
    {
	    static readonly HtmlSettings DefaultSettings = new HtmlSettings();

        // Don't use static class, as we don't expect to using static as the method names are already short (Js, Css)

        Uglify()
        {
        }

        /// <summary>
        /// Crunched HTML string passed to it, returning crunched string.
        /// </summary>
        /// <param name="source">source HTML</param>
        /// <param name="settings">HTML minification settings</param>
        /// <param name="sourceFileName">The source file name used when reporting errors. Default is <c>null</c></param>
        /// <returns>minified HTML</returns>
        public static UglifyResult Html(string source, HtmlSettings settings = null, string sourceFileName = null)
        {
            settings = settings ?? DefaultSettings;

            var parser = new HtmlParser(source, sourceFileName, settings);
            var document = parser.Parse();
            string text = null;

            var errors = new List<UglifyError>(parser.Errors);

            if (document != null)
            {
                var minifier = new HtmlMinifier(document, settings);
                minifier.Minify();

                errors.AddRange(minifier.Errors);

                var writer = new StringWriter();
                var htmlWriter = new HtmlWriterToHtml(writer, settings);
                htmlWriter.Write(document);
                text = writer.ToString();
            }

            return new UglifyResult(text, errors);
        }


        /// <summary>
        /// Extract the text from a HTML string.
        /// </summary>
        /// <param name="source">The source HTML</param>
        /// <param name="options">The options to extract the text.</param>
        /// <param name="sourceFileName">The source file name used when reporting errors. Default is <c>null</c></param>
        /// <returns>The text extracted from this HTML string</returns>
        public static UglifyResult HtmlToText(string source, HtmlToTextOptions options = HtmlToTextOptions.None, string sourceFileName = null)
        {
            // Use specific settings to extract text from html
            var settings = new HtmlSettings
            {
                RemoveOptionalTags = false,
                RemoveEmptyAttributes = false,
                RemoveAttributeQuotes = false,
                DecodeEntityCharacters = true,
                RemoveScriptStyleTypeAttribute = false,
                ShortBooleanAttribute = false,
                MinifyJs = false,
                MinifyJsAttributes = false,
                MinifyCss = false,
                MinifyCssAttributes = false
            };

            var parser = new HtmlParser(source, sourceFileName, settings);
            var document = parser.Parse();
            string text = null;

            var errors = new List<UglifyError>(parser.Errors);

            if (document != null)
            {
                var minifier = new HtmlMinifier(document, settings);
                minifier.Minify();

                errors.AddRange(minifier.Errors);

                var writer = new StringWriter();
                var htmlWriter = new HtmlWriterToText(writer, options);
                htmlWriter.Write(document);

                text = writer.ToString();
            }

            return new UglifyResult(text, errors);
        }

        /// <summary>
        /// Crunched JS string passed to it, returning crunched string.
        /// The ErrorList property will be set with any errors found during the minification process.
        /// </summary>
        /// <param name="source">source Javascript</param>
        /// <param name="codeSettings">code minification settings</param>
        /// <returns>minified Javascript</returns>
        public static UglifyResult Js(string source, CodeSettings codeSettings)
        {
            // just pass in default settings
            return Js(source, null, codeSettings);
        }

        /// <summary>
        /// Crunched JS string passed to it, returning crunched string.
        /// The ErrorList property will be set with any errors found during the minification process.
        /// </summary>
        /// <param name="source">source Javascript</param>
        /// <param name="fileName">File name to use in error reporting. Default is <c>input</c></param>
        /// <param name="codeSettings">code minification settings</param>
        /// <returns>minified Javascript</returns>
        public static UglifyResult Js(string source, string fileName = null, CodeSettings codeSettings = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            fileName = fileName ?? "input";
            codeSettings = codeSettings ?? new CodeSettings();

            // default is an empty string
            string crunched;

            // reset the errors builder
            var errorList = new List<UglifyError>();

            // create the parser and hook the engine error event
            var parser = new JSParser();
            parser.CompilerError += (sender, e) =>
            {
                var error = e.Error;
                if (error.Severity <= codeSettings.WarningLevel)
                {
                    errorList.Add(error);
                }
            };

            var sb = StringBuilderPool.Acquire();
            try
            {
                var preprocessOnly = codeSettings.PreprocessOnly;
                using (var stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture))
                {
                    if (preprocessOnly)
                    {
                        parser.EchoWriter = stringWriter;
                    }

                    // parse the input
                    var scriptBlock = parser.Parse(new DocumentContext(source) { FileContext = fileName }, codeSettings);
                    if (scriptBlock != null && !preprocessOnly)
                    {
                        // we'll return the crunched code
                        if (codeSettings.Format == JavaScriptFormat.JSON)
                        {
                            // we're going to use a different output visitor -- one
                            // that specifically returns valid JSON.
                            if (!JsonOutputVisitor.Apply(stringWriter, scriptBlock, codeSettings))
                            {
                                errorList.Add(new UglifyError()
                                    {
                                        Severity = 0,
                                        File = fileName,
                                        Message = CommonStrings.InvalidJSONOutput,
                                    });
                            }
                        }
                        else
                        {
                            // just use the normal output visitor
                            OutputVisitor.Apply(stringWriter, scriptBlock, codeSettings);

                            codeSettings.SymbolsMap?.EndFile(stringWriter, codeSettings.LineTerminator);
                        }
                    }
                }

                crunched = sb.ToString();
            }
            catch (Exception e)
            {
                errorList.Add(new UglifyError()
                    {
                        Severity = 0,
                        File = fileName,
                        Message = e.Message,
                    });
                throw;
            }
            finally
            {
                sb.Release();
            }

            return new UglifyResult(crunched, errorList);
        }

        public static UglifyResult Css(string source, CssSettings settings = null,
            CodeSettings scriptSettings = null)
        {
            return Css(source, null, settings, scriptSettings);
        }

        /// <summary>
        /// Minifies the CSS stylesheet passes to it using the given settings, returning the minified results
        /// The ErrorList property will be set with any errors found during the minification process.
        /// </summary>
        /// <param name="source">CSS Source</param>
        /// <param name="fileName">File name to use in error reporting. Default is <c>input</c></param>
        /// <param name="settings">CSS minification settings</param>
        /// <param name="scriptSettings">JS minification settings to use for expression-minification</param>
        /// <returns>Minified StyleSheet</returns>
        public static UglifyResult Css(string source, string fileName, CssSettings settings = null, CodeSettings scriptSettings = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            fileName = fileName ?? "input";
            settings = settings ?? new CssSettings();
            scriptSettings = scriptSettings ?? new CodeSettings();

            // initialize some values, including the error list (which shoudl start off empty)
            string minifiedResults;
            var errorList = new List<UglifyError>();

            // create the parser object and if we specified some settings,
            // use it to set the Parser's settings object
            var parser = new CssParser
            {
                FileContext = fileName,
                Settings = settings,
                JSSettings = scriptSettings
            };

            // hook the error handler
            parser.CssError += (sender, e) =>
            {
                var error = e.Error;
                if (error.Severity <= settings.WarningLevel)
                {
                    errorList.Add(error);
                }
            };

            // try parsing the source and return the results
            try
            {
                minifiedResults = parser.Parse(source);
            }
            catch (Exception e)
            {
                errorList.Add(new UglifyError()
                    {
                        Severity = 0,
                        File = fileName,
                        Message = e.Message,
                    });
                throw;
            }
            return new UglifyResult(minifiedResults, errorList);
        }
    }
}
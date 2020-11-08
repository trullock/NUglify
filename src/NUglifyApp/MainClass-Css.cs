// MainClass-Css.cs
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
using System.Globalization;
using System.IO;
using System.Text;
using NUglify.Css;

namespace NUglify
{
    public partial class MainClass
    {
        #region ProcessCssFile method

        int ProcessCssFile(IList<InputGroup> inputGroups, UglifyCommandParser uglifyCommandParser, StringBuilder outputBuilder)
        {
            var retVal = 0;

            // blank line before
            WriteProgress();

            // we can share the same parser object
            var parser = new CssParser();
            parser.Settings = uglifyCommandParser.CssSettings;
            parser.JSSettings = uglifyCommandParser.JSSettings;

            using (var writer = new StringWriter(outputBuilder, CultureInfo.InvariantCulture))
            {
                // if we are echoing the input, then set the settings echo writer to the output stream
                // otherwise make sure it's null
                if (this.m_echoInput)
                {
                    parser.EchoWriter = writer;
                }
                else
                {
                    parser.EchoWriter = null;
                }
                
                var ndx = 0;
                foreach (var inputGroup in inputGroups)
                {
                    // process input source...
                    parser.CssError += (sender, ea) =>
                    {
                        var error = ea.Error;
                        if (inputGroup.Origin == SourceOrigin.Project || error.Severity == 0)
                        {
                            // ignore severity values greater than our severity level
                            if (error.Severity <= uglifyCommandParser.WarningLevel)
                            {
                                // we found an error
                                m_errorsFound = true;

                                WriteError(error.ToString());
                            }
                        }
                    };

                    if (m_echoInput && ndx > 0)
                    {
                        writer.Write(uglifyCommandParser.CssSettings.LineTerminator);
                    }

                    // if we want to time this, start a stopwatch now
                    Stopwatch stopwatch = null;
                    if (m_outputTimer)
                    {
                        stopwatch = new Stopwatch();
                        stopwatch.Start();
                    }

                    // crunch the source and output to the string builder we were passed
                    var crunchedStyles = parser.Parse(inputGroup.Source);

                    if (stopwatch != null)
                    {
                        var ticks = stopwatch.ElapsedTicks;
                        stopwatch.Stop();

                        // frequency is ticks per second, so if we divide by 1000.0, then we will have a
                        // double-precision value indicating the ticks per millisecond. Divide this into the
                        // number of ticks we measure, and we'll get the milliseconds in double-precision.
                        var frequency = Stopwatch.Frequency / 1000.0;
                        var timerMessage = string.Format(CultureInfo.CurrentCulture, NUglify.TimerFormat, 0, ticks / frequency);

                        Debug.WriteLine(timerMessage);
                        Debug.WriteLine(string.Empty);
                        WriteProgress(timerMessage);
                        WriteProgress();
                    }

                    // if there is output, send it where it needs to go
                    if (!string.IsNullOrEmpty(crunchedStyles))
                    {
                        if (!m_echoInput)
                        {
                            if (ndx++ > 0)
                            {
                                // separate input group outputs with an appropriate newline
                                writer.Write(uglifyCommandParser.CssSettings.LineTerminator);
                            }

                            writer.Write(crunchedStyles);
                        }
                    }
                }
            }

            return retVal;
        }

        #endregion
    }
}

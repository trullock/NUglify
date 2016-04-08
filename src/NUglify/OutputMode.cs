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
namespace NUglify
{
    /// <summary>
    /// Output mode setting
    /// </summary>
    public enum OutputMode
    {
        /// <summary>
        /// Output the minified code on a single line for maximum minification.
        /// LineBreakThreshold may still break the single line into multiple lines
        /// at a syntactically correct point after the given line length is reached.
        /// Not easily human-readable.
        /// </summary>
        SingleLine,

        /// <summary>
        /// Output the minified code on multiple lines to increase readability
        /// </summary>
        MultipleLines,

        /// <summary>
        /// Supress code output; typically used for linting or analysis of source code
        /// </summary>
        None
    }
}
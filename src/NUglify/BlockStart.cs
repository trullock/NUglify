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
    /// Describes how to output the opening curly-brace for blocks when the OutputMode
    /// is set to MultipleLines. 
    /// </summary>
    public enum BlockStart
    {
        /// <summary>
        /// Output the opening curly-brace block-start character on its own new line. Ex:
        /// if (condition)
        /// {
        ///     ...
        /// }
        /// </summary>
        NewLine = 0,

        /// <summary>
        /// Output the opening curly-brace block-start character at the end of the previous line. Ex:
        /// if (condition) {
        ///     ...
        /// }
        /// </summary>
        SameLine,

        /// <summary>
        /// Output the opening curly-brace block-start character on the same line or a new line
        /// depending on how it was specified in the sources. 
        /// </summary>
        UseSource
    }
}
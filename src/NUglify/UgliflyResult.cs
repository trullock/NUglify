// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System.Collections.Generic;

namespace NUglify
{
    /// <summary>
    /// Results of a <see cref="Uglify.Css(string,NUglify.Css.CssSettings,NUglify.JavaScript.CodeSettings)"/> or 
    /// <see cref="Uglify.Js(string,NUglify.JavaScript.CodeSettings)"/> operation.
    /// </summary>
    public struct UgliflyResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UgliflyResult"/> struct.
        /// </summary>
        /// <param name="code">The uglified code.</param>
        /// <param name="errors">The errors.</param>
        public UgliflyResult(string code, List<UglifyError> errors)
        {
            Code = code;
            Errors = errors;
        }

        /// <summary>
        /// Gets the the uglified code. May ne null if <see cref="HasErrors"/> is <c>true</c>.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        public bool HasErrors => Errors != null && Errors.Count > 0;

        /// <summary>
        /// Gets the errors. Empty if no errors.
        /// </summary>
        public List<UglifyError> Errors { get; }
    }
}
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
    public struct UglifyResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UglifyResult"/> struct.
        /// </summary>
        /// <param name="code">The uglified code.</param>
        /// <param name="messages">The Messages.</param>
        public UglifyResult(string code, List<UglifyError> messages)
        {
            Code = code;
            Errors = messages;
            HasErrors = false;
            if (messages != null)
            {
                foreach (var error in messages)
                {
                    if (error.IsError)
                    {
                        HasErrors = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the the uglified code. May be null if <see cref="HasErrors"/> is <c>true</c>.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has Messages.
        /// </summary>
        public bool HasErrors { get; }

        /// <summary>
        /// Gets the Messages. Empty if no Messages.
        /// </summary>
        public List<UglifyError> Errors { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}

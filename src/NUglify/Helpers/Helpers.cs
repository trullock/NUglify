// Helpers.cs
//
// Copyright 2012 Microsoft Corporation
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

using System.Collections;
using System.Globalization;
using NUglify.Css;
using NUglify.JavaScript;

namespace NUglify.Helpers
{
#if !CORE
    public static class ErrorStringHelper
    {
        public static System.Collections.Generic.IEnumerable<string> AvailableCssStrings
        {
            get
            {
                var resources = CssStrings.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry property in resources)
                {
                    yield return property.Key.ToString();
                }
            }
        }

        public static System.Collections.Generic.IEnumerable<string> AvailableJSStrings
        {
            get
            {
                var resources = JScript.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry property in resources)
                {
                    yield return property.Key.ToString();
                }
            }
        }
    }
#endif
}

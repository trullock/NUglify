// AssemblyInfo.cs
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

using System.Reflection;
using System.Resources;
using System.Security;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("Ajax Uglify MSBuild Task")]
[assembly: AssemblyDescription("JavaScript and CSS minification MSBuild Task")]
[assembly: AssemblyCulture("")]

// minimum permission (execute) and nothing optional
//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
//[assembly: PermissionSet(SecurityAction.RequestOptional, Name = "Nothing")]
//[assembly: SecurityCritical(SecurityCriticalScope.Explicit)]
//[assembly: AllowPartiallyTrustedCallers]

// we are compliant and not visible to COM by default
[assembly: System.Runtime.InteropServices.ComVisible(false)]

// Set neutral resources language for assembly.
[assembly: NeutralResourcesLanguage("en")]

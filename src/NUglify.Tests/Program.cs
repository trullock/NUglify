// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;

namespace NUglify.Tests
{
    public class Program
    {
        public static void Main()
        {
            // JavaScript sample
            {
                var result = Uglify.Js("var x = 5; var y = 6;");
                Console.WriteLine(result.Code);
            }

            // Css sample
            {
                var result = Uglify.Css("div { color: #FFF; }");
                Console.WriteLine(result.Code); // 
            }
        }
    }
}

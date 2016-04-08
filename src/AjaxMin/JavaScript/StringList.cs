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

namespace NUglify.JavaScript
{
    /// <summary>
    /// Array of strings which can be converted to string using a single allocation and single copy
    /// </summary>
    public class StringList
    {
        private string[] m_strings;

        private void Add(StringList source, ref int pos)
        {
            for (var i = 0; i < source.m_strings.Length; i ++)
            {
                m_strings[pos++] = source.m_strings[i];
            }
        }

        public StringList(object left, object right)
        {
            var list1 = left  as StringList;
            var list2 = right as StringList;

            var len = 1;

            if (list1 != null)
            {
                len = list1.m_strings.Length;
            }
            
            if (list2 != null)
            {
                len += list2.m_strings.Length;
            }
            else
            {
                len++;
            }

            m_strings = new string[len];

            len = 0;

            if (list1 != null)
            {
                Add(list1, ref len);
            }
            else if (left != null)
            {
                m_strings[len++] = left.ToString();
            }

            if (list2 != null)
            {
                Add(list2, ref len);
            }
            else if (right != null)
            {
                m_strings[len++] = right.ToString();
            }
        }

        public override string ToString()
        {
            return String.Concat(m_strings);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DllUnitTest
{
    public class TestBuildEngine : IBuildEngine
    {
        public List<string> LogMessages { get; private set; }

        public string MockProjectPath { get; set; }

        public TestBuildEngine()
        {
            LogMessages = new List<string>();
        }

        public int NumErrors { get; private set; }

        public int NumWarnings { get; private set; }

        #region IBuildEngine Members

        public bool BuildProjectFile(string projectFileName, string[] targetNames, System.Collections.IDictionary globalProperties, System.Collections.IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public int ColumnNumberOfTaskNode
        {
            get { return 0; }
        }

        public bool ContinueOnError
        {
            get { return true; }
        }

        public int LineNumberOfTaskNode
        {
            get { return 0; }
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            LogMessages.Add("C " + e.Message);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            ++NumErrors;
            LogMessages.Add(FormatMessage(true, e.File, e.LineNumber, e.EndLineNumber, e.ColumnNumber, e.EndColumnNumber, e.Code, e.Subcategory, e.Message));
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            LogMessages.Add("? " + e.Message);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            ++NumWarnings;
            LogMessages.Add(FormatMessage(false, e.File, e.LineNumber, e.EndLineNumber, e.ColumnNumber, e.EndColumnNumber, e.Code, e.Subcategory, e.Message));
        }

        public string ProjectFileOfTaskNode
        {
            get { return MockProjectPath; }
        }

        #endregion

        #region helper methods

        private string FormatMessage(bool isError, string file, int lineStart, int lineEnd, int columnStart, int columnEnd, string code, string subcat, string message)
        {
            var sb = new StringBuilder();

            // prepend with * for errors and ! for warnings.
            sb.Append(isError ? "* " : "! ");

            if (!string.IsNullOrWhiteSpace(file))
            {
                sb.Append(file);
            }

            if (lineStart > 0)
            {
                // we will always at least start with the start line
                sb.AppendFormat("({0}", lineStart);

                if (lineEnd > lineStart)
                {
                    if (columnStart > 0 && columnStart > 0)
                    {
                        // all four values were specified
                        sb.AppendFormat(",{0},{1},{2}", columnStart, lineEnd, columnEnd);
                    }
                    else
                    {
                        // one or both of the columns wasn't specified, so ignore them both
                        sb.AppendFormat("-{0}", lineEnd);
                    }
                }
                else if (columnStart > 0)
                {
                    sb.AppendFormat(",{0}", columnStart);
                    if (columnEnd > columnStart)
                    {
                        sb.AppendFormat("-{0}", columnEnd);
                    }
                }

                sb.Append(')');
            }

            // seaprate the location from the error description
            sb.Append(':');

            // if there is a subcategory, add it prefaced with a space
            if (!string.IsNullOrEmpty(subcat))
            {
                sb.Append(' ');
                sb.Append(subcat);
            }

            // not localizable
            sb.Append(isError ? " error " : " warning ");

            // if there is an error code
            if (!string.IsNullOrEmpty(code))
            {
                sb.Append(code);
            }

            // separate description from the message
            sb.Append(": ");

            if (!string.IsNullOrEmpty(message))
            {
                sb.Append(message);
            }

            return sb.ToString();
        }

        #endregion
    }
}

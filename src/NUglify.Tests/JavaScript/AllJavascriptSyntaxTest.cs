using NUglify.Tests.JavaScript.Common;
using NUnit.Framework;

namespace NUglify.Tests.JavaScript
{
	[TestFixture]
	public class AllJavascriptSyntaxTest
	{
		/// <summary>
		/// check all possible files in input-directory for syntax errors after minification
		/// </summary>
		[Test]
		public void SyntaxTestForAllFilesLineBreaks() {
			TestHelper.Instance.RunSyntaxTestForAllFilesLineBreaks();
		}
	}
}
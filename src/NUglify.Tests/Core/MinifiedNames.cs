using System.Diagnostics;
using NUglify.JavaScript;
using NUnit.Framework;

namespace NUglify.Tests.Core
{
    /// <summary>
    /// Summary description for MinifiedNames
    /// </summary>
    [TestFixture]
    public class MinifiedNames
    {
        [Test]
        public void KnownNames()
        {
            string name;
            for (var ndx = 0; (name = CrunchEnumerator.GenerateNameFromNumber(ndx)).Length < 4; ++ndx)
            {
                Trace.WriteLine(string.Format("{0}: {1}", ndx, name));
            }

            Assert.That(CrunchEnumerator.GenerateNameFromNumber(0), Is.EqualTo("n"), "name for 0");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(1), Is.EqualTo("t"), "name for 1");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(20), Is.EqualTo("g"), "name for last one-digit");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(21), Is.EqualTo("nt"), "name for first roll-over");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(22), Is.EqualTo("tt"), "name for first roll-over + 1");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(23), Is.EqualTo("it"), "name for first roll-over + 2");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(461), Is.EqualTo("gn"), "name for last two-digit");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(462), Is.EqualTo("ntt"), "name for first three-digit");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(9722), Is.EqualTo("gnn"), "name of last three-digit");
            Assert.That(CrunchEnumerator.GenerateNameFromNumber(9723), Is.EqualTo("nttt"), "name for first four-digit");
        }
    }
}

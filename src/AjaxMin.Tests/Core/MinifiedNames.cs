using System.Diagnostics;
using AjaxMin.JavaScript;
using NUnit.Framework;

namespace AjaxMin.Tests.Core
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

            Assert.AreEqual("n", CrunchEnumerator.GenerateNameFromNumber(0), "name for 0");
            Assert.AreEqual("t", CrunchEnumerator.GenerateNameFromNumber(1), "name for 1");
            Assert.AreEqual("g", CrunchEnumerator.GenerateNameFromNumber(20), "name for last one-digit");
            Assert.AreEqual("nt", CrunchEnumerator.GenerateNameFromNumber(21), "name for first roll-over");
            Assert.AreEqual("tt", CrunchEnumerator.GenerateNameFromNumber(22), "name for first roll-over + 1");
            Assert.AreEqual("it", CrunchEnumerator.GenerateNameFromNumber(23), "name for first roll-over + 2");
            Assert.AreEqual("gn", CrunchEnumerator.GenerateNameFromNumber(461), "name for last two-digit");
            Assert.AreEqual("ntt", CrunchEnumerator.GenerateNameFromNumber(462), "name for first three-digit");
            Assert.AreEqual("gnn", CrunchEnumerator.GenerateNameFromNumber(9722), "name of last three-digit");
            Assert.AreEqual("nttt", CrunchEnumerator.GenerateNameFromNumber(9723), "name for first four-digit");
        }
    }
}

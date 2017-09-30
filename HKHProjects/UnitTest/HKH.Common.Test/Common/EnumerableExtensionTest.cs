using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common;

namespace HKH.Common.Test
{
    [TestClass]
    public class EnumerableExtensionTest
    {
        private class TempCls
        {
            public TempCls(string ss)
            {
                SS = ss;
            }

            public string SS { get; set; }
        }

        [TestMethod]
        public void DoTest()
        {
            IList<TempCls> l1 = new List<TempCls> { new TempCls("aa"), new TempCls("bbb"), new TempCls("cc") };
            IList<TempCls> l2 = new List<TempCls> { new TempCls("dd"), new TempCls("eee") };

            l1.Do(t => t.SS = t.SS + "1");
            Assert.AreEqual(l1[0].SS, "aa1");
            Assert.AreEqual(l1[2].SS,"cc1");

            l2.Do(t => t.SS.Length > 2, t => t.SS = t.SS + "2");
            Assert.AreEqual(l2[0].SS, "dd");
            Assert.AreEqual(l2[1].SS, "eee2");
        }

		[TestMethod]
        public void RemoveTest()
        {
            IList<string> l1 = new List<string> { "aa", "bbb", "cc" };

            l1.Remove(s => s.Length < 3);
            Assert.AreEqual(l1.Count, 1);
            Assert.AreEqual(l1[0], "bbb");
        }

		[TestMethod]
		public void ConcatTest()
        {
            IList<string> l1 = new List<string> { "aa", "bbb", "cc" };
            IList<string> l2 = new List<string> { "dd", "eee" };
            IList<string> l3 = new List<string> { "ff" };

            IEnumerable<string> le = l1.Concat(l2, l3);

            Assert.AreEqual(le.Count(), 6);
            Assert.AreEqual(le.FirstOrDefault(), "aa");
            Assert.AreEqual(le.ElementAt(3), "dd");
            Assert.AreEqual(le.ElementAt(5), "ff");

            le = l1.Concat(s => s.Length == 2, l2, l3);

            Assert.AreEqual(le.Count(), 4);
            Assert.AreEqual(le.FirstOrDefault(), "aa");
            Assert.AreEqual(le.ElementAt(2), "dd");
            Assert.AreEqual(le.ElementAt(3), "ff");
        }
    }
}

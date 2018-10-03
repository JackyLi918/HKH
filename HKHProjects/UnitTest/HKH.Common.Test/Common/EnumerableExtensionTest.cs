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

        [TestMethod]
        public void OrderByTest()
        {
            List<Test1> lst = new List<Test1>();
            lst.Add(new Test1 { Name = "aaa", Age = 3, Birthday = new DateTime(2000, 1, 1), T2 = new Test2 { Age = 1 } });
            lst.Add(new Test1 { Name = "bbb", Age = 1, Birthday = new DateTime(3000, 1, 1), T2 = new Test2 { Age = 2 } });
            lst.Add(new Test1 { Name = "ccc", Age = 2, Birthday = new DateTime(1000, 1, 1), T2 = new Test2 { Age = 3 } });

            lst = lst.OrderBy("Name", false).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.OrderBy("Name", true).ToList();
            Assert.AreEqual("ccc", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("aaa", lst[2].Name);

            lst = lst.OrderBy("Age", false).ToList();
            Assert.AreEqual("bbb", lst[0].Name);
            Assert.AreEqual("ccc", lst[1].Name);
            Assert.AreEqual("aaa", lst[2].Name);

            lst = lst.OrderBy("Age", true).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("ccc", lst[1].Name);
            Assert.AreEqual("bbb", lst[2].Name);

            lst = lst.OrderBy("Birthday", false).ToList();
            Assert.AreEqual("ccc", lst[0].Name);
            Assert.AreEqual("aaa", lst[1].Name);
            Assert.AreEqual("bbb", lst[2].Name);

            lst = lst.OrderBy("Birthday", true).ToList();
            Assert.AreEqual("bbb", lst[0].Name);
            Assert.AreEqual("aaa", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.OrderBy("T2.Age", false).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.OrderBy("T2.Age", true).ToList();
            Assert.AreEqual("ccc", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("aaa", lst[2].Name);
        }

        [TestMethod]
        public void ThenByTest()
        {
            List<Test1> lst = new List<Test1>();
            lst.Add(new Test1 { Name = "aaa", Age = 3, Birthday = new DateTime(1000, 1, 1), T2 = new Test2 { Age = 1 } });
            lst.Add(new Test1 { Name = "bbb", Age = 1, Birthday = new DateTime(3000, 1, 1), T2 = new Test2 { Age = 2 } });
            lst.Add(new Test1 { Name = "aaa", Age = 2, Birthday = new DateTime(2000, 1, 1), T2 = new Test2 { Age = 3 } });

            IOrderedEnumerable<Test1> ordLst = lst.OrderBy("Name");

            lst = ordLst.ThenBy("Age", false).ToList();
            Assert.AreEqual(2, lst[0].Age);
            Assert.AreEqual(3, lst[1].Age);
            Assert.AreEqual(1, lst[2].Age);

            lst = ordLst.ThenBy("Age", true).ToList();
            Assert.AreEqual(3, lst[0].Age);
            Assert.AreEqual(2, lst[1].Age);
            Assert.AreEqual(1, lst[2].Age);

            lst = ordLst.ThenBy("Birthday", false).ToList();
            Assert.AreEqual(new DateTime(1000, 1, 1), lst[0].Birthday);
            Assert.AreEqual(new DateTime(2000, 1, 1), lst[1].Birthday);
            Assert.AreEqual(new DateTime(3000, 1, 1), lst[2].Birthday);

            lst = ordLst.ThenBy("Birthday", true).ToList();
            Assert.AreEqual(new DateTime(2000, 1, 1), lst[0].Birthday);
            Assert.AreEqual(new DateTime(1000, 1, 1), lst[1].Birthday);
            Assert.AreEqual(new DateTime(3000, 1, 1), lst[2].Birthday);

            lst = ordLst.ThenBy("T2.Age", false).ToList();
            Assert.AreEqual(1, lst[0].T2.Age);
            Assert.AreEqual(3, lst[1].T2.Age);
            Assert.AreEqual(2, lst[2].T2.Age);

            lst = ordLst.ThenBy("T2.Age", true).ToList();
            Assert.AreEqual(3, lst[0].T2.Age);
            Assert.AreEqual(1, lst[1].T2.Age);
            Assert.AreEqual(2, lst[2].T2.Age);
        }
    }
}

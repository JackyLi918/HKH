using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKH.Common.Test
{
    [TestClass]
    public class QueryableExtensionTest
    {
        [TestMethod]
        public void OrderByTest()
        {
            List<Test1> lst = new List<Test1>();
            lst.Add(new Test1 { Name = "aaa", Age = 3, Birthday = new DateTime(2000, 1, 1), T2 = new Test2 { Age = 1 } });
            lst.Add(new Test1 { Name = "bbb", Age = 1, Birthday = new DateTime(3000, 1, 1), T2 = new Test2 { Age = 2 } });
            lst.Add(new Test1 { Name = "ccc", Age = 2, Birthday = new DateTime(1000, 1, 1), T2 = new Test2 { Age = 3 } });

            var t = lst.AsQueryable<Test1>().OrderBy("Name", false);
            
            lst = lst.AsQueryable<Test1>().OrderBy("Name", false).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("Name", true).ToList();
            Assert.AreEqual("ccc", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("aaa", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("Age", false).ToList();
            Assert.AreEqual("bbb", lst[0].Name);
            Assert.AreEqual("ccc", lst[1].Name);
            Assert.AreEqual("aaa", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("Age", true).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("ccc", lst[1].Name);
            Assert.AreEqual("bbb", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("Birthday", false).ToList();
            Assert.AreEqual("ccc", lst[0].Name);
            Assert.AreEqual("aaa", lst[1].Name);
            Assert.AreEqual("bbb", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("Birthday", true).ToList();
            Assert.AreEqual("bbb", lst[0].Name);
            Assert.AreEqual("aaa", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("T2.Age", false).ToList();
            Assert.AreEqual("aaa", lst[0].Name);
            Assert.AreEqual("bbb", lst[1].Name);
            Assert.AreEqual("ccc", lst[2].Name);

            lst = lst.AsQueryable<Test1>().OrderBy("T2.Age", true).ToList();
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

            IOrderedQueryable<Test1> ordLst = lst.AsQueryable().OrderBy("Name");
  
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

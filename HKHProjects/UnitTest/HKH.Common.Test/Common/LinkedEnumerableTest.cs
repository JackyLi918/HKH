using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common;

namespace HKH.Common.Test
{
    [TestClass]
    public class LinkedEnumerableTest
    {
		[TestMethod]
        public void LengthTest()
        {
            IList<string> l1 = new List<string> { "aa", "bb", "cc" };
            IList<string> l2 = new List<string> { "dd", "ee" };
            IList<string> l3 = new List<string> { "ff" };

            LinkedList<string> le = new LinkedList<string>(l1, l2, l3);

            Assert.AreEqual(le.Count(), 6);
            Assert.AreEqual(le.ElementAt<string>(0), "aa");
            Assert.AreEqual(le.ElementAt<string>(3), "dd");
            Assert.AreEqual(le.ElementAt<string>(5), "ff");
        }
    }
}

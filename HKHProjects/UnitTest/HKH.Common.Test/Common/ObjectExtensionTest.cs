using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKH.Common.Test
{
	[TestClass]
	public class ObjectExtensionTest
	{
		[TestMethod]
		public void GetValueTest()
		{
			Test1 t1 = new Test1();
			Test2 t2 = new Test2();

			object val1 = t1.GetValue("Name");
			object val2 = t2.GetValue("Name");
			Assert.IsNull(val1);
			Assert.IsNull(val2);

			val1 = t1.GetValue("Age");
			val2 = t2.GetValue("Age");
			Assert.AreEqual(0, val1);
			Assert.AreEqual(0, val2);

			val1 = t1.GetValue("Birthday");
			val2 = t1.GetValue("Birthday");
			Assert.AreEqual(DateTime.MinValue, val1);
			Assert.AreEqual(DateTime.MinValue, val2);

			t1.Name = "aaa";
			t1.Age = 10;
			t1.Birthday = new DateTime(1921, 7, 1);
			t2.Name = "bbb";
			t2.Age = 20;
			t2.Birthday = new DateTime(1949, 10, 1);

			val1 = t1.GetValue("Name");
			val2 = t2.GetValue("Name");
			Assert.AreEqual("aaa", val1);
			Assert.AreEqual("bbb", val2);

			val1 = t1.GetValue("Age");
			val2 = t2.GetValue("Age");
			Assert.AreEqual(10, val1);
			Assert.AreEqual(20, val2);

			val1 = t1.GetValue("Birthday");
			val2 = t2.GetValue("Birthday");
			Assert.AreEqual(new DateTime(1921, 7, 1), val1);
			Assert.AreEqual(new DateTime(1949, 10, 1), val2);

			t1.T2 = t2;
			val1 = t1.GetValue("T2.Name");
			val2 = t1.GetValue("T2.Age");
			Assert.AreEqual("bbb", val1);
			Assert.AreEqual(20, val2);

			List<string> lst = ObjectExtension.DumpCache(true, false);
		}

		[TestMethod]
		public void SetValueTest()
		{
			Test1 t1 = new Test1();
			Test2 t2 = new Test2();

			t1.SetValue("Name", "ccc");
			t1.SetValue("Age", 10);
			t1.SetValue("Birthday", new DateTime(1921, 7, 1));
			t2.SetValue("Name", "ddd");
			t2.SetValue("Age", 20);
			t2.SetValue("Birthday", new DateTime(1949, 10, 1));
			Assert.AreEqual("ccc", t1.Name);
			Assert.AreEqual(10, t1.Age);
			Assert.AreEqual(new DateTime(1921, 7, 1), t1.Birthday);
			Assert.AreEqual("ddd", t2.Name);
			Assert.AreEqual(20, t2.Age);
			Assert.AreEqual(new DateTime(1949, 10, 1), t2.Birthday);

			t1.T2 = t2;
			t1.SetValue("T2.Name", "eee");
			t1.SetValue("T2.Age", 30);
			t1.SetValue("T2.Birthday", new DateTime(1997, 7, 1));
			Assert.AreEqual("eee", t1.T2.Name);
			Assert.AreEqual(30, t1.T2.Age);
			Assert.AreEqual(new DateTime(1997, 7, 1), t1.T2.Birthday);

			List<string> lst = ObjectExtension.DumpCache(false, true);

			t1.SetValue("Age", "", true);
			Assert.AreEqual(0, t1.Age);
		}
	}

	class Test1
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public DateTime Birthday { get; set; }
		public Test2 T2 { get; set; }
	}

	class Test2
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public DateTime Birthday { get; set; }
	}
}

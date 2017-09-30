using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common;

namespace HKH.Common.Test
{
	[TestClass]
	public class PropertyCompareTest
	{
		[TestMethod]
		public void EqualTest()
		{
			pcTest pt1 = new pcTest { Name = "1" };
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>("Name");
			Assert.AreEqual(pcCompare.Compare(pt1, pt1), 0);
		}

		[TestMethod]
		public void CompareTest()
		{
			pcTest pt1 = new pcTest { Name = "1" };
			pcTest pt2 = new pcTest { Name = "2" };
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>("Name");
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) < 0);
		}

		[TestMethod]
		public void CompareOfTTest()
		{
			pcTest pt1 = new pcTest { PP = new pp { Name = "1" } };
			pcTest pt2 = new pcTest { PP = new pp { Name = "2" } };
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>("PP");
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) < 0);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ExceptionTest1()
		{
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>(t=>t.Field);
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void ExceptionTest2()
		{
			pcTest pt1 = new pcTest { PC = new pc { Name = "1" } };
			pcTest pt2 = new pcTest { PC = new pc { Name = "2" } };
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>("PC");
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) < 0);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullExceptionTest()
		{
			pcTest pt1 = new pcTest { PC = new pc { Name = "1" } };
			pcTest pt2 = new pcTest { PC = new pc { Name = "2" } };
			PropertyCompare<pcTest> pcCompare = new PropertyCompare<pcTest>();
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) < 0);
		}

		[TestMethod]
		public void CompareListTest()
		{
			pcTest pt1 = new pcTest { Name = "1", PP = new pp { Name = "2" } };
			pcTest pt2 = new pcTest { Name = "1", PP = new pp { Name = "1" } };
			PropertyCompareList<pcTest> pcCompare = new PropertyCompareList<pcTest>();
			pcCompare.Add("Name");
			pcCompare.Add("PP");
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) > 0);

			pcCompare = new PropertyCompareList<pcTest>(new PropertyCompare<pcTest>(t => t.Name), new PropertyCompare<pcTest>(t => t.PP));
			Assert.IsTrue(pcCompare.Compare(pt1, pt2) > 0);
		}
	}

	class pcTest
	{
		public int Field { get; set; }

		public string Name { get; set; }
		public pc PC { get; set; }
		public pp PP { get; set; }
	}

	class pc
	{
		public string Name { get; set; }
	}

	class pp : IComparable<pp>
	{
		public string Name { get; set; }

		#region IComparable<pp> Members

		public int CompareTo(pp other)
		{
			return Name.CompareTo(other.Name);
		}

		#endregion
	}
}

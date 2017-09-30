using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Exchange.Test
{
	public class Student
	{
		public string Name { get; set; }
		public string ClassName { get; set; }
		public DateTime Birthday { get; set; }
		public string Name2 { get; set; }
		public double Total { get; set; }
	}
	public class Grade
	{
		public string ClassId
		{
			get;
			set;
		}

		public string StudentId
		{
			get;
			set;
		}

		public int Net
		{
			get;
			set;
		}

		public int Java
		{
			get;
			set;
		}

		public int Html
		{
			get;
			set;
		}

		//for test
		public string Name { get; set; }
		public string Brand { get; set; }
		public string Spec { get; set; }
		public string UnitName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}
}

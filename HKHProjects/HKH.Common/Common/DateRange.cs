using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace System//HKH.Common
{
	public class DateRange
	{
		public DateRange()
		{
			Start = DateTime.MinValue;
			End = DateTime.MaxValue;
		}

		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public SqlDateRange ToSqlDateRange()
		{
			return new SqlDateRange() { Start = this.Start, End = this.End };
		}
	}

	public class SqlDateRange
	{
		private DateTime _start;
		private DateTime _end;

		public SqlDateRange()
		{
			Start = SqlDateTime.MinValue.Value;
			End = DateTime.MaxValue;
		}

		public DateTime Start
		{
			get { return _start; }
			set { _start = (value < SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : value); }
		}
		public DateTime End
		{
			get { return _end; }
			set { _end = (value < SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : value); }
		}
	}
}

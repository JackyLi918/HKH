/*******************************************************
 * Filename: Mapping.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/10/2015 11:30:15 AM
 * Author:	JackyLi
 * 
*****************************************************/

namespace HKH.Exchange.Configuration
{
	public abstract class ColumnMapping
	{
		protected string colName = string.Empty;
		protected int colIndex = -1;

		internal ColumnMapping()
		{
			PropertyName = string.Empty;
		}

		/// <summary>
		/// Excel column name，example A, B, AB
		/// </summary>
		public string ColumnName
		{
			get { return colName; }
			set
			{
				colName = value;
				CalculateIndex(colName);
			}
		}

		/// <summary>
		/// the column index to Source(A, B, AB)
		/// </summary>
		public int ColumnIndex
		{
			get { return colIndex; }
		}

		/// <summary>
		/// property name of class or column name of datatable
		/// </summary>
		public string PropertyName { get; set; }

		private void CalculateIndex(string source)
		{
			if (string.IsNullOrEmpty(source))
				colIndex = -1;

			string temp = source.Trim().ToUpper();

			if (2 == temp.Length)
			{
				//temp[0] - 65 + 26 + temp[1] - 65;
				colIndex = temp[0] - 104 + temp[1];
			}
			else
			{
				colIndex = temp[0] - 65;
			}
		}
	}

	public abstract class ExportColumnMapping : ColumnMapping
	{
		private PropertyType _propType = PropertyType.Normal;

		/// <summary>
		/// 
		/// </summary>
		public PropertyType PropertyType
		{
			get { return _propType; }
			set { _propType = value; }
		}
	}

	/// <summary>
	/// Mapping
	/// </summary>
	public class BasicExportColumnMapping : ExportColumnMapping
	{
		protected int rowIndex = -1;

		/// <summary>
		/// 
		/// </summary>
		public int RowIndex
		{
			get { return rowIndex; }
			set { rowIndex = value; }
		}

		public bool Offset { get; set; }

		public string Location { get { return colName + rowIndex.ToString(); } }
	}

	public class DetailsExportColumnMapping : ExportColumnMapping
	{
		private string title = string.Empty;

		/// <summary>
		/// Column title, defaut is datacolumn name or class proerty name
		/// </summary>
		public string Title
		{
			get { return string.IsNullOrEmpty(title) ? PropertyName : title; }
			set { title = value; }
		}
	}

	public class ImportColumnMapping : ColumnMapping
	{
		internal ImportColumnMapping()
		{
			Inherit = false;
			Direction = InheritDirection.Up;
		}


		/// <summary>
		/// indicate whether set the merged cell value to each model
		/// </summary>
		public bool Inherit { get; set; }

		/// <summary>
		/// if inherit is true, where should the current cell value be copied from
		/// </summary>
		public InheritDirection Direction { get; set; }
	}
}
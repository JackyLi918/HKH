/*******************************************************
 * Filename: CsvExportDataTable.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/19/2015 12:08:21 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.CSV
{
	/// <summary>
	/// CsvExportDataTable
	/// </summary>
	public class CSVExportDataTable:  CSVExportBase<DataRow, DataTable>
	{
		#region Constructor

		public CSVExportDataTable(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

        protected CSVExportDataTable(string configurationFile, string tableID, string exportId)
			: base(configurationFile, tableID, exportId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override int GetCount(DataTable tList)
		{
			return tList.Rows.Count;
		}

		protected override DataRow GetTObject(DataTable tList, int index)
		{
			return tList.Rows[index];
		}

		protected override object GetValue(DataRow tObj, string propertyName)
		{
			return tObj[propertyName];
		}

		#endregion
    }
}
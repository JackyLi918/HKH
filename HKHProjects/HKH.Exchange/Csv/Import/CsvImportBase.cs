﻿using HKH.Common;
using HKH.Exchange.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Exchange.Configuration;
using HKH.Exchange.Common;

namespace HKH.Exchange.CSV
{
	public abstract class CSVImportBase<T, TList> : ImportBase<DataRow, DataTable, T, TList>
		where T : class
		where TList : class
	{
		#region Constructor

		protected CSVImportBase(string configurationFile, string tableID)
			: this(configurationFile, tableID, "1")
		{
		}

		protected CSVImportBase(string configurationFile, string tableID, string importId)
			: base(configurationFile, tableID, importId)
		{
		}

		#endregion

		#region CsvImport Overrides Methods

		protected override DataRow GetRow(int index)
		{
			return sheet.Rows[index];
		}

		protected override int GetLastRowIndex(DataTable sheet)
		{
			return sheet.Rows.Count - 1;
		}

		protected override bool IsValidCell(DataRow row, int colIndex)
		{
			return Convert.IsDBNull(row[colIndex]);
		}

		protected override object GetCellData(DataRow row, int colIndex)
		{
			return row[colIndex];
		}

		protected override void SetCellData(DataRow row, int colIndex, object value)
		{
			row[colIndex] = value;
		}

		protected override void PrepareWorkSheet(Import import, string xlsFile, string sheetName)
		{
			sheet = new HKH.CSV.CSVReader(xlsFile).ReadAll(import.FirstRowIndex > 1);
		}

		#endregion
	}
}
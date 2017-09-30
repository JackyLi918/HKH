/*******************************************************
 * Filename: CsvImport.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/23/2015 6:35:29 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using HKH.Exchange.Excel;

namespace HKH.Exchange.CSV
{
	/// <summary>
	/// NPOIImport
	/// </summary>
	internal sealed class CSVImport<T, TList> : CSVImportBase<T, TList>
		where T : class, new()
		where TList : class
	{
		#region Variables

		private bool isDataRow = false;

		#endregion

		public event SourceDataValidatingHandler<DataRow ,DataTable > SourceDataValidating;
		public event DataValidatingHandler<T, TList> TargetDataValidating;

		public CSVImport(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
			this.isDataRow = (typeof(T) == typeof(DataRow));
		}

		#region Properties

		#endregion

		#region Methods

		#endregion

		#region Base Class Overrides

		protected override T GetTInstance()
		{
			if (isDataRow)
				return (successList as DataTable).NewRow() as T;
			else
				return new T();
		}

		protected override void SetValue(T tModel, string propertyName, object value)
		{
			if (isDataRow)
				(tModel as DataRow)[propertyName] = value;
			else
				tModel.SetValue(propertyName, value,true );
		}

		protected override void AppendToTList(T tModel, TList tList)
		{
			if (tList == null)
				return;

			if (isDataRow)
				(tList as DataTable).Rows.Add(tModel as DataRow);
			else
				(tList as IList<T>).Add(tModel);
		}

		protected override bool ValidateSourceData(DataRow  row, DataTable  sheet)
		{
			if (SourceDataValidating != null)
			{
				SourceDataValidatingEventArgs<DataRow, DataTable> args = new SourceDataValidatingEventArgs<DataRow, DataTable>(row, sheet, GetImport(ImportId));
				SourceDataValidating(this, args);
				return !args.Cancel;
			}
			else
			{
				return true;
			}
		}

		protected override bool ValidateTargetData(T tModel, TList tList)
		{
			if (TargetDataValidating != null)
			{
				DataValidatingEventArgs<T, TList> args = new DataValidatingEventArgs<T, TList>(tModel, tList, GetImport(ImportId));
				TargetDataValidating(this, args);
				return !args.Cancel;
			}
			else
			{
				return true;
			}
		}

		public override void Dispose()
		{
			base.Dispose();

			SourceDataValidating = null;
			TargetDataValidating = null;
		}

		#endregion

		#region Helper

		#endregion
	}
}
/*******************************************************
 * Filename: CsvExport.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/19/2015 12:07:22 PM
 * Author:	JackyLi
 * 
*****************************************************/

using HKH.Exchange.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.CSV
{
	/// <summary>
	/// CsvExport
	/// </summary>
	internal sealed class CSVExport<T, TList> : CSVExportBase<T, TList>
		where T : class
		where TList : class
	{
		#region Variables

		private bool isDataRow = false;

		#endregion

		internal event DataValidatingHandler<T, TList> SourceDataValidating;
		internal event GetValueHandler<T> GetValueCallback;

		public CSVExport(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
			this.isDataRow = (typeof(T) == typeof(DataRow));
		}

		#region Properties

		#endregion

		#region Methods

		protected override bool ValidateSourceData(T tModel, TList tList)
		{
			if (SourceDataValidating != null)
			{
				DataValidatingEventArgs<T, TList> args = new DataValidatingEventArgs<T, TList>(tModel, tList, GetExport(ExportId));
				SourceDataValidating(this, args);
				return !args.Cancel;
			}
			else
			{
				return true;
			}
		}

		#endregion

		#region Helper
		protected override int GetCount(TList tList)
		{
			if (isDataRow)
				return (tList as DataTable).Rows.Count;
			else
				return (tList as IList<T>).Count;
		}

		protected override T GetTObject(TList tList, int index)
		{
			if (isDataRow)
				return (tList as DataTable).Rows[index] as T;
			else
				return (tList as IList<T>)[index];
		}

		protected override object GetValue(T tModel, string propertyName)
		{
			if (GetValueCallback != null)
			{
				GetValueEventArgs<T> args = new GetValueEventArgs<T>(tModel, propertyName);
				GetValueCallback(this, args);
				if (args.Handled)
					return args.Value;
			}

			if (isDataRow)
				return (tModel as DataRow)[propertyName];
			else
				return tModel.GetValue(propertyName);
		}

		#endregion

		public override void Dispose()
		{
			base.Dispose();

			SourceDataValidating = null;
			GetValueCallback = null;
		}
	}
}
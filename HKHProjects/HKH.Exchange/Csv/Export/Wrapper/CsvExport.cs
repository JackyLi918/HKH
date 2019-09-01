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
	internal sealed class CSVExport<TBody, TBodyList> : CSVExportBase<TBody, TBodyList>
		where TBody : class
		where TBodyList : class
	{
		#region Variables

		private bool isDataRow = false;

		#endregion

		internal event DataValidatingHandler<TBody, TBodyList> SourceDataValidating;
		internal event GetValueHandler<TBody> GetValueCallback;

		public CSVExport(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
			this.isDataRow = (typeof(TBody) == typeof(DataRow));
		}

		#region Properties

		#endregion

		#region Methods

		protected override bool ValidateSourceData(TBody tObj, TBodyList tList)
		{
			if (SourceDataValidating != null)
			{
				DataValidatingEventArgs<TBody, TBodyList> args = new DataValidatingEventArgs<TBody, TBodyList>(tObj, tList, GetExport(ExportId));
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
		protected override int GetCount(TBodyList tList)
		{
			if (isDataRow)
				return (tList as DataTable).Rows.Count;
			else
				return (tList as IList<TBody>).Count;
		}

		protected override TBody GetTObject(TBodyList tList, int index)
		{
			if (isDataRow)
				return (tList as DataTable).Rows[index] as TBody;
			else
				return (tList as IList<TBody>)[index];
		}

		protected override object GetValue(TBody tObj, string propertyName)
		{
			if (GetValueCallback != null)
			{
				GetValueEventArgs<TBody> args = new GetValueEventArgs<TBody>(tObj, propertyName);
				GetValueCallback(this, args);
				if (args.Handled)
					return args.Value;
			}

			if (isDataRow)
				return (tObj as DataRow)[propertyName];
			else
				return tObj.GetValue(propertyName);
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
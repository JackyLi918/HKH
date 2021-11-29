/*******************************************************
 * Filename: NPOIImport.cs
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
using NPOI.SS.UserModel;

namespace HKH.Exchange.Excel
{
	/// <summary>
	/// NPOIImport
	/// </summary>
	internal sealed class NPOIImport<T, TList> : NPOIImportBase<T, TList>
		where T : class, new()
		where TList : class
	{
		#region Variables

		private bool isDataRow = false;

		#endregion

		public event SourceDataValidatingHandler<IRow, ISheet> SourceDataValidating;
		public event DataValidatingHandler<T, TList> TargetDataValidating;

		public NPOIImport(string configurationFile, string tableID)
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

		protected override void SetValue(T tObj, string propertyName, object value)
		{
			if (isDataRow)
				(tObj as DataRow)[propertyName] = value;
			else
				tObj.SetValue(propertyName, value, true);
		}

		protected override void AppendToTList(T tObj, TList tList)
		{
			if (tList == null)
				return;

			if (isDataRow)
				(tList as DataTable).Rows.Add(tObj as DataRow);
			else
				(tList as IList<T>).Add(tObj);
		}

		protected override bool ValidateSourceData(IRow row, ISheet sheet)
		{
			if (SourceDataValidating != null)
			{
				SourceDataValidatingEventArgs<IRow, ISheet> args = new SourceDataValidatingEventArgs<IRow, ISheet>(row, sheet, Setting);
				SourceDataValidating(this, args);
				return !args.Cancel;
			}
			else
			{
				return true;
			}
		}

		protected override bool ValidateTargetData(T tObj, TList tList)
		{
			if (TargetDataValidating != null)
			{
				DataValidatingEventArgs<T, TList> args = new DataValidatingEventArgs<T, TList>(tObj, tList, Setting);
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
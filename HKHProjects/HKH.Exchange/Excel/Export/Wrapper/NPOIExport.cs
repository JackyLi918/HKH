/*******************************************************
 * Filename: NPOIExport.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/20/2015 11:52:48 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using NPOI.SS.UserModel;

namespace HKH.Exchange.Excel
{
	/// <summary>
	/// NPOIExport
	/// </summary>
	internal sealed class NPOIExport<TBody, TBodyList> : NPOIExportBase<TBody, TBodyList>
		where TBody : class
		where TBodyList : class
	{
		#region Variables

		private bool isDataRow = false;

		#endregion

		internal event DataValidatingHandler<TBody, TBodyList> SourceDataValidating;
		internal event ExtendDataWritingHandler<TBody, TBodyList> ExtendDataWriting;
		internal event TableWritingHandler TableHeaderWriting;
		internal event TableWritingHandler TableFooterWriting;
		internal event PageWritingHandler PageHeaderWriting;
		internal event PageWritingHandler PageFooterWriting;
		internal event GetValueHandler<TBody> GetValueCallback;

		internal event GetValueHandler<object> GetHeaderValueCallback;

		public NPOIExport(string configurationFile, string tableID)
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

		public override void CustomPageHeader(IHeader header)
		{
			if (PageHeaderWriting != null)
				PageHeaderWriting(this, new PageWritingEventArgs(header, GetExport(ExportId)));
		}

		protected override void CustomTableHeader(ISheet sheet)
		{
			if (TableHeaderWriting != null)
				TableHeaderWriting(this, new TableWritingEventArgs(sheet, GetExport(ExportId)));
		}

		protected override void WriteExtendData(TBody tObj, TBodyList tList, IRow row)
		{
			if (ExtendDataWriting != null)
			{
				ExtendDataWriting(this, new ExtendDataWritingEventArgs<TBody, TBodyList>(tObj, tList, row, GetExport(ExportId)));
			}
		}

		protected override void CustomTableFooter(ISheet sheet)
		{
			if (TableFooterWriting != null)
				TableFooterWriting(this, new TableWritingEventArgs(sheet, GetExport(ExportId)));
		}

		protected override void CustomPageFooter(IFooter footer)
		{
			if (PageFooterWriting != null)
				PageFooterWriting(this, new PageWritingEventArgs(footer, GetExport(ExportId)));
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

		protected override object GetValue<THeader>(THeader tObj, string propertyName)
		{
			if (GetHeaderValueCallback != null)
			{
				GetValueEventArgs<object> args = new GetValueEventArgs<object>(tObj, propertyName);
				GetHeaderValueCallback(this, args);
				if (args.Handled)
					return args.Value;
			}

			return base.GetValue<THeader>(tObj, propertyName);
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
			ExtendDataWriting = null;
			TableHeaderWriting = null;
			TableFooterWriting = null;
			PageHeaderWriting = null;
			PageFooterWriting = null;
			GetValueCallback = null;

			GetHeaderValueCallback = null;
		}
	}

	public delegate void TableWritingHandler(object sender, TableWritingEventArgs args);
	public delegate void PageWritingHandler(object sender, PageWritingEventArgs args);
	public delegate void ExtendDataWritingHandler<T, TList>(object sender, ExtendDataWritingEventArgs<T, TList> args)
		where T : class
		where TList : class;

	public class TableWritingEventArgs
	{
		public TableWritingEventArgs(ISheet sheet, Export exp)
		{
			this.Sheet = sheet;
			Export = exp;
		}

		public ISheet Sheet { get; private set; }

		public Export Export { get; private set; }
	}

	public class PageWritingEventArgs
	{
		public PageWritingEventArgs(IHeaderFooter headerfooter, Export exp)
		{
			this.HeaderFootor = headerfooter;
			Export = exp;
		}

		public IHeaderFooter HeaderFootor { get; private set; }
		public Export Export { get; private set; }
	}

	public class ExtendDataWritingEventArgs<T, TList>
		where T : class
		where TList : class
	{
		public ExtendDataWritingEventArgs(T tObj, TList tList, IRow row, Export exp)
		{
			Entity = tObj;
			EntityList = tList;
			ExcelRow = row;
			Export = exp;
		}

		public T Entity { get; private set; }
		public TList EntityList { get; private set; }
		public IRow ExcelRow { get; private set; }
		public Export Export { get; private set; }

	}
}
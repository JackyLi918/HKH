/*******************************************************
 * Filename: EventHandlers.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/26/2015 6:01:11 PM
 * Author:	JackyLi
 * 
*****************************************************/

using HKH.Exchange.Configuration;

namespace HKH.Exchange.Common
{
	public delegate void SourceDataValidatingHandler<T, TList>(object sender, SourceDataValidatingEventArgs<T, TList> args)
		where T : class
		where TList : class;

	public class SourceDataValidatingEventArgs<T, TList>
		where T : class
		where TList : class
	{
		public SourceDataValidatingEventArgs(T row, TList sheet, IImportExportConfiguration import)
		{
			Cancel = false;
			this.Row = row;
			this.Sheet = sheet;
			Configuration = import;
		}

		public bool Cancel { get; set; }
		public T Row { get; private set; }
		public TList Sheet { get; private set; }
		public IImportExportConfiguration Configuration { get; private set; }
	}

	public delegate void DataValidatingHandler<T, TList>(object sender, DataValidatingEventArgs<T, TList> args)
		where T : class
		where TList : class;

	public class DataValidatingEventArgs<T, TList>
		where T : class
		where TList : class
	{
		public DataValidatingEventArgs(T tObj, TList tList, IImportExportConfiguration export)
		{
			Cancel = false;
			Entity = tObj;
			EntityList = tList;
			Configuration = export;
		}

		public bool Cancel { get; set; }
		public T Entity { get; private set; }
		public TList EntityList { get; private set; }
		public IImportExportConfiguration Configuration { get; private set; }
	}

	public delegate void GetValueHandler<T>(object sender, GetValueEventArgs<T> args)
	where T : class;

	public class GetValueEventArgs<T>
		where T : class
	{
		public GetValueEventArgs(T tObj, string propName)
		{
			Entity = tObj;
			PropertyName = propName;
			Handled = false;
		}

		public T Entity { get; private set; }
		public string PropertyName { get; private set; }
		public object Value { get; set; }
		public bool Handled { get; set; }
	}

}
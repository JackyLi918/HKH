/*******************************************************
 * Filename: ImportWrapper.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/26/2015 6:33:07 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Data;
using HKH.Exchange.CSV;
using HKH.Exchange.Excel;
using NPOI.SS.UserModel;

namespace HKH.Exchange.Common
{
	/// <summary>
	/// ImportWrapper
	/// </summary>
	public class ImportWrapper
	{
		private ImportWrapper()
		{
		}

		#region Methods

		public static void ImportExcel<T, TList>(string configFile, string tableId, string importId, string sourceFile, TList successList, TList failList,
			bool withEvents, SourceDataValidatingHandler<IRow, ISheet> sourceDataVilidater = null, DataValidatingHandler<T, TList> targetDataValidater = null)
			where T : class, new()
			where TList : class
		{
			NPOIImport<T, TList> import = CreateNPOIImport<T, TList>(configFile, tableId, importId, withEvents, sourceDataVilidater, targetDataValidater);
			import.Import(sourceFile, successList, failList);
			import.Dispose();
		}

		public static void ImportCsv<T, TList>(string configFile, string tableId, string importId, string sourceFile, TList successList, TList failList,
	bool withEvents, SourceDataValidatingHandler<DataRow, DataTable> sourceDataVilidater = null, DataValidatingHandler<T, TList> targetDataValidater = null)
			where T : class, new()
			where TList : class
		{
			CSVImport<T, TList> import = CreateCsvImport<T, TList>(configFile, tableId, importId, withEvents, sourceDataVilidater, targetDataValidater);
			import.Import(sourceFile, successList, failList);
			import.Dispose();
		}

		#endregion

		#region Helper

		private static NPOIImport<T, TList> CreateNPOIImport<T, TList>(string configFile, string tableId, string importId, bool withEvents,
			SourceDataValidatingHandler<IRow, ISheet> sourceDataVilidater = null, DataValidatingHandler<T, TList> targetDataValidater = null)
			where T : class, new()
			where TList : class
		{
			NPOIImport<T, TList> import = new NPOIImport<T, TList>(configFile, tableId);
			import.ImportId = importId;

			if (withEvents)
			{
				if (sourceDataVilidater != null)
					import.SourceDataValidating += sourceDataVilidater;

				if (targetDataValidater != null)
					import.TargetDataValidating += targetDataValidater;
			}

			return import;
		}

		private static CSVImport<T, TList> CreateCsvImport<T, TList>(string configFile, string tableId, string importId, bool withEvents,
	SourceDataValidatingHandler<DataRow, DataTable> sourceDataVilidater = null, DataValidatingHandler<T, TList> targetDataValidater = null)
			where T : class, new()
			where TList : class
		{
			CSVImport<T, TList> import = new CSVImport<T, TList>(configFile, tableId);
			import.ImportId = importId;

			if (withEvents)
			{
				if (sourceDataVilidater != null)
					import.SourceDataValidating += sourceDataVilidater;

				if (targetDataValidater != null)
					import.TargetDataValidating += targetDataValidater;
			}

			return import;
		}

		#endregion
	}
}
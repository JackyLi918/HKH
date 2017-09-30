/*******************************************************
 * Filename: ExportWrapper.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/20/2015 1:56:48 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.IO;
using HKH.Exchange.Excel;
using HKH.Exchange.CSV;

namespace HKH.Exchange.Common
{
	/// <summary>
	/// ExportWrapper
	/// </summary>
	public class ExportWrapper
	{
		private ExportWrapper()
		{
		}

		#region Methods

		public static void ExportExcel<T, TList>(string configFile, string tableId, string exportId, string targetFile, TList tList,
			DataValidatingHandler<T, TList> dataValidater = null, ExtendDataWritingHandler<T, TList> extendDataWriter = null, TableWritingHandler tableHeaderWriter = null,
			TableWritingHandler tableFooterWriter = null, PageWritingHandler pageHeaderWriter = null, PageWritingHandler pageFooterWriter = null, GetValueHandler<T> getValueCallback = null)
			where T : class
			where TList : class
		{
			NPOIExport<T, TList> export = CreateNPOIExport<T, TList>(configFile, tableId, exportId, dataValidater, extendDataWriter, tableHeaderWriter, tableFooterWriter, pageHeaderWriter, pageFooterWriter, getValueCallback, null);
			export.Export(targetFile, tList);
			export.Dispose();
		}

		/// <summary>
		/// Export data to a stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="tList"></param>
		public static void ExportExcel<T, TList>(string configFile, string tableId, string exportId, Stream stream, TList tList,
			DataValidatingHandler<T, TList> dataValidater = null, ExtendDataWritingHandler<T, TList> extendDataWriter = null, TableWritingHandler tableHeaderWriter = null,
			TableWritingHandler tableFooterWriter = null, PageWritingHandler pageHeaderWriter = null, PageWritingHandler pageFooterWriter = null, GetValueHandler<T> getValueCallback = null)
			where T : class
			where TList : class
		{
			NPOIExport<T, TList> export = CreateNPOIExport<T, TList>(configFile, tableId, exportId, dataValidater, extendDataWriter, tableHeaderWriter, tableFooterWriter, pageHeaderWriter, pageFooterWriter, getValueCallback, null);
			export.Export(stream, tList);
			export.Dispose();
		}

		public static void FillExcel<T, TList, TBasic>(string configFile, string tableId, string exportId, string templateFile, string targetFile, TList tList, TBasic tBasic, GetValueHandler<object> getBasicValueCallback = null)
			where T : class
			where TList : class
		{
			NPOIExport<T, TList> export = CreateNPOIExport<T, TList>(configFile, tableId, exportId, null, null, null, null, null, null, null, getBasicValueCallback);
			export.Fill<TBasic>(templateFile, targetFile, tList, tBasic);
			export.Dispose();
		}

		public static void FillExcel<T, TList, TBasic>(string configFile, string tableId, string exportId, string templateFile, Stream targetStream, TList tList, TBasic tBasic, GetValueHandler<object> getBasicValueCallback = null)
			where T : class
			where TList : class
		{
			NPOIExport<T, TList> export = CreateNPOIExport<T, TList>(configFile, tableId, exportId, null, null, null, null, null, null, null, getBasicValueCallback);
			export.Fill(templateFile, targetStream, tList, tBasic);
			export.Dispose();
		}

		public static void ExportCSV<T, TList>(string configFile, string tableId, string exportId, string targetFile, TList tList,
			DataValidatingHandler<T, TList> dataValidater = null, GetValueHandler<T> getValueCallback = null)
			where T : class
			where TList : class
		{
			CSVExport<T, TList> export = CreateCSVExport<T, TList>(configFile, tableId, exportId, dataValidater, getValueCallback);
			export.Export(targetFile, tList);
			export.Dispose();
		}

		public static void ExportCSV<T, TList>(string configFile, string tableId, string exportId, Stream targetStream, TList tList,
			DataValidatingHandler<T, TList> dataValidater = null, GetValueHandler<T> getValueCallback = null)
			where T : class
			where TList : class
		{
			CSVExport<T, TList> export = CreateCSVExport<T, TList>(configFile, tableId, exportId, dataValidater, getValueCallback);
			export.Export(targetStream, tList);
			export.Dispose();
		}

		#endregion

		#region Helper

		private static NPOIExport<T, TList> CreateNPOIExport<T, TList>(string configFile, string tableId, string exportId,
			DataValidatingHandler<T, TList> dataValidater, ExtendDataWritingHandler<T, TList> extendDataWriter, TableWritingHandler tableHeaderWriter,
			TableWritingHandler tableFooterWriter, PageWritingHandler pageHeaderWriter, PageWritingHandler pageFooterWriter, GetValueHandler<T> getValueCallback, GetValueHandler<object> getBasicValueCallback)
			where T : class
			where TList : class
		{
			NPOIExport<T, TList> export = new NPOIExport<T, TList>(configFile, tableId);
			export.ExportId = exportId;

			if (dataValidater != null)
				export.SourceDataValidating += dataValidater;

			if (extendDataWriter != null)
				export.ExtendDataWriting += extendDataWriter;

			if (tableHeaderWriter != null)
				export.TableHeaderWriting += tableHeaderWriter;

			if (tableFooterWriter != null)
				export.TableFooterWriting += tableFooterWriter;

			if (pageHeaderWriter != null)
				export.PageHeaderWriting += pageHeaderWriter;

			if (pageFooterWriter != null)
				export.PageFooterWriting += pageFooterWriter;

			if (getValueCallback != null)
				export.GetValueCallback += getValueCallback;

			if (getBasicValueCallback != null)
				export.GetBasicValueCallback += getBasicValueCallback;

			return export;
		}

		private static CSVExport<T, TList> CreateCSVExport<T, TList>(string configFile, string tableId, string exportId,
			DataValidatingHandler<T, TList> dataValidater, GetValueHandler<T> getValueCallback)
			where T : class
			where TList : class
		{
			CSVExport<T, TList> export = new CSVExport<T, TList>(configFile, tableId);
			export.ExportId = exportId;

			if (dataValidater != null)
				export.SourceDataValidating += dataValidater;

			if (getValueCallback != null)
				export.GetValueCallback += getValueCallback;

			return export;
		}

		#endregion
	}
}
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
using HKH.Exchange.CSV;
using HKH.Exchange.Excel;

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

		public static void ExportExcel<TBody, TBodyList>(string configFile, string tableId, string exportId, string targetFile, TBodyList tList,
			DataValidatingHandler<TBody, TBodyList> dataValidater = null, ExtendDataWritingHandler<TBody, TBodyList> extendDataWriter = null, TableWritingHandler tableHeaderWriter = null,
			TableWritingHandler tableFooterWriter = null, PageWritingHandler pageHeaderWriter = null, PageWritingHandler pageFooterWriter = null, GetValueHandler<TBody> getValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			NPOIExport<TBody, TBodyList> export = CreateNPOIExport<TBody, TBodyList>(configFile, tableId, exportId, dataValidater, extendDataWriter, tableHeaderWriter, tableFooterWriter, pageHeaderWriter, pageFooterWriter, getValueCallback, null);
			export.Export(targetFile, tList);
			export.Dispose();
		}

		/// <summary>
		/// Export data to a stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="tList"></param>
		public static void ExportExcel<TBody, TBodyList>(string configFile, string tableId, string exportId, Stream stream, TBodyList tList,
			DataValidatingHandler<TBody, TBodyList> dataValidater = null, ExtendDataWritingHandler<TBody, TBodyList> extendDataWriter = null, TableWritingHandler tableHeaderWriter = null,
			TableWritingHandler tableFooterWriter = null, PageWritingHandler pageHeaderWriter = null, PageWritingHandler pageFooterWriter = null, GetValueHandler<TBody> getValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			NPOIExport<TBody, TBodyList> export = CreateNPOIExport<TBody, TBodyList>(configFile, tableId, exportId, dataValidater, extendDataWriter, tableHeaderWriter, tableFooterWriter, pageHeaderWriter, pageFooterWriter, getValueCallback, null);
			export.Export(stream, tList);
			export.Dispose();
		}

		public static void FillExcel<TBody, TBodyList, THeader>(string configFile, string tableId, string exportId, string templateFile, string targetFile, TBodyList tList, THeader tHeader, GetValueHandler<object> getHeaderValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			NPOIExport<TBody, TBodyList> export = CreateNPOIExport<TBody, TBodyList>(configFile, tableId, exportId, null, null, null, null, null, null, null, getHeaderValueCallback);
			export.Fill<THeader>(templateFile, targetFile, tList, tHeader);
			export.Dispose();
		}

		public static void FillExcel<TBody, TBodyList, THeader>(string configFile, string tableId, string exportId, string templateFile, Stream targetStream, TBodyList tList, THeader tHeader, GetValueHandler<object> getHeaderValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			NPOIExport<TBody, TBodyList> export = CreateNPOIExport<TBody, TBodyList>(configFile, tableId, exportId, null, null, null, null, null, null, null, getHeaderValueCallback);
			export.Fill(templateFile, targetStream, tList, tHeader);
			export.Dispose();
		}

		public static void ExportCSV<TBody, TBodyList>(string configFile, string tableId, string exportId, string targetFile, TBodyList tList,
			DataValidatingHandler<TBody, TBodyList> dataValidater = null, GetValueHandler<TBody> getValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			CSVExport<TBody, TBodyList> export = CreateCSVExport<TBody, TBodyList>(configFile, tableId, exportId, dataValidater, getValueCallback);
			export.Export(targetFile, tList);
			export.Dispose();
		}

		public static void ExportCSV<TBody, TBodyList>(string configFile, string tableId, string exportId, Stream targetStream, TBodyList tList,
			DataValidatingHandler<TBody, TBodyList> dataValidater = null, GetValueHandler<TBody> getValueCallback = null)
			where TBody : class
			where TBodyList : class
		{
			CSVExport<TBody, TBodyList> export = CreateCSVExport<TBody, TBodyList>(configFile, tableId, exportId, dataValidater, getValueCallback);
			export.Export(targetStream, tList);
			export.Dispose();
		}

		#endregion

		#region Helper

		private static NPOIExport<TBody, TBodyList> CreateNPOIExport<TBody, TBodyList>(string configFile, string tableId, string exportId,
			DataValidatingHandler<TBody, TBodyList> dataValidater, ExtendDataWritingHandler<TBody, TBodyList> extendDataWriter, TableWritingHandler tableHeaderWriter,
			TableWritingHandler tableFooterWriter, PageWritingHandler pageHeaderWriter, PageWritingHandler pageFooterWriter, GetValueHandler<TBody> getValueCallback, GetValueHandler<object> getHeaderValueCallback)
			where TBody : class
			where TBodyList : class
		{
			NPOIExport<TBody, TBodyList> export = new NPOIExport<TBody, TBodyList>(configFile, tableId);
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

			if (getHeaderValueCallback != null)
				export.GetHeaderValueCallback += getHeaderValueCallback;

			return export;
		}

		private static CSVExport<TBody, TBodyList> CreateCSVExport<TBody, TBodyList>(string configFile, string tableId, string exportId,
			DataValidatingHandler<TBody, TBodyList> dataValidater, GetValueHandler<TBody> getValueCallback)
			where TBody : class
			where TBodyList : class
		{
			CSVExport<TBody, TBodyList> export = new CSVExport<TBody, TBodyList>(configFile, tableId);
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
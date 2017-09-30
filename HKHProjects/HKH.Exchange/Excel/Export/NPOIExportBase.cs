using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using HKH.Common;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using NPOI.SS.UserModel;

namespace HKH.Exchange.Excel
{
    public abstract class NPOIExportBase<T, TList> : ExportBase<T, TList>
        where T : class
        where TList : class
    {
        private const string Pattern_GetAllFunctions = @"(?i)\{\w+\([+-]?\d+\)\}";

        #region Protected Variable

        protected IWorkbook workBook = null;
        protected ISheet sheet = null;

        #endregion

        #region Constructor

        protected NPOIExportBase(string configurationFile, string tableID)
            : this(configurationFile, tableID, "1")
        {
        }

        protected NPOIExportBase(string configurationFile, string tableID, string exportId)
            : base(configurationFile, tableID)
        {
            this.ExportId = exportId;
            Reset();
        }

        #endregion

        #region Public Methods

        protected override void ExportCore(Stream stream, TList tList)
        {
            workBook = NPOIExtension.CreateWorkbook(export.XlsFormat);
            //----set summary
            //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //dsi.Company = "BlackEyes";
            //workBook.DocumentSummaryInformation = dsi;
            //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //si.Subject = "HKH.Exchange.ExcelExport";
            //workBook.SummaryInformation = si;
            //----end

            //----create dateformat
            dateFormat = workBook.CreateDataFormat().GetFormat(dateFormatString);
            //----end

            sheet = workBook.CreateSheet(string.IsNullOrEmpty(_tableMapping.Sheet) ? "Sheet1" : _tableMapping.Sheet);

            CustomPageHeader(sheet.Header);
            CustomTableHeader(sheet);

            OutputDetails(tList);

            CustomTableFooter(sheet);

            workBook.Write(stream);
        }

        public void Fill<TBasic>(string templateFile, string targetFile, TList tList, TBasic tBasic)
        {
            using (Stream stream = File.Create(targetFile))
            {
                Fill(templateFile, stream, tList, tBasic);
                if (stream.CanWrite)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public void Fill<TBasic>(string templateFile, Stream targetStream, TList tList, TBasic tBasic)
        {
            if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
                throw Error.ArgumentNotValid("templateFile");

            using (Stream stream = File.Open(templateFile, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = NPOIExtension.CreateWorkbook(XlsFormat.Auto, stream);
                Fill(tList, tBasic);
            }

            workBook.Write(targetStream);
            Reset();
        }

        private void Fill<TBasic>(TList tList, TBasic tBasic)
        {
            mode = ExportMode.Fill;
            export = GetExport(ExportId);
            dateFormatString = string.IsNullOrEmpty(export.DateFormat) ? DEFAULTDATEFORMATSTRING : export.DateFormat;
            numberFormatSTring = string.IsNullOrEmpty(export.NumberFormat) ? DEFAULTNUMBERFORMATSTRING : export.NumberFormat;

            //looks as sheet index if it is Integer, otherwise TableName
            int sheetIndex = 0;
            if (int.TryParse(_tableMapping.Sheet, out sheetIndex))
                sheet = workBook.GetSheetAt(sheetIndex);
            else
                sheet = workBook.GetSheet(_tableMapping.Sheet);

            //----create date format
            dateFormat = workBook.CreateDataFormat().GetFormat(dateFormatString);
            //----end

            OutputDetails(tList);
            OutputBasic(tBasic);
        }

        #endregion

        #region Protected Virtual Methods

        public override int NextRowNum()
        {
            if (mode == ExportMode.Export)
            {
                if (sheet.LastRowNum > 0)
                    return sheet.LastRowNum + 1;
                else
                {
                    IRow row = sheet.GetRow(sheet.LastRowNum);
                    return row == null ? 0 : 1;
                }
            }
            else
            {
                curEIndex++;
                return export.DetailsMapping.FirstRowIndex + curEIndex;
            }
        }

        protected IRow GetNewRow(int rowNum, TList tList)
        {
            IRow row = null; ;

            if (mode == ExportMode.Fill)
            {
                switch (export.DetailsMapping.RowMode)
                {
                    case FillRowMode.Copy:
                        //copy current row if has more row
                        if (curTIndex < GetCount(tList) - 1)
                        {
                            sheet.CopyRow(rowNum, rowNum + 1);
                        }
                        row = sheet.GetRow(rowNum);
                        sheet.GetRow(rowNum + 1).Height = row.Height;
                        break;
                    case FillRowMode.Fill:
                        row = sheet.GetRow(rowNum);
                        break;
                    default:
                        row = sheet.CreateRow(rowNum);
                        break;
                }
            }
            else
            {
                row = sheet.CreateRow(rowNum);
            }

            return row;
        }

        protected void CalculateExpression(ExportColumnMapping columnMapping, ICell cell, int sourceRowIndex)
        {
            string value = columnMapping.PropertyName;
            List<string> funcs = new List<string>();

            foreach (Match m in Regex.Matches(columnMapping.PropertyName, Pattern_GetAllFunctions))
            {
                funcs.TryAdd(m.Value);
            }

            if (funcs.Count > 0)
            {
                foreach (string fun in funcs)
                {
                    value = value.Replace(fun, ExportExpression.Eval(fun, cell.RowIndex, sourceRowIndex));
                }
            }

            if (value.StartsWith("="))
            {
                cell.SetCellFormula(value.TrimStart('='));
            }
            else
                cell.SetCellValue(value);
        }

        //TODO: jacky, this code doesnot work for npoi2.0
        //protected IColor GetColor(Color sysColor)
        //{
        //	return GetColor(sysColor.R, sysColor.G, sysColor.B);
        //}

        //protected IColor GetColor(byte red, byte green, byte blue)
        //{
        //	HSSFPalette hssfPalette = workBook.GetCustomPalette();
        //	HSSFColor hssfColour = hssfPalette.FindColor(red, green, blue);
        //	if (hssfColour == null)
        //	{
        //		if (PaletteRecord.STANDARD_PALETTE_SIZE < 255)
        //		{
        //			if (PaletteRecord.STANDARD_PALETTE_SIZE < 64)
        //			{
        //				PaletteRecord.STANDARD_PALETTE_SIZE = 64;
        //				PaletteRecord.STANDARD_PALETTE_SIZE += 1;
        //				hssfColour = hssfPalette.AddColor(red, green, blue);
        //			}
        //			else
        //			{
        //				hssfColour = hssfPalette.FindSimilarColor(red, green, blue);
        //			}
        //		}
        //	}

        //	return hssfColour;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        public virtual void CustomPageHeader(IHeader header)
        {
            return;
        }

        /// <summary>
        /// Custom table header before data exporting
        /// </summary>
        /// <param name="sheet"></param>
        protected virtual void CustomTableHeader(ISheet sheet)
        {
            return;
        }

        /// <summary>
        /// Write extend data after exported
        /// </summary>
        /// <param name="tModel"></param>
        /// <param name="tList"></param>
        /// <param name="row"></param>
        protected virtual void WriteExtendData(T tModel, TList tList, IRow row)
        {
        }

        /// <summary>
        /// Custom table end after data exported
        /// </summary>
        /// <param name="sheet"></param>
        protected virtual void CustomTableFooter(ISheet sheet)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        protected virtual void CustomPageFooter(IFooter footer)
        {
        }

        protected virtual object GetValue<TBasic>(TBasic tObj, string propertyName)
        {
            if (tObj is DataTable)
            {
                return (tObj as DataTable).Rows[0][propertyName];
            }
            if (tObj is DataRow)
            {
                return (tObj as DataRow)[propertyName];
            }
            else
            {
                PropertyInfo pi = typeof(TBasic).GetProperty(propertyName);
                return pi.GetValue(tObj, null);
            }
        }

        protected virtual string GetColumnTitle(DetailsExportColumnMapping columnMapping)
        {
            return columnMapping.Title;
        }

        /// <summary>
        /// release resource
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            if (sheet != null)
            {
                //sheet.Dispose();
                sheet = null;
            }
            if (workBook != null)
            {
                //workBook.Dispose();
                workBook = null;
            }
        }

        #endregion

        #region Private Methods

        private void OutputBasic<TBasic>(TBasic tBasic)
        {
            int rowIndex = 0;
            foreach (BasicExportColumnMapping columnMapping in export.BasicMapping.Values)
            {
                rowIndex = (columnMapping.Offset && export.DetailsMapping.RowMode != FillRowMode.Fill) ? columnMapping.RowIndex + curEIndex : columnMapping.RowIndex;
                IRow dataRow = sheet.GetRow(rowIndex);
                if (columnMapping.PropertyType == PropertyType.Expression)
                    CalculateExpression(columnMapping, dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                else
                    dataRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetValue<TBasic>(tBasic, columnMapping.PropertyName), dateFormat);
            }
        }

        private void OutputDetails(TList tList)
        {
            //write columns' title
            if (mode == ExportMode.Export && export.DetailsMapping.OutPutTitle)
            {
                IRow titleRow = sheet.CreateRow(NextRowNum());
                foreach (DetailsExportColumnMapping columnMapping in export.DetailsMapping.Values)
                {
                    //write column title under sheet title
                    titleRow.CreateCell(columnMapping.ColumnIndex);
                    titleRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetColumnTitle(columnMapping), dateFormat);
                }
            }

            T tObj = null;

            int rowIndex = 0;
            while (NextTObject(tList, out tObj))
            {
                if (ValidateSourceData(tObj, tList))
                {
                    IRow dataRow = GetNewRow(NextRowNum(), tList);
                    //write data
                    foreach (DetailsExportColumnMapping columnMapping in export.DetailsMapping.Values)
                    {
                        if (mode == ExportMode.Export || export.DetailsMapping.RowMode == FillRowMode.New)
                            dataRow.CreateCell(columnMapping.ColumnIndex);

                        if (columnMapping.PropertyType == PropertyType.Expression)
                            CalculateExpression(columnMapping, dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                        else
                            dataRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetValue(tObj, columnMapping.PropertyName), dateFormat);
                    }

                    if (mode == ExportMode.Export)
                        WriteExtendData(tObj, tList, dataRow);

                    rowIndex++;
                }
            }
        }

        #endregion
    }
}

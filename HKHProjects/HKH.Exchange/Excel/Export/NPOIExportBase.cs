using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using HKH.Common;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HKH.Exchange.Excel
{
    public abstract class NPOIExportBase<TBody, TBodyList> : ExportBase<TBody, TBodyList>
        where TBody : class
        where TBodyList : class
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

        protected override void ExportCore(Stream stream, TBodyList tList)
        {
            workBook = NPOIExtension.CreateWorkbook(Setting.XlsFormat);
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

            sheet = workBook.CreateSheet(Setting.Sheet);

            CustomPageHeader(sheet.Header);
            CustomTableHeader(sheet);

            WriteBody(tList);

            CustomTableFooter(sheet);

            workBook.Write(stream, true);
        }

        public void Fill<THeader>(string templateFile, string targetFile, TBodyList tList, THeader tHeader)
        {
            if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
                throw Error.ArgumentNotValid("templateFile");

            using (Stream stream = File.Create(targetFile))
            {
                Fill(templateFile, stream, tList, tHeader);
                if (stream.CanWrite)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public void Fill<THeader>(string templateFile, Stream targetStream, TBodyList tList, THeader tHeader)
        {
            if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
                throw Error.ArgumentNotValid("templateFile");

            using (Stream stream = File.Open(templateFile, FileMode.Open, FileAccess.ReadWrite))
            {
                Fill(stream, targetStream, tList, tHeader);
                stream.Close();
            }
        }
        public void Fill<THeader>(Stream templateStream, Stream targetStream, TBodyList tList, THeader tHeader)
        {
            Setting = GetExport(ExportId);

            templateStream.Seek(0, SeekOrigin.Begin);
            workBook = NPOIExtension.CreateWorkbook(XlsFormat.Auto, templateStream);
            FillCore(Setting, tList, tHeader);

            workBook.Write(targetStream, true);
            Reset();
        }
        public void Fill<THeader>(Export setting, string templateFile, string targetFile, TBodyList tList, THeader tHeader)
        {
            if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
                throw Error.ArgumentNotValid("templateFile");

            using (Stream stream = File.Create(targetFile))
            {
                Fill(setting, templateFile, stream, tList, tHeader);
                if (stream.CanWrite)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public void Fill<THeader>(Export setting, string templateFile, Stream targetStream, TBodyList tList, THeader tHeader)
        {
            if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
                throw Error.ArgumentNotValid("templateFile");

            using (Stream stream = File.Open(templateFile, FileMode.Open, FileAccess.ReadWrite))
            {
                Fill(setting, stream, targetStream, tList, tHeader);
                stream.Close();
            }
        }
        public void Fill<THeader>(Export setting, Stream templateStream, Stream targetStream, TBodyList tList, THeader tHeader)
        {
            Setting = setting;

            templateStream.Seek(0, SeekOrigin.Begin);
            workBook = NPOIExtension.CreateWorkbook(XlsFormat.Auto, templateStream);
            FillCore(Setting, tList, tHeader);

            workBook.Write(targetStream, true);
            Reset();
        }
        private void FillCore<THeader>(Export setting, TBodyList tList, THeader tHeader)
        {
            Setting = setting;
            FillCore(tList, tHeader);
        }
        private void FillCore<THeader>(TBodyList tList, THeader tHeader)
        {
            mode = ExportMode.Fill;
            InitFormat();

            //looks as sheet index if it is Integer, otherwise TableName
            int sheetIndex = 0;
            if (int.TryParse(Setting.Sheet, out sheetIndex))
                sheet = workBook.GetSheetAt(sheetIndex);
            else
                sheet = workBook.GetSheet(Setting.Sheet);

            //----create date format
            dateFormat = workBook.CreateDataFormat().GetFormat(dateFormatString);
            //----end

            if (tList != null && Setting.Body != null && Setting.Body.Count > 0)
                WriteBody(tList);
            if (tHeader != null && Setting.Header != null && Setting.Header.Count > 0)
                WriteHeader(tHeader);
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
                return Setting.Body.FirstRowIndex + curEIndex;
            }
        }

        protected IRow GetNewRow(int rowNum, TBodyList tList)
        {
            IRow row = null; ;

            if (mode == ExportMode.Fill)
            {
                switch (Setting.Body.RowMode)
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
        protected virtual void WritePicture(ExportColumnMapping columnMapping, string filePath, ICell cell, int sourceRowIndex)
        {
            if (!File.Exists(filePath)) return;

            byte[] bytes = File.ReadAllBytes(filePath);
            WritePicture(columnMapping, bytes, PictureType.Unknown, cell, sourceRowIndex);
        }
        protected virtual void WritePicture(ExportColumnMapping columnMapping, byte[] bytes, PictureType picType, ICell cell, int sourceRowIndex)
        {
            var picIndex = workBook.AddPicture(bytes, picType);
            var patriarch = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor;
            IPicture pict;
            if (Setting.XlsFormat == XlsFormat.Xls)
            {
                anchor = new HSSFClientAnchor(0, 0, 0, 0, cell.ColumnIndex, sourceRowIndex, cell.ColumnIndex + 1, sourceRowIndex + 1);
                pict = (HSSFPicture)patriarch.CreatePicture(anchor, picIndex);
            }
            else
            {
                anchor = new XSSFClientAnchor(0, 0, 0, 0, cell.ColumnIndex, sourceRowIndex, cell.ColumnIndex + 1, sourceRowIndex + 1);
                pict = (XSSFPicture)patriarch.CreatePicture(anchor, picIndex);
            }
        }

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
        /// <param name="t"></param>
        /// <param name="tList"></param>
        /// <param name="row"></param>
        protected virtual void WriteExtendData(TBody tObj, TBodyList tList, IRow row)
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

        protected virtual object GetValue<THeader>(THeader tObj, string propertyName)
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
                PropertyInfo pi = typeof(THeader).GetProperty(propertyName);
                return pi.GetValue(tObj, null);
            }
        }

        protected virtual string GetColumnTitle(ExportBodyColumnMapping columnMapping)
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

        private void WriteHeader<THeader>(THeader tHeader)
        {
            foreach (ExportHeaderColumnMapping columnMapping in Setting.Header.Values)
            {
                int rowIndex = (columnMapping.Offset && Setting.Body.RowMode != FillRowMode.Fill) ? columnMapping.RowIndex + curEIndex : columnMapping.RowIndex;
                IRow dataRow = sheet.GetRow(rowIndex);
                switch (columnMapping.PropertyType)
                {
                    case PropertyType.Expression:
                        CalculateExpression(columnMapping, dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                        break;
                    case PropertyType.Picture:
                        WritePicture(columnMapping, (GetValue(tHeader, columnMapping.PropertyName) ?? "").ToString(), dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                        break;
                    default:
                        dataRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetValue<THeader>(tHeader, columnMapping.PropertyName), dateFormat);
                        break;
                }
            }
        }

        private void WriteBody(TBodyList tList)
        {
            //write columns' title
            if (mode == ExportMode.Export && Setting.Body.OutPutTitle)
            {
                IRow titleRow = GetNewRow(NextRowNum(), tList);
                foreach (ExportBodyColumnMapping columnMapping in Setting.Body.Values)
                {
                    //write column title under sheet title
                    titleRow.CreateCell(columnMapping.ColumnIndex);
                    titleRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetColumnTitle(columnMapping), dateFormat);
                }
            }

            TBody tObj;
            int rowIndex = 0;
            while (NextTObject(tList, out tObj))
            {
                if (ValidateSourceData(tObj, tList))
                {
                    IRow dataRow = GetNewRow(NextRowNum(), tList);
                    //write data
                    foreach (ExportBodyColumnMapping columnMapping in Setting.Body.Values)
                    {
                        if (mode == ExportMode.Export || Setting.Body.RowMode == FillRowMode.New)
                            dataRow.CreateCell(columnMapping.ColumnIndex);

                        switch (columnMapping.PropertyType)
                        {
                            case PropertyType.Expression:
                                CalculateExpression(columnMapping, dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                                break;
                            case PropertyType.Picture:
                                WritePicture(columnMapping, (GetValue(tObj, columnMapping.PropertyName) ?? "").ToString(), dataRow.GetCell(columnMapping.ColumnIndex), rowIndex);
                                break;
                            default:
                                dataRow.GetCell(columnMapping.ColumnIndex).SetCellValue(GetValue(tObj, columnMapping.PropertyName), dateFormat);
                                break;
                        }
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

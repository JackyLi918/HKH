using System.Data;
using System.IO;
using System.Linq;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using NPOI.SS.UserModel;

namespace HKH.Exchange.Excel
{
    public abstract class NPOIImportBase<T, TList> : ImportBase<IRow, ISheet, T, TList>
        where T : class
        where TList : class
    {
        protected IWorkbook workBook = null;

        #region Constructor

        protected NPOIImportBase(string configurationFile, string tableID)
            : this(configurationFile, tableID, "1")
        {
        }

        protected NPOIImportBase(string configurationFile, string tableID, string importId)
            : base(configurationFile, tableID)
        {
        }

        #endregion

        #region ExcelImport Overrides Methods

        protected override IRow GetRow(int index)
        {
            return sheet.GetRow(index);
        }

        protected override int GetLastRowIndex(ISheet sheet)
        {
            return sheet.LastRowNum;
        }

        protected override bool IsValidCell(IRow row, int colIndex)
        {
            return row.GetCell(colIndex) == null || row.GetCell(colIndex).CellType == CellType.Blank;
        }

        protected override object GetCellData(IRow row, int colIndex)
        {
            return row.GetCell(colIndex).GetCellValue();
        }

        protected override void SetCellData(IRow row, int colIndex, object value)
        {
            row.GetCell(colIndex).SetCellValue(value);
        }

        /// <summary>
        /// Load the excel file to NPOI worksheet
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetName"></param>
        protected override void PrepareWorkSheet(Import import, string xlsFile, string sheetName)
        {
            using (Stream stream = File.Open(xlsFile, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = NPOIExtension.CreateWorkbook(import.XlsFormat, stream);

                //looks as sheet index if it is Integer, otherwise TableName
                int sheetIndex = 0;
                if (int.TryParse(sheetName, out sheetIndex))
                    sheet = workBook.GetSheetAt(sheetIndex);
                else
                    sheet = workBook.GetSheet(sheetName);

                stream.Close();
            }
        }

        protected override string[] GetDataHeaders(ISheet sheet)
        {
            if (Setting.FirstRowIndex > 0)
            {
                IRow firstRow = sheet.GetRow(0);
                return firstRow.Cells.Select<ICell, string>(c => c.GetCellValue().ToString()).ToArray();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            workBook = null;
        }

        #endregion
    }
}

using System;
using System.IO;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace HKH.Exchange.Excel
{
    public static class NPOIExtension
	{
		public static IWorkbook CreateWorkbook(XlsFormat fmt, Stream stream = null)
		{
			IWorkbook workbook = null;
			switch (fmt)
			{
				case XlsFormat.Xls:
					workbook = stream == null ? new HSSFWorkbook() : new HSSFWorkbook(stream);
					break;
				case XlsFormat.Xlsx:
					workbook = stream == null ? new XSSFWorkbook() : new XSSFWorkbook(stream);
					break;
				default:
					workbook = (stream == null || stream.Length == 0) ? new XSSFWorkbook() : CreateWorkbook(ConfirmXlsFormat(stream), stream);
					break;
			}

			return workbook;
		}

		private static XlsFormat ConfirmXlsFormat(Stream s)
		{
			long pos = s.Position;
			s.Seek(0, SeekOrigin.Begin);
			int b = s.ReadByte();

			//reset pos
			s.Seek(pos, SeekOrigin.Begin);

			if (b == 208)	//208 is Excel2003
				return XlsFormat.Xls;

			return XlsFormat.Xlsx;
		}

		/// <summary>
		/// 只处理boolean和date/double，其他的将执行Cell.ToString()
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public static object GetCellValue(this ICell cell)
		{
			object val = null;
			switch (cell.CellType)
			{
				case CellType.Blank:
					val = null;
					break;
				case CellType.Boolean:
					val = cell.BooleanCellValue;
					break;
				case CellType.Numeric:
					if (DateUtil.IsCellDateFormatted(cell))
						val = cell.DateCellValue;
					else
						val = cell.NumericCellValue;
					break;
				case CellType.String:
					val = cell.StringCellValue;
					break;
				case CellType.Formula:
					switch (cell.CachedFormulaResultType)
					{
						case CellType.Blank:
							val = null;
							break;
						case CellType.Boolean:
							val = cell.BooleanCellValue;
							break;
						case CellType.Numeric:
							if (DateUtil.IsCellDateFormatted(cell))
								val = cell.DateCellValue;
							else
								val = cell.NumericCellValue;
							break;
						case CellType.String:
							val = cell.StringCellValue;
							break;
						default:
							val = cell.ToString();
							break;
					}
					break;
				default:
					val = cell.ToString();
					break;
			}
			return val;
		}

		/// <summary>
		/// 对比NPOI's API，只处理四种基本类型
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="value"></param>
		public static void SetCellValue(this ICell cell, object value)
		{
			SetCellValue(cell, value, null);
		}

		/// <summary>
		/// 对比NPOI's API，只处理四种基本类型
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="value"></param>
		public static void SetCellValue(this ICell cell, object value, short? dateFormat)
		{
			if (value == null)
			{
				cell.SetCellType(CellType.Blank);
			}
			else if (value is Boolean)
			{
				cell.SetCellValue((bool)value);
			}
			else if (value is DateTime)
			{
				if (((DateTime)value) >= (DateTime.MaxValue.Date))
					cell.SetCellType(CellType.Unknown);
				else
				{
					if (dateFormat != null)
					{
						ICellStyle cellStyle = cell.Sheet.Workbook.CreateCellStyle();
						cellStyle.CloneStyleFrom(cell.CellStyle);
						cellStyle.DataFormat = dateFormat.Value;

						cell.CellStyle = cellStyle;
					}
				}

				cell.SetCellValue((DateTime)value);
			}
			else if (Utility.IsNumeric(value))
			{
				cell.SetCellValue(Convert.ToDouble(value));
			}
			else if (value is String)
			{
				cell.SetCellValue(value.ToString());
			}
			else
				SetCellValue(cell, Convert.ToString(value), dateFormat);
		}

		/// <summary>
		/// NPOI missing the ShiftedRows height, waiting offical fix.  just rewrite here
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="sourceRowIndex"></param>
		/// <param name="targetRowIndex"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
        public static IRow CopyRow(ISheet sheet, int sourceRowIndex, int targetRowIndex)
        {
            if (sourceRowIndex == targetRowIndex)
                throw new ArgumentException("sourceIndex and targetIndex cannot be same");
            // Get the source / new row
            IRow newRow = sheet.GetRow(targetRowIndex);
            IRow sourceRow = sheet.GetRow(sourceRowIndex);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                sheet.ShiftRows(targetRowIndex, sheet.LastRowNum, 1, true, false);
            }
            newRow = sheet.CreateRow(targetRowIndex);
            newRow.Height = sourceRow.Height;   //copy row height

            // Loop through source columns to add to new row
            for (int i = sourceRow.FirstCellNum; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    continue;
                }
                ICell newCell = newRow.CreateCell(i);

                if (oldCell.CellStyle != null)
                {
                    // apply style from old cell to new cell 
                    newCell.CellStyle = oldCell.CellStyle;
                }

                // If there is a cell comment, copy
                //if (oldCell.CellComment != null)
                //{
                //    sheet.CopyComment(oldCell, newCell);
                //}

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null)
                {
                    newCell.Hyperlink = oldCell.Hyperlink;
                }

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.Blank:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.Error:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.Formula:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                }
            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = sheet.GetMergedRegion(i);
                if (cellRangeAddress != null && cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                            (newRow.RowNum +
                                    (cellRangeAddress.LastRow - cellRangeAddress.FirstRow
                                            )),
                            cellRangeAddress.FirstColumn,
                            cellRangeAddress.LastColumn);
                    sheet.AddMergedRegion(newCellRangeAddress);
                }
            }
            return newRow;
        }
    }
}

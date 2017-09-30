using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HKH.Exchange.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HKH.Exchange.Common;

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
	}
}

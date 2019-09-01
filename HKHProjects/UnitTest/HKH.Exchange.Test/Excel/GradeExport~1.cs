using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HKH.Exchange.Configuration;
using HKH.Exchange.Excel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace HKH.Exchange.Test
{
	public class GradeExport_1 : NPOIExportList<Grade>
	{
		public GradeExport_1(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected override void CustomTableHeader(ISheet sheet)
		{
			Export export = GetExport(ExportId);
			IRow titleRow = sheet.CreateRow(NextRowNum());
			ICell cell = titleRow.CreateCell(0);
			sheet.AddMergedRegion(new CellRangeAddress(titleRow.RowNum, titleRow.RowNum, 0, export.Body.MaxColumnIndex));

			ICellStyle style = workBook.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			IFont font = workBook.CreateFont();
			font.Color = HSSFColor.Red.Index;
			font.FontName = "宋体";
			font.FontHeight = 20 * 20;
			font.Boldweight = 10;
			font.IsItalic = true;

			style.SetFont(font);
			cell.CellStyle = style;
			cell.SetCellValue("成绩导出");
		}

		protected override void CustomTableFooter(ISheet sheet)
		{
			Export export = GetExport(ExportId);
			IRow endRow = sheet.CreateRow(NextRowNum());
			ICell cell = endRow.CreateCell(0);
			sheet.AddMergedRegion(new CellRangeAddress(endRow.RowNum, endRow.RowNum, 0, export.Body.MaxColumnIndex));

			ICellStyle style = workBook.CreateCellStyle();

			style.Alignment = HorizontalAlignment.Right;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.FillForegroundColor = HSSFColor.Green.Index;
			style.FillPattern = FillPattern.AltBars;
			style.FillBackgroundColor = HSSFColor.Green.Index;
			cell.CellStyle = style;

			cell.SetCellValue(DateTime.Today, dateFormat);
		}
	}
}

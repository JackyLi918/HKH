/*******************************************************
 * Filename: ExportWrapperTest.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/20/2015 3:05:03 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using HKH.Exchange.Excel;
using HKH.Exchange.Common;

namespace HKH.Exchange.Test
{
    /// <summary>
    /// ExportWrapperTest
    /// </summary>
    [TestClass]
    public class ExportWrapperTest
    {
        #region Variables

        string configFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\GradeImport.xml";
        string tableId = "1";
        string excelFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\testData.xls";

        #endregion

        #region Methods
        [TestMethod]
        public void TestExport_1()
        {
            List<Grade> gradeList = new List<Grade> { new Grade { ClassId = "一班", StudentId = "1", Html = 60 }, new Grade { ClassId = "二班", StudentId = "1", Java = 70 } };
            ExportWrapper.ExportExcel<Grade, List<Grade>>(configFile, tableId, "2", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\export.xls", gradeList,
                tableHeaderWriter: CustomTableHeader, tableFooterWriter: CustomTableFooter);
            ExportWrapper.ExportExcel<Grade, List<Grade>>(configFile, tableId, "3", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\export.xlsx", gradeList,
                tableHeaderWriter: CustomTableHeader, tableFooterWriter: CustomTableFooter);
        }

        [TestMethod]
        public void TestFill_1()
        {
            Student stu = new Student { ClassName = "软件研发部", Name = "库管员", Birthday = DateTime.Today, Name2 = "测试人员" };
            List<Grade> gradeList = new List<Grade> { new Grade { Name = "DVD光盘", Brand = "紫光", UnitName = "个", Quantity = 10, Price = 5.0 }, new Grade { Name = "计算机", Brand = "Dell", UnitName = "台", Quantity = 1, Price = 5000.0 } ,
                                  new Grade { Name = "硬盘", Brand = "华硕", Spec ="2T", UnitName = "个", Quantity = 1, Price = 600.0 }, new Grade { Name = "液晶显示器", Brand = "Dell", Spec="21寸宽屏", UnitName = "台", Quantity = 1, Price = 2000.0 }};
            stu.Total = gradeList.Sum(g => g.Quantity * g.Price);

            ExportWrapper.FillExcel<Grade, List<Grade>, Student>(configFile, tableId, "1", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill.xls", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill1.xls", gradeList, stu);
            ExportWrapper.FillExcel<Grade, List<Grade>, Student>(configFile, tableId, "1", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill.xlsx", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill1.xlsx", gradeList, stu);

            ExportWrapper.ExportCSV<Grade, List<Grade>>(configFile, tableId, "3", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\csv1.csv", gradeList);
        }
        #endregion

        #region Helper
        private void CustomTableHeader(object sender, TableWritingEventArgs args)
        {
            IExportable expObj = sender as IExportable;

            IRow titleRow = args.Sheet.CreateRow(expObj.NextRowNum());
            ICell cell = titleRow.CreateCell(0);
            args.Sheet.AddMergedRegion(new CellRangeAddress(titleRow.RowNum, titleRow.RowNum, 0, args.Export.Body.MaxColumnIndex));

            ICellStyle style = args.Sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = args.Sheet.Workbook.CreateFont();
            font.Color = HSSFColor.Red.Index;
            font.FontName = "宋体";
            font.FontHeight = 20 * 20;
            font.Boldweight = 10;
            font.IsItalic = true;

            style.SetFont(font);
            cell.CellStyle = style;
            cell.SetCellValue("成绩导出");
        }

        private void CustomTableFooter(object sender, TableWritingEventArgs args)
        {
            IExportable expObj = sender as IExportable;

            IRow endRow = args.Sheet.CreateRow(expObj.NextRowNum());
            ICell cell = endRow.CreateCell(0);
            args.Sheet.AddMergedRegion(new CellRangeAddress(endRow.RowNum, endRow.RowNum, 0, args.Export.Body.MaxColumnIndex));

            ICellStyle style = args.Sheet.Workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.FillForegroundColor = HSSFColor.Green.Index;
            style.FillPattern = FillPattern.AltBars;
            style.FillBackgroundColor = HSSFColor.Green.Index;

            cell.SetCellValue(DateTime.Today, expObj.ExcelDateFormat);
        }
        #endregion
    }
}
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.XSSF.UserModel;

namespace HKH.Exchange.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            XSSFWorkbook workbook;
            using (Stream stream = File.Open("example.xlsx", FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(stream);
                stream.Close();
            }

            using (Stream stream = File.Create("example1.xlsx"))
            {
                var sheet = workbook.GetSheetAt(0);
                sheet.CreateRow(0).CreateCell(0).SetCellValue("Test PivotalTable");

                workbook.Write(stream);
                if (stream.CanWrite)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
        }
    }
}

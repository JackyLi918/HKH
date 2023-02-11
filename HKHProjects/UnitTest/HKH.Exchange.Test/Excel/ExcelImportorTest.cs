using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Data;

namespace HKH.Exchange.Test
{
	[TestClass]
	public class ExcelImportorTest
	{
		string configFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\GradeImport.xml";
		string tableId = "1";
        string excelFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\testdata.xls";
		string csvFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\testcsv.csv";

        [TestMethod]
        public void TestConfiguration()
		{
            var config = Exchange.Configuration.ExchangeConfiguration.Load(configFile);
            config.Write("a.json");
        }

        [TestMethod]
		public void TestImport()
		{
			System.Data.DataTable dt = new dsGrade().Grade;
            GradecsvImportor gImport = new GradecsvImportor(configFile, tableId,"2");
			gImport.Import(csvFile, dt, null);
			Assert.AreEqual(66, dt.Rows.Count);

			gImport = new GradecsvImportor(configFile, tableId, "3");
			dt = new dsGrade().Grade;
			gImport.Import(csvFile, dt, null);
			Assert.AreEqual(66, dt.Rows.Count);

			//DataProvider.GetInstance().Update(dt);
			List<Grade> success= new List<Grade>();
			List<Grade> fail = new List<Grade>();
			HKH.Exchange.Common.ImportWrapper.ImportCsv<Grade, List<Grade>>(configFile, tableId, "2", csvFile, success, fail,false);
			Assert.AreEqual(success.Count, 66);
			Assert.AreEqual(fail.Count, 0);
		}

		[TestMethod]
		public void TestImport_1()
		{
			IList<Grade> gradeList = new List<Grade>();
			GradeImportor_1 gImport = new GradeImportor_1(configFile, tableId);
			gImport.Import(excelFile, gradeList, null);
			Update(gradeList);
		}

		[TestMethod]
		public void TestExport_1()
		{
			List<Grade> gradeList = new List<Grade> { new Grade { ClassId = "一班", StudentId = "1", Html = 60 }, new Grade { ClassId = "二班", StudentId = "1", Java = 70 } };
			GradeExport_1 gExport = new GradeExport_1(configFile, tableId);
			gExport.ExportId = "2";
			gExport.Export(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\export.xls", gradeList);
		}

		[TestMethod]
		public void TestFill_1()
		{
			Student stu = new Student { ClassName = "软件研发部", Name = "库管员", Birthday = DateTime.Today, Name2 = "测试人员" };
			List<Grade> gradeList = new List<Grade> { new Grade { Name = "DVD光盘", Brand = "紫光", UnitName = "个", Quantity = 10, Price = 5.0 }, new Grade { Name = "计算机", Brand = "Dell", UnitName = "台", Quantity = 1, Price = 5000.0 }
			, new Grade { Name = "硬盘", Brand = "华硕", Spec ="2T", UnitName = "个", Quantity = 1, Price = 600.0 }, new Grade { Name = "液晶显示器", Brand = "Dell", Spec="21寸宽屏", UnitName = "台", Quantity = 1, Price = 2000.0 }};
			stu.Total = gradeList.Sum(g => g.Quantity * g.Price);

			GradeExport_1 gExport = new GradeExport_1(configFile, tableId);
			gExport.ExportId = "1";
			gExport.Fill<Student>(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill.xls", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill1.xls", gradeList, stu);
			gExport.Fill<Student>(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill.xlsx", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\fill1.xlsx", gradeList, stu);
		}

		private void Update(IList<Grade> list)
		{
			//foreach (Grade grade in list)
			//{
			//	DataProvider.GetInstance().ExecuteNonQuery("Insert into Grade values(@ClassId,@StudentId,@Net,@Java,@Html)",
			//		new System.Data.IDbDataParameter[]{
   //                     DataProvider.GetInstance().CreateParameter("@ClassId",grade.ClassId),
   //                     DataProvider.GetInstance().CreateParameter("@StudentId",grade.StudentId),
   //                     DataProvider.GetInstance().CreateParameter("@Net",grade.Net),
   //                     DataProvider.GetInstance().CreateParameter("@Java",grade.Java),
   //                     DataProvider.GetInstance().CreateParameter("@Html",grade.Html)
   //                });
			//}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HKH.Exchange;
using HKH.Exchange.Excel;
using HKH.Data.Configuration;
using HKH.Data;

namespace HKH.Exchange.Test
{
	class GradeImportor : HKH.Exchange.Excel.NPOIImportDataTable
	{
		public GradeImportor(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected override bool ValidateSourceData(NPOI.SS.UserModel.IRow row, NPOI.SS.UserModel.ISheet sheet)
		{
			//验证有效数据
			object stuId = row.GetCell(0).GetCellValue();
			if (stuId == null || Convert.IsDBNull(stuId) || "学生".Equals(stuId))
				return false;
			return true;
		}

		protected override bool ValidateTargetData(DataRow tModel, DataTable tList)
		{
			//验证重复
			if (tList.Rows.Find(new object[] { tModel["ClassId"], tModel["StudentId"] }) != null)
				return false;
			return true;
		}
	}
}

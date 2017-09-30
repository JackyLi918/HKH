using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.Test
{
    class GradecsvImportor : HKH.Exchange.CSV.CSVImportDataTable
    {
        public GradecsvImportor(string configurationFile, string tableID)
            : base(configurationFile, tableID)
        {
        }
        public GradecsvImportor(string configurationFile, string tableID,string ImportID)
            : base(configurationFile, tableID, ImportID)
        {
        }
        protected override bool ValidateSourceData(DataRow row,DataTable sheet)
        {
            //验证有效数据
            //object stuId = row.GetCell(0).GetCellValue();
            //if (stuId == null || Convert.IsDBNull(stuId) || "学生".Equals(stuId))
            //    return false;
            return true;
        }

        protected override bool ValidateTargetData(DataRow tModel, DataTable tList)
        {
            //验证重复
            //if (tList.Rows.Find(new object[] { tModel["ClassId"], tModel["StudentId"] }) != null)
            //    return false;
            return true;
        }
    }
}

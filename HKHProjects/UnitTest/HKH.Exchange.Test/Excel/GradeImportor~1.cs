using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HKH.Exchange;
using HKH.Data.Configuration;
using HKH.Data;
using HKH.Exchange.Excel;

namespace HKH.Exchange.Test
{
    class GradeImportor_1 : HKH.Exchange.Excel.NPOIImportList<Grade>
    {
        public GradeImportor_1(string configurationFile, string tableID)
            : base(configurationFile, tableID)
        {
        }

        protected override bool ValidateSourceData(NPOI.SS.UserModel.IRow row, NPOI.SS.UserModel.ISheet sheet)
        {
            //验证有效数据
            object stuId =  row.GetCell(0).GetCellValue();
            if (stuId == null || Convert.IsDBNull(stuId) || "学生".Equals(stuId))
                return false;
            return true;
        }

        protected override bool ValidateTargetData(Grade tModel, IList<Grade> tList)
        {
            return !tList.Contains(tModel);
        }
    }
}

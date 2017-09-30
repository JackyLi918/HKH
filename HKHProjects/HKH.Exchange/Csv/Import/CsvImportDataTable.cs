using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.CSV
{
    /// <summary>
	/// load from excel file to dataset
	/// </summary>
	public abstract class CSVImportDataTable : CSVImportBase<DataRow, DataTable>
	{
		#region Constructor

		public CSVImportDataTable(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected CSVImportDataTable(string configurationFile, string tableID, string importId)
			: base(configurationFile, tableID, importId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override DataRow GetTInstance()
		{
			return successList.NewRow();
		}

		protected override void SetValue(DataRow tModel, string propertyName, object value)
		{
			tModel[propertyName] = value;
		}

		protected override void AppendToTList(DataRow tModel, DataTable tList)
		{
			if (tList != null)
				tList.Rows.Add(tModel);
		}

		#endregion
	}
}



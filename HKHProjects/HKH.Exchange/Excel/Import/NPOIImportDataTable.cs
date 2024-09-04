using System.Data;

namespace HKH.Exchange.Excel
{
	public abstract class NPOIImportDataTable : NPOIImportBase<DataRow, DataTable>
	{
		#region Constructor

		public NPOIImportDataTable(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected NPOIImportDataTable(string configurationFile, string tableID, string importId)
			: base(configurationFile, tableID, importId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override DataRow GetTInstance()
		{
			return successList.NewRow();
		}

		protected override void SetValue(DataRow tObj, string propertyName, object value)
		{
			tObj[propertyName] = value;
		}

		protected override void AppendToTList(DataRow tObj, DataTable tList)
		{
			if (tList != null)
				tList.Rows.Add(tObj);
		}

		#endregion
	}
}

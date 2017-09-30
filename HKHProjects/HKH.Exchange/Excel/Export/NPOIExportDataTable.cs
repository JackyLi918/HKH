using System.Data;

namespace HKH.Exchange.Excel
{
	public class NPOIExportDataTable : NPOIExportBase<DataRow, DataTable>
	{
		#region Constructor

		public NPOIExportDataTable(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected NPOIExportDataTable(string configurationFile, string tableID, string exportId)
			: base(configurationFile, tableID, exportId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override int GetCount(DataTable tList)
		{
			return tList.Rows.Count;
		}

		protected override DataRow GetTObject(DataTable tList, int index)
		{
			return tList.Rows[index];
		}

		protected override object GetValue(DataRow tModel, string propertyName)
		{
			return tModel[propertyName];
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;

namespace HKH.Exchange.Excel
{
	public class NPOIExportList<T> : NPOIExportBase<T, IList<T>> where T : class
	{
		#region Constructor

		public NPOIExportList(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected NPOIExportList(string configurationFile, string tableID, string exportId)
			: base(configurationFile, tableID, exportId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override int GetCount(IList<T> tList)
		{
			return tList.Count;
		}

		protected override T GetTObject(IList<T> tList, int index)
		{
			return tList[index];
		}

		protected override object GetValue(T tModel, string propertyName)
		{
			return tModel.GetValue(propertyName);
		}

		#endregion
	}
}

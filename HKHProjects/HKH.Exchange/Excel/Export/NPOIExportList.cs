using System;
using System.Collections.Generic;

namespace HKH.Exchange.Excel
{
	public class NPOIExportList<TBody> : NPOIExportBase<TBody, IList<TBody>> where TBody : class
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

		protected override int GetCount(IList<TBody> tList)
		{
			return tList.Count;
		}

		protected override TBody GetTObject(IList<TBody> tList, int index)
		{
			return tList[index];
		}

		protected override object GetValue(TBody tObj, string propertyName)
		{
			return tObj.GetValue(propertyName);
		}

		#endregion
	}
}

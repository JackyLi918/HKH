using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Exchange.Excel
{
	public abstract class NPOIImportList<T> : NPOIImportBase<T, IList<T>> where T : class, new()
	{
		#region Constructor

		public NPOIImportList(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

		protected NPOIImportList(string configurationFile, string tableID, string importId)
			: base(configurationFile, tableID, importId)
		{
		}

		#endregion

		#region Base Class Overrides

		protected override T GetTInstance()
		{
			return new T();
		}

		protected override void SetValue(T tObj, string propertyName, object value)
		{
			tObj.SetValue(propertyName, value, true);
		}

		protected override void AppendToTList(T tObj, IList<T> tList)
		{
			if (tList != null)
				tList.Add(tObj);
		}

		#endregion
	}
}

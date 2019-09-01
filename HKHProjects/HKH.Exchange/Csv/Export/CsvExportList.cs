/*******************************************************
 * Filename: CsvExportList.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/19/2015 12:08:34 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.CSV
{
	/// <summary>
	/// CsvExportList
	/// </summary>
    public class CSVExportList<TBody> : CSVExportBase<TBody, IList<TBody>> where TBody : class
	{
		#region Constructor

		public CSVExportList(string configurationFile, string tableID)
			: base(configurationFile, tableID)
		{
		}

        protected CSVExportList(string configurationFile, string tableID, string exportId)
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
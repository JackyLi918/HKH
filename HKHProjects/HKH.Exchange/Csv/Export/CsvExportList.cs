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
    public class CSVExportList<T> : CSVExportBase<T, IList<T>> where T : class
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
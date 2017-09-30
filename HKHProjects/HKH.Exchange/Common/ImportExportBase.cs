using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.Common
{
	public abstract class ImportExportBase<T, TList> : IDisposable 
		where T : class
		where TList : class
	{
		#region Protected Variable

		protected string configurationFile;
		protected string _tableId;
		protected TableMapping _tableMapping;

		#endregion

		#region Constructor

		protected ImportExportBase(string configurationFile, string tableID)
		{
			this.configurationFile = configurationFile;
			this._tableId = tableID;
			_tableMapping = ExchangeConfiguration.GetTableMapping(configurationFile, tableID);
		}

		#endregion

		#region Property

		public string ConfigurationFile
		{
			get
			{
				return configurationFile;
			}
		}

		public string TableId
		{
			get
			{
				return _tableId;
			}
			set
			{
				_tableId = value;
				_tableMapping = ExchangeConfiguration.GetTableMapping(configurationFile, _tableId);
			}
		}

		public Export GetExport(string exportId)
		{
			return _tableMapping.Exports[exportId];
		}

		public Import GetImport(string importId)
		{
			return _tableMapping.Imports[importId];
		}

		#endregion

		#region Protected Virtual Methods

		#endregion

		public virtual void Dispose()
		{
		}
	}
}

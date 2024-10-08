﻿using System;
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
        protected ImportExportBase() : this(null, null)
        {

        }
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
        }

        protected Export GetExport(string exportId)
        {
            if (_tableMapping == null || string.IsNullOrEmpty(exportId) || exportId == "0")
                return ExchangeConfiguration.BuildDefaultExport<T>();
            else
                return _tableMapping.Exports[exportId];
        }

        protected Import GetImport(string importId)
        {
            if (_tableMapping == null || string.IsNullOrEmpty(importId) || importId == "0")
            {
                return null;
            }
            else
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

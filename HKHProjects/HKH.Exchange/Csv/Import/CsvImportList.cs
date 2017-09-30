using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.CSV
{
    public abstract class CSVImportList<T> : CSVImportBase<T, IList<T>> where T : class, new()
    {
        #region Constructor

        public CSVImportList(string configurationFile, string tableID)
            : base(configurationFile, tableID)
        {
        }

		protected CSVImportList(string configurationFile, string tableID, string importId)
            : base(configurationFile, tableID, importId)
        {
        }

        #endregion

        #region Base Class Overrides

        protected override T GetTInstance()
        {
            return new T();
        }

        protected override void SetValue(T tModel, string propertyName, object value)
        {
			tModel.SetValue(propertyName, value,true );
        }

        protected override void AppendToTList(T tModel, IList<T> tList)
        {
            if (tList != null)
                tList.Add(tModel);
        }

        #endregion
    }
}

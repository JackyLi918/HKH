/*******************************************************
 * Filename: DbContext.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 1:31:44 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Data;

namespace HKH.Linq.Data
{
    /// <summary>
    /// DbContext
    /// </summary>
    public abstract class DbContext
    {
        private IEntityProvider provider;

        protected DbContext()
        {

        }
        protected DbContext(IEntityProvider provider)
        {
            this.provider = provider;
        }

        #region Variables

        #endregion

        #region Properties

        public IEntityProvider Provider
        {
            get { return this.provider; }
            protected set { this.provider = value; }
        }

        #endregion

        #region Methods
        protected IEntityTable<T> GetTable<T>(string tableId = null)
        {
            return this.provider.GetTable<T>(tableId);
        }

        #endregion

        #region Helper

        #endregion
    }
}
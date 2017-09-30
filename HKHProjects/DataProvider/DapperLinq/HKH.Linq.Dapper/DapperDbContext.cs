/*******************************************************
 * Filename: DapperDbContext.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 1:59:09 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Data;

namespace HKH.Linq.Data.Dapper
{
    /// <summary>
    /// DapperDbContext
    /// </summary>
    public abstract class DapperDbContext : DbContext
    {
        #region Variables

        #endregion

        protected DapperDbContext()
        { }
        protected DapperDbContext(DapperEntityProvider provider)
            : base(provider)
        {
        }

        #region Properties

        public IDataProvider DataProvider
        {
            get { return (this.Provider as DapperEntityProvider).DataProvider; }
        }

        #endregion

        #region Methods

        #endregion

        #region Helper

        #endregion
    }
}
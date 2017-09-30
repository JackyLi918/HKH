/*******************************************************
 * Filename: SqlServerDbContext.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 2:02:49 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Data;
using HKH.Linq.Data.Common;
using HKH.Linq.Data.Dapper;
using HKH.Linq.Data.Mapping;

namespace HKH.Linq.Data.SqlServer
{
    /// <summary>
    /// SqlServerDbContext
    /// </summary>
    public class SqlServerDbContext : DapperDbContext
    {
        #region Variables

        #endregion

        protected SqlServerDbContext()
        { }

        public SqlServerDbContext(IDataProvider dataProvider, QueryMapping mapping = null, QueryPolicy policy = null)
            : base(new SqlServerEntityProvider(dataProvider, mapping ?? new ImplicitMapping(), policy ?? QueryPolicy.Default))
        {
        }

        public SqlServerDbContext(SqlServerEntityProvider provider)
            : base(provider)
        {
        }

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region Helper

        #endregion
    }
}
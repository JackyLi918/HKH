/*******************************************************
 * Filename: SqlServerEntityProvider.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 1:18:03 PM
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

namespace HKH.Linq.Data.SqlServer
{
    /// <summary>
    /// SqlServerEntityProvider
    /// </summary>
    public class SqlServerEntityProvider : DapperEntityProvider
    {
        public SqlServerEntityProvider(IDataProvider dataProvider, QueryMapping mapping, QueryPolicy policy)
            : base(dataProvider, TSqlLanguage.Default, mapping, policy)
        {
        }
    }
}
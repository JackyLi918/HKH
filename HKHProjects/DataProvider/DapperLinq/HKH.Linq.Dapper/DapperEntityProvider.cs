/*******************************************************
 * Filename: DapperEntityProvider.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 1:09:50 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Data;
using HKH.Linq.Data;
using HKH.Linq.Data.Common;

namespace HKH.Linq.Data.Dapper
{
    /// <summary>
    /// DapperEntityProvider
    /// </summary>
    public abstract class DapperEntityProvider : EntityProvider
    {
        private IDataProvider dataProvider;

        public DapperEntityProvider(IDataProvider dataProvider, QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
            : base(language, mapping, policy)
        {
            this.dataProvider = dataProvider;
        }

        public IDataProvider DataProvider
        {
            get { return this.dataProvider; }
            set { this.dataProvider = value; }
        }

        public override QueryExecutor CreateExecutor()
        {
            return new DapperExecutor(this);
        }
    }
}
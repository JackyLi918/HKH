/*******************************************************
 * Filename: DapperExecutor.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/27/2017 10:30:14 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HKH.Data;
using HKH.Data.Dapper;

namespace HKH.Linq.Data.Dapper
{
    using System.Collections;
    using System.Dynamic;
    using System.Threading;
    using Common;

    /// <summary>
    /// DapperExecutor
    /// </summary>
    public class DapperExecutor : QueryExecutor
    {
        DapperEntityProvider provider;
        int rowsAffected;

        public DapperExecutor(DapperEntityProvider provider)
        {
            this.provider = provider;
        }

        public EntityProvider Provider
        {
            get { return this.provider; }
        }

        public override int RowsAffected
        {
            get { return this.rowsAffected; }
        }

        public virtual bool BufferResultRows
        {
            get { return false; }
        }

        public override IEnumerable<T> Execute<T>(QueryCommand query, object[] paramValues)
        {
            if (query.Parameters.Count == 0)
                return provider.DataProvider.Query<T>(query.CommandText);
            else
                return provider.DataProvider.Query<T>(query.CommandText, CreateDynamicParameters(query, paramValues));
        }

        public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream)
        {
            var batch = new DbBatchCommandExecutable(this, query, paramSets.ToArray());
            if (!stream)
            {
                return new BufferedEnumerable<int>(batch);
            }
            else
            {
                return batch;
            }
        }

        public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream)
        {
            var batch = new DbBatchQueryExecutable<T>(this, query, paramSets.ToArray());
            if (!stream)
            {
                return new BufferedEnumerable<T>(batch);
            }
            else
            {
                return batch;
            }
        }

        public override int ExecuteCommand(QueryCommand query, object[] paramValues)
        {
            if (query.Parameters.Count == 0)
                this.rowsAffected = provider.DataProvider.Execute(query.CommandText);
            else
                this.rowsAffected = provider.DataProvider.Execute(query.CommandText, CreateDynamicParameters(query, paramValues));

            return this.rowsAffected;
        }

        public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, object[] paramValues)
        {
            return new BufferedEnumerable<T>(Execute<T>(query, paramValues));
        }

        private SqlDynamicParameters CreateDynamicParameters(QueryCommand query, object[] paramValues)
        {
            IDataProvider dataProvider = provider.DataProvider;
            IDbDataParameter[] parameters = new IDbDataParameter[query.Parameters.Count];

            for (int i = 0; i < query.Parameters.Count; i++)
            {
                parameters[i] = dataProvider.CreateParameter(query.Parameters[i].Name);
                parameters[i].Value = paramValues[i] ?? DBNull.Value;
            }

            return new SqlDynamicParameters(parameters);
        }
    }
}
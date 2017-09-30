/*******************************************************
 * Filename: DbBatchCommandExecutable.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	4/6/2017 12:30:44 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using HKH.Linq.Data.Common;
using HKH.Data.Dapper;

namespace HKH.Linq.Data.Dapper
{
    /// <summary>
    /// DbBatchCommandExecutable
    /// </summary>
    public class DbBatchCommandExecutable : IAsyncEnumerable<int>
    {
        private readonly DapperExecutor executor;
        private readonly QueryCommand query;
        private readonly object[][] paramSets;

        public DbBatchCommandExecutable(DapperExecutor executor, QueryCommand query, object[][] paramSets)
        {
            this.executor = executor;
            this.query = query;
            this.paramSets = paramSets;
        }

        public Task<IAsyncEnumerator<int>> GetEnumeratorAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<IAsyncEnumerator<int>>(new Enumerator(this));
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class Enumerator : IAsyncEnumerator<int>, IEnumerator<int>, IDisposable
        {
            private readonly DbBatchCommandExecutable parent;
            private int paramSet;
            private int current;
            private CommandDefinition dapperCmd;

            public Enumerator(DbBatchCommandExecutable parent)
            {
                this.parent = parent;
                this.paramSet = -1;
            }

            public int Current
            {
                get { return this.current; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public async Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (this.paramSet < 0)
                {
                    this.paramSet++;
                    dapperCmd = new CommandDefinition(this.parent.query.CommandText, CreateDynamicParameters(this.parent.query, this.parent.paramSets), cancellationToken: cancellationToken);
                    this.current = await (this.parent.executor.Provider as DapperEntityProvider).DataProvider.ExecuteAsync(dapperCmd).ConfigureAwait(false);
                    return true;
                }
                else
                {
                    this.Dispose();
                    return false;
                }
            }

            public bool MoveNext()
            {
                if (this.paramSet < 0)
                {
                    this.paramSet++;
                    dapperCmd = new CommandDefinition(this.parent.query.CommandText, CreateDynamicParameters(this.parent.query, this.parent.paramSets));
                    this.current = (this.parent.executor.Provider as DapperEntityProvider).DataProvider.Execute(dapperCmd);
                    return true;
                }
                else
                {
                    this.Dispose();
                    return false;
                }
            }
            protected dynamic CreateDynamicParameters(QueryCommand query, object[][] paramValues)
            {
                object[] parameters = new object[paramValues.Length];

                for (int i = 0; i < paramValues.Length; i++)
                {
                    object[] paramValue = paramValues[i];
                    IDictionary<string, object> d = new ExpandoObject();

                    for (int j = 0; j < query.Parameters.Count; j++)
                    {
                        d.Add(query.Parameters[j].Name, paramValue[i] ?? DBNull.Value);
                    }

                    parameters[i] = d;
                }
                return parameters;
            }
            public void Dispose()
            {
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }

}
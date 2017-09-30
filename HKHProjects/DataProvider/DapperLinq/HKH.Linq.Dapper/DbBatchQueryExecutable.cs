/*******************************************************
 * Filename: DbBatchQueryExecutable.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	4/6/2017 12:32:14 PM
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
    /// DbBatchQueryExecutable
    /// </summary>
    public class DbBatchQueryExecutable<T> : IAsyncEnumerable<T>
    {
        private readonly DapperExecutor executor;
        private readonly QueryCommand query;
        private readonly object[][] paramSets;

        public DbBatchQueryExecutable(DapperExecutor executor, QueryCommand query, object[][] paramSets)
        {
            this.executor = executor;
            this.query = query;
            this.paramSets = paramSets;
        }

        public Task<IAsyncEnumerator<T>> GetEnumeratorAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<IAsyncEnumerator<T>>(new Enumerator(this));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class Enumerator : IAsyncEnumerator<T>, IEnumerator<T>, IDisposable
        {
            private readonly DbBatchQueryExecutable<T> parent;
            private T current;
            private SqlMapper.GridReader dataReader;
            private IAsyncEnumerator<T> asyncEnumerator;

            public Enumerator(DbBatchQueryExecutable<T> parent)
            {
                this.parent = parent;
            }

            public T Current
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

                if (this.dataReader == null)
                {
                    try
                    {
                        CommandDefinition dapperCmd = new CommandDefinition(this.parent.query.CommandText, CreateDynamicParameters(this.parent.query, this.parent.paramSets), cancellationToken: cancellationToken);
                        this.dataReader = await (this.parent.executor.Provider as DapperEntityProvider).DataProvider.QueryMultipleAsync(dapperCmd).ConfigureAwait(false);
                        asyncEnumerator = await (await this.dataReader.ReadAsync<T>().ConfigureAwait(false)).ToAsync().GetEnumeratorAsync(cancellationToken);
                    }
                    finally
                    {
                        if (this.dataReader == null)
                        {
                            this.Dispose();
                        }
                    }
                }

                if (asyncEnumerator != null)
                {
                    if (await asyncEnumerator.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    {
                        this.current = asyncEnumerator.Current;
                        return true;
                    }
                    else
                    {
                        this.Dispose();
                        return false;
                    }
                }
                else
                {
                    this.Dispose();
                    return false;
                }
            }

            public bool MoveNext()
            {
                if (this.dataReader == null)
                {
                    try
                    {
                        CommandDefinition dapperCmd = new CommandDefinition(this.parent.query.CommandText, CreateDynamicParameters(this.parent.query, this.parent.paramSets));
                        this.dataReader = (this.parent.executor.Provider as DapperEntityProvider).DataProvider.QueryMultiple(dapperCmd);
                        asyncEnumerator = this.dataReader.Read<T>().GetEnumerator().ToAsync();
                    }
                    finally
                    {
                        if (this.dataReader == null)
                        {
                            this.Dispose();
                        }
                    }
                }

                if (asyncEnumerator != null)
                {
                    if (asyncEnumerator.MoveNext())
                    {
                        this.current = asyncEnumerator.Current;
                        return true;
                    }
                    else
                    {
                        this.Dispose();
                        return false;
                    }
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
                if (dataReader != null)
                {
                    this.dataReader.Dispose();
                }
                if (asyncEnumerator != null)
                {
                    asyncEnumerator.Dispose();
                }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }
}
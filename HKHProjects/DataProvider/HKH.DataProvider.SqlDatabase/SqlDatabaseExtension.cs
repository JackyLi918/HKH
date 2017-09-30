/*******************************************************
 * Filename: SqlDatabaseExtension.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/26/2013 5:44:24 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HKH.Data;
using HKH.Data.Dapper;
using HKH.Data.SqlDatabase.Metadata;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HKH.Data.SqlDatabase
{
	/// <summary>
	/// SqlDatabaseExtension
	/// </summary>
	public static class SqlDatabaseExtension
	{
		/// <summary>
		/// Execute parameterized SQL  
		/// </summary>
		/// <returns>Number of rows affected</returns>
		public static int Execute(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			IDbConnection cnn = transaction == null ? provider.Builder.GetConnection() : transaction.Connection;
			try
			{
				if (transaction == null)
					cnn.Open();
				if (param is IDbDataParameter[])
					return SqlMapper.Execute(cnn, sql, new SqlDynamicParameters(param as IDbDataParameter[]), transaction, commandTimeout, commandType);
				else
					return SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType);
			}
			finally
			{
				if (transaction == null && cnn.State == ConnectionState.Open)
					cnn.Close();
			}
		}

		/// <summary>
		/// Executes a query, returning the data typed as per T
		/// </summary>
		/// <remarks>the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new [space] get new object</remarks>
		/// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
		/// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static IEnumerable<T> Query<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
			, string federationName = null, string distributionName = null, FedKey key = null, bool filtered = false, bool fanout = false)
		{
			return QueryCore<T>(provider, sql, param, transaction, buffered, commandTimeout, commandType, false, federationName, distributionName, key, filtered, fanout);
		}

		/// <summary>
		/// Execute a command that returns multiple result sets, and access each in turn
		/// </summary>
		public static GridDataReader QueryMultiple(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
			, string federationName = null, string distributionName = null, FedKey key = null, bool filtered = false)
		{
			IDbConnection cnn = transaction == null ? provider.Builder.GetConnection() : transaction.Connection;
			try
			{
				if (transaction == null)
					cnn.Open();
				if (!string.IsNullOrEmpty(federationName) && !string.IsNullOrEmpty(distributionName) && key != null)
					cnn.UseFederation(federationName, distributionName, key, filtered);

				return new GridDataReader(cnn, transaction, SqlMapper.QueryMultiple(cnn, sql, param, transaction, commandTimeout, commandType));
			}
			catch
			{
				if (transaction == null && cnn.State == ConnectionState.Open)
					cnn.Close();
				throw;
			}
		}

		/// <summary>
		/// Execute a query asynchronously using .NET 4.5 Task.
		/// </summary>
		public static IEnumerable<T> QueryAsync<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
			, string federationName = null, string distributionName = null, FedKey key = null, bool filtered = false, bool fanout = false)
		{
			return QueryCore<T>(provider, sql, param, transaction, true, commandTimeout, commandType, true, federationName, distributionName, key, filtered, fanout);
		}

		#region Helper

		private static IEnumerable<T> QueryCore<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, bool isAsync = true
			, string federationName = null, string distributionName = null, FedKey key = null, bool filtered = false, bool fanout = false)
		{
			bool isFed = !string.IsNullOrEmpty(federationName) && (fanout || (!string.IsNullOrEmpty(distributionName) && key != null));
			IDbConnection cnn = transaction == null ? provider.Builder.GetConnection() : transaction.Connection;
			IEnumerable<T> result = null;

			try
			{
				if (transaction == null)
					cnn.Open();

				if (isFed)
				{
					if (fanout)
					{
						result = QueryFanout<T>(provider, sql, param, transaction, buffered, commandTimeout, commandType, isAsync, federationName);
					}
					else//(key != null)
					{
						cnn.UseFederation(federationName, distributionName, key, filtered);
						result = ExecuteQuery<T>(cnn, sql, param, transaction, buffered, commandTimeout, commandType, isAsync);
					}
				}
				else
					result = ExecuteQuery<T>(cnn, sql, param, transaction, buffered, commandTimeout, commandType, isAsync);
			}
			finally
			{
				if (transaction == null && cnn.State == ConnectionState.Open)
					cnn.Close();
			}
			return result;
		}

		private static IEnumerable<T> QueryFanout<T>(IDataProvider provider, string sql, dynamic param, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType? commandType, bool isAsync
			, string federationName = null)
		{
			IEnumerable<T> result = new ConcurrentBag<T>();
			var operationResults = new ConcurrentBag<MemberOperation>();

			//resolve to the federation so we can get the members
			var federatedDB = new FederatedDatabase(provider.Builder.ConnectionString);
			Federation targetFederation;

			if (!federatedDB.Federations.ContainsKey(federationName))
			{
				throw new ArgumentException("Federation does not exist", "federationName");
			}

			targetFederation = federatedDB.Federations[federationName];

			Parallel.ForEach(targetFederation.Members.Select(x => x.Value), new ParallelOptions() { MaxDegreeOfParallelism = IDbExtensions.DEFAULT_MAXTHREADS }, currentMember =>
			{
				Exception processingException = null;

				try
				{
					using (var conn = provider.Builder.GetConnection())
					{
						conn.Open();
						conn.UseFederationMember(currentMember);
						IEnumerable<T> t = ExecuteQuery<T>(conn, sql, param, transaction, buffered, commandTimeout, commandType, isAsync);
						result = result.Concat(t);
						conn.Close();
					}
				}
				catch (Exception ex)
				{
					processingException = ex;
				}
				finally
				{
					operationResults.Add(new MemberOperation()
					{
						Member = currentMember,
						Exception = processingException
					});
				}
			});

			// if any of the memberoperation instances have an exception then we throw 
			if (operationResults.Any(x => x.Exception != null))
			{
				throw new FederationException()
				{
					Operations = operationResults.ToList()
				};
			}
			
			return result;
		}

		private static IEnumerable<T> ExecuteQuery<T>(IDbConnection cnn, string sql, dynamic param, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType? commandType, bool isAsync)
		{
			//TODO: why async not work from today? 9/26
			//if (isAsync)
			//	return SqlMapper.QueryAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType).Result;
			//else
				return SqlMapper.Query<T>(cnn, sql, param, transaction, buffered, commandTimeout, commandType);
		}

		#endregion
	}

	public class GridDataReader : IDisposable
	{
		IDbConnection connection = null;
		IDbTransaction transaction = null;
		SqlMapper.GridReader gridReader = null;
		internal GridDataReader(IDbConnection connection, IDbTransaction transaction, SqlMapper.GridReader gridReader)
		{
			this.connection = connection;
			this.transaction = transaction;
			this.gridReader = gridReader;
		}

		/// <summary>
		/// Read the next grid of results
		/// </summary>
		public IEnumerable<T> Read<T>()
		{
			return gridReader.Read<T>();
		}

		/// <summary>
		/// Read multiple objects from a single recordset on the grid
		/// </summary>
		/// <typeparam name="TFirst"></typeparam>
		/// <typeparam name="TSecond"></typeparam>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="func"></param>
		/// <param name="splitOn"></param>
		/// <returns></returns>
		public IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = "id")
		{
			return gridReader.Read<TFirst, TSecond, TReturn>(func, splitOn);
		}

		/// <summary>
		/// Read multiple objects from a single recordset on the grid
		/// </summary>
		/// <typeparam name="TFirst"></typeparam>
		/// <typeparam name="TSecond"></typeparam>
		/// <typeparam name="TThird"></typeparam>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="func"></param>
		/// <param name="splitOn"></param>
		/// <returns></returns>
		public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = "id")
		{
			return gridReader.Read<TFirst, TSecond, TThird, TReturn>(func, splitOn);
		}

		/// <summary>
		/// Read multiple objects from a single record set on the grid
		/// </summary>
		/// <typeparam name="TFirst"></typeparam>
		/// <typeparam name="TSecond"></typeparam>
		/// <typeparam name="TThird"></typeparam>
		/// <typeparam name="TFourth"></typeparam>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="func"></param>
		/// <param name="splitOn"></param>
		/// <returns></returns>
		public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = "id")
		{
			return gridReader.Read<TFirst, TSecond, TThird, TFourth, TReturn>(func, splitOn);
		}

		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Dispose the grid, closing and disposing both the underlying reader and command.
		/// </summary>
		public void Dispose()
		{
			gridReader.Dispose();
			if (transaction == null && connection.State == ConnectionState.Open)
				connection.Close();
		}
	}
}
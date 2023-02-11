using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace HKH.Data.Dapper
{
	/// <summary>
	/// DataProviderExtension
	/// </summary>
	public static class DataProviderExtension
	{
		#region Execute

		public static int Execute(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return Execute(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static int Execute(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<int>(provider, command.Transaction, conn => SqlMapper.Execute(conn, command));
		}

		public static Task<int> ExecuteAsync(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteAsync(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<int> ExecuteAsync(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<int>>(provider, command.Transaction, conn => SqlMapper.ExecuteAsync(conn, command));
		}

		#endregion

		#region ExecuteReader

		public static IDataReader ExecuteReader(this IDataProvider provider, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteReader(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static IDataReader ExecuteReader(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<IDataReader>(provider, command.Transaction, conn => SqlMapper.ExecuteReader(conn, command));
		}

		public static IDataReader ExecuteReader(this IDataProvider provider, CommandDefinition command, CommandBehavior commandBehavior)
		{
			return ProcessRequest<IDataReader>(provider, command.Transaction, conn => SqlMapper.ExecuteReader(conn, command, commandBehavior));
		}

		public static Task<IDataReader> ExecuteReaderAsync(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteReaderAsync(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<IDataReader> ExecuteReaderAsync(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<IDataReader>>(provider, command.Transaction, conn => SqlMapper.ExecuteReaderAsync(conn, command));
		}

		#endregion

		#region ExecuteScalar

		public static object ExecuteScalar(this IDataProvider provider, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteScalar(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static T ExecuteScalar<T>(this IDataProvider provider, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteScalar<T>(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static object ExecuteScalar(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<object>(provider, command.Transaction, conn => SqlMapper.ExecuteScalar(conn, command));
		}

		public static T ExecuteScalar<T>(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<T>(provider, command.Transaction, conn => SqlMapper.ExecuteScalar<T>(conn, command));

		}

		public static Task<object> ExecuteScalarAsync(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteScalarAsync(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<T> ExecuteScalarAsync<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ExecuteScalarAsync<T>(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<object> ExecuteScalarAsync(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<object>>(provider, command.Transaction, conn => SqlMapper.ExecuteScalarAsync(conn, command));
		}

		public static Task<T> ExecuteScalarAsync<T>(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<T>>(provider, command.Transaction, conn => SqlMapper.ExecuteScalarAsync<T>(conn, command));
		}

		#endregion

		#region Query

		public static IEnumerable<dynamic> Query(this IDataProvider provider, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return Query<dynamic>(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType, buffered));
		}

		public static IEnumerable<T> Query<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return Query<T>(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType, buffered));
		}

		public static IEnumerable<object> Query(this IDataProvider provider, Type type, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<object>>(provider, transaction, conn => SqlMapper.Query(conn, type, sql, param, transaction, buffered, commandTimeout, commandType));
		}

		public static IEnumerable<T> Query<T>(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<IEnumerable<T>>(provider, command.Transaction, conn => SqlMapper.Query<T>(conn, command));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<IEnumerable<TReturn>>(provider, transaction, conn => SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static IEnumerable<dynamic> Query(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return QueryAsync(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<IEnumerable<dynamic>> QueryAsync(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<IEnumerable<dynamic>>>(provider, command.Transaction, conn => SqlMapper.QueryAsync(conn, command));
		}

		public static Task<IEnumerable<T>> QueryAsync<T>(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return QueryAsync<T>(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<IEnumerable<object>> QueryAsync(this IDataProvider provider, Type type, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return QueryAsync(provider, type, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<IEnumerable<T>> QueryAsync<T>(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<IEnumerable<T>>>(provider, command.Transaction, conn => SqlMapper.QueryAsync<T>(conn, command));
		}

		public static Task<IEnumerable<object>> QueryAsync(this IDataProvider provider, Type type, CommandDefinition command)
		{
			return ProcessRequest<Task<IEnumerable<object>>>(provider, command.Transaction, conn => SqlMapper.QueryAsync(conn, type, command));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TThird, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDataProvider provider, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			return ProcessRequest<Task<IEnumerable<TReturn>>>(provider, transaction, conn => SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(conn, sql, map, GetDynamicParameter(param), transaction, buffered, splitOn, commandTimeout, commandType));
		}

		#endregion

		#region QueryMultiple

		public static SqlMapper.GridReader QueryMultiple(this IDataProvider provider, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return QueryMultiple(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static SqlMapper.GridReader QueryMultiple(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<SqlMapper.GridReader>(provider, command.Transaction, conn => SqlMapper.QueryMultiple(conn, command));
		}

		public static Task<SqlMapper.GridReader> QueryMultipleAsync(this IDataProvider provider, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return QueryMultipleAsync(provider, BuildCommandDefinition(sql, param, transaction, commandTimeout, commandType));
		}

		public static Task<SqlMapper.GridReader> QueryMultipleAsync(this IDataProvider provider, CommandDefinition command)
		{
			return ProcessRequest<Task<SqlMapper.GridReader>>(provider, command.Transaction, conn => SqlMapper.QueryMultipleAsync(conn, command));
		}

		#endregion

		private static object GetDynamicParameter(dynamic param)
		{
			if (param is IDbDataParameter[])
				return new SqlDynamicParameters(param as IDbDataParameter[]);
			else
				return param;
		}

		private static CommandDefinition BuildCommandDefinition(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool buffered = true)
		{
			CancellationToken cancellationToken = new CancellationToken();
			return new CommandDefinition(sql, GetDynamicParameter(param), transaction, commandTimeout, commandType, (buffered ? CommandFlags.Buffered : CommandFlags.None), cancellationToken);
		}

		/// <summary>
		/// run all sqlmapper method as delegate
		/// </summary>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="provider"></param>
		/// <param name="transaction"></param>
		/// <param name="dowork"></param>
		/// <returns></returns>
		public static TReturn ProcessRequest<TReturn>(IDataProvider provider, IDbTransaction transaction, Func<IDbConnection, TReturn> dowork)
		{
			IDbConnection conn = transaction == null ? provider.Builder.GetConnection() : transaction.Connection;
			try
			{
				if (transaction == null)
					conn.Open();
				return dowork(conn);
			}
			finally
			{
				if (transaction == null && conn.State == ConnectionState.Open)
					conn.Close();
			}
		}
	}
}

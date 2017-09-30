using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace HKH.Data
{
	public interface IBaseDataBaseObjectBuilder
	{
		#region Members

		/// <summary>
		/// Get the ConnectionString of database
		/// </summary>
		string ConnectionString { get; }

		/// <summary>
		/// Get an IDbConnection object
		/// </summary>
		/// <returns>IDbConnection</returns>
		IDbConnection GetConnection();

		/// <summary>
		/// Get an IDbCommand object
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="commandType"></param>
		/// <param name="parameters"></param>
		/// <returns>IDbCommand</returns>
		IDbCommand GetCommand(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

		/// <summary>
		/// Get a IDbDataAdapter object for select
		/// </summary>
		/// <param name="strSql">strSql</param>
		/// <param name="commandType"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		IDbDataAdapter GetDataAdapter(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

		/// <summary>
		/// Get an IDbDataAdapter object
		/// </summary>
		/// <param name="selectCommand"></param>
		/// <returns></returns>
		IDbDataAdapter GetDataAdapter(IDbCommand selectCommand);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IDbDataParameter GetParameter(string name);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <returns></returns>
		IDbDataParameter GetParameter(string name, int dbTypeCode);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <returns></returns>
		IDbDataParameter GetParameter(string name, string dbTypeName);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <returns></returns>
		IDbDataParameter[] GetParameters(string strSql, params object[] values);

		void Dispose();

		#endregion
	}
}

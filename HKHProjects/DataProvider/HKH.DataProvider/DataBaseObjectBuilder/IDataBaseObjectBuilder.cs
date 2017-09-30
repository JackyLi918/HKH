using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using HKH.Data.Configuration;

namespace HKH.Data
{
	public interface IDataBaseObjectBuilder : IBaseDataBaseObjectBuilder, ICommandBuiler
	{
		#region Members

		/// <summary>
		/// Gets the DataProviderConfiguration of this builder
		/// </summary>
        HKHConnectionStringElement Configuration { get; }

		/// <summary>
		/// Get an IDbCommand object with empty commandtext
		/// </summary>
		/// <returns>IDbCommand</returns>
		IDbCommand GetCommand();

		/// <summary>
		/// Get an IDbCommand object
		/// </summary>
		/// <param name="strSql">sqlStatement</param>
		/// <returns>IDbCommand</returns>
		IDbCommand GetCommand(string strSql);

		/// <summary>
		/// Get an IDbCommand object
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="commandType"></param>
		/// <returns>IDbCommand</returns>
		IDbCommand GetCommand(string strSql, CommandType commandType);

		/// <summary>
		/// Get a IDbDataAdapter object for select
		/// </summary>
		/// <param name="strSql">sqlStatement</param>
		/// <returns>IDbDataAdapter</returns>
		IDbDataAdapter GetDataAdapter();

		/// <summary>
		/// Get a IDbDataAdapter object for select
		/// </summary>
		/// <param name="strSql">strSql</param>
		/// <returns>IDbDataAdapter</returns>
		IDbDataAdapter GetDataAdapter(string strSql);

		/// <summary>
		/// Get a IDbDataAdapter object for select
		/// </summary>
		/// <param name="strSql">strSql</param>
		/// <param name="commandType"></param>
		/// <returns></returns>
		IDbDataAdapter GetDataAdapter(string strSql, CommandType commandType);

		#endregion
	}
}

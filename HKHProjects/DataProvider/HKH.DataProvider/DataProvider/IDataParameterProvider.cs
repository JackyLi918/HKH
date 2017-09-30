using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace HKH.Data
{
	public interface IDataParameterProvider
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, DbType type, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, DbType type, int size, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="precision"></param>
		/// <param name="scale"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value);

		#region Handle non-DbType parameter

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeName"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="precision"></param>
		/// <param name="scale"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <param name="size"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dbTypeCode"></param>
		/// <param name="size"></param>
		/// <param name="direction"></param>
		/// <param name="precision"></param>
		/// <param name="scale"></param>
		/// <param name="srcColumn"></param>
		/// <param name="srcVersion"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value);
		
		#endregion
	}
}

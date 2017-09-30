/*******************************************************
 * Filename: SqlDatabaseObjectBuilder.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/26/2013 3:47:14 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HKH.Data;
using HKH.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HKH.Data.SqlDatabase
{
	/// <summary>
	/// SqlDatabaseObjectBuilder
	/// </summary>
	public class SqlDatabaseObjectBuilder : DataBaseObjectBuilder
	{
		RetryManager _retryManager = null;
		RetryPolicy _connRetryPolicy = null;
		RetryPolicy _cmdRetryPolicy = null;

		#region Constructor

		public SqlDatabaseObjectBuilder(HKHConnectionStringElement configuration)
			: base(configuration)
		{
			InitRetryPolicy();
		}

		public SqlDatabaseObjectBuilder(string connectionString)
			: base(connectionString)
		{
			InitRetryPolicy();
		}

		private void InitRetryPolicy()
		{
			// Get an instance of the RetryManager class.
			_retryManager = _retryManager ?? RetryPolicyFactory.CreateDefault();
			_connRetryPolicy = _retryManager.GetDefaultSqlConnectionRetryPolicy();
			_cmdRetryPolicy = _retryManager.GetDefaultSqlCommandRetryPolicy();
		}

		#endregion

		#region Base Class Overrides

		public override void BuildCommands(IDbDataAdapter adapter)
		{
			new SqlCommandBuilder(adapter as SqlDataAdapter);
		}

		protected override IDbConnection CreateConnection()
		{
			return new ReliableSqlConnection(connectionString, _connRetryPolicy, _cmdRetryPolicy);
		}

		protected override IDbCommand CreateCommand()
		{
			return new SqlCommand();
		}

		protected override IDbDataAdapter CreateDataAdapter()
		{
			return new SqlDataAdapter();
		}

		public override IDbDataParameter GetParameter(string name)
		{
			SqlParameter parameter = new SqlParameter();
			parameter.ParameterName = GetFormattedParameterName(name);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name, string dbTypeName)
		{
			SqlParameter parameter = GetParameter(name) as SqlParameter;
			parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dbTypeName, true);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name, int dbTypeCode)
		{
			SqlParameter parameter = GetParameter(name) as SqlParameter;
			parameter.SqlDbType = (SqlDbType)dbTypeCode;
			return parameter;
		}

		private string partten = @"(?<param>@+[A-Za-z0-9_]+)";
		public override IDbDataParameter[] GetParameters(string strSql, params object[] values)
		{
			List<IDbDataParameter> parameters = new List<IDbDataParameter>();
			Regex regParam = new Regex(partten, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
			foreach (Match paramMatch in regParam.Matches(strSql))
			{
				//ignore global variable
				if (paramMatch.Value.StartsWith("@@"))
					continue;

				//sql-server is IgnoreCase
				if (parameters.FirstOrDefault(param => param.ParameterName.Equals(paramMatch.Value, StringComparison.OrdinalIgnoreCase)) == null)
				{
					parameters.Add(GetParameter(paramMatch.Value));
				}
			}

			if (parameters.Count != values.Length)
				throw new ArgumentException("The count of parameter does not equal to the count of value.");

			for (int i = 0; i < parameters.Count; i++)
				parameters[i].Value = values[i];

			return parameters.ToArray<IDbDataParameter>();
		}

		protected override string GetFormattedParameterName(string name)
		{
			return string.Format("@{0}", name.TrimStart('@'));
		}

		#endregion
	}
}
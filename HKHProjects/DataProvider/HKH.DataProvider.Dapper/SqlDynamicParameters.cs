/*******************************************************
 * Filename: SqlDynamicParameters.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	9/27/2013 1:10:35 PM
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

namespace HKH.Data.Dapper
{
	/// <summary>
	/// SqlDynamicParameters
	/// </summary>
    public class SqlDynamicParameters : SqlMapper.IDynamicParameters
	{
		public SqlDynamicParameters(IDbDataParameter[] parameters)
		{
			this.Parameters = parameters;
		}

		private IDbDataParameter[] Parameters { get; set; }
        public void AddParameters(System.Data.IDbCommand command, SqlMapper.Identity identity)
		{
			command.Parameters.Clear();

			if (Parameters != null)
			{
				foreach (IDbDataParameter parameter in Parameters)
				{
					parameter.Value = parameter.Value ?? DBNull.Value;
					command.Parameters.Add(parameter);
				}
			}
		}
	}
}
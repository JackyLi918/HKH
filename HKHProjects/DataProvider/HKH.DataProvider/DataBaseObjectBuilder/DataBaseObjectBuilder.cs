using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HKH.Data.Configuration;
using System.Text.RegularExpressions;

namespace HKH.Data
{
	public abstract class DataBaseObjectBuilder : BaseDataBaseObjectBuilder, IDataBaseObjectBuilder
	{
		#region Protected Variables

		protected HKHConnectionStringElement configuration = null;

		#endregion

		#region Constructor

		protected DataBaseObjectBuilder(HKHConnectionStringElement configuration)
			: base(configuration.ConnectionString)
		{
			this.configuration = configuration;
		}

		protected DataBaseObjectBuilder(string connectionString)
			: base(connectionString)
		{
			this.configuration = HKHConnectionStringElement.Null;
		}

		#endregion

		#region IDataBaseObjectBuilder Members

		public HKHConnectionStringElement Configuration
		{
			get { return configuration; }
		}

		public IDbCommand GetCommand()
		{
			return GetCommand(string.Empty, CommandType.Text, null);
		}

		public IDbCommand GetCommand(string strSql)
		{
			return GetCommand(strSql, CommandType.Text, null);
		}

		public IDbCommand GetCommand(string strSql, CommandType commandType)
		{
			return GetCommand(strSql, commandType, null);
		}

		public IDbDataAdapter GetDataAdapter()
		{
			return GetDataAdapter(string.Empty, CommandType.Text, null);
		}

		public IDbDataAdapter GetDataAdapter(string strSql)
		{
			return GetDataAdapter(strSql, CommandType.Text, null);
		}

		public IDbDataAdapter GetDataAdapter(string strSql, CommandType commandType)
		{
			return GetDataAdapter(strSql, commandType, null);
		}

		#endregion

	}
}

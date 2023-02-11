using System.Data;
using HKH.Data.Configuration;

namespace HKH.Data
{
	public abstract class DataBaseObjectBuilder : BaseDataBaseObjectBuilder, IDataBaseObjectBuilder
	{
		#region Protected Variables

		protected HKHConnectionString configuration = null;

		#endregion

		#region Constructor

		protected DataBaseObjectBuilder(HKHConnectionString configuration)
			: base(configuration.ConnectionString)
		{
			this.configuration = configuration;
		}

		protected DataBaseObjectBuilder(string connectionString)
			: base(connectionString)
		{
			this.configuration = HKHConnectionString.Null;
		}

		#endregion

		#region IDataBaseObjectBuilder Members

		public HKHConnectionString Configuration
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

using System;
using System.Data;

namespace HKH.Data
{
	public abstract class BaseDataBaseObjectBuilder : IBaseDataBaseObjectBuilder
	{
		#region Protected Variables

		protected string connectionString = null;

		#endregion

		#region Constructor

		protected BaseDataBaseObjectBuilder(string connectionString)
		{
			this.connectionString = connectionString;
		}

		#endregion

		#region IBaseDataBaseObjectBuilder Members

		public string ConnectionString { get { return connectionString; } }

		public IDbConnection GetConnection()
		{
			return CreateConnection();
		}

		public IDbCommand GetCommand(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
		{
			IDbCommand command = CreateCommand();
			command.CommandType = commandType;
			command.CommandText = strSql;

			AppendParameters(command, parameters);

			return command;
		}

		public IDbDataAdapter GetDataAdapter(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
		{
			return GetDataAdapter(GetCommand(strSql, commandType, parameters));
		}

		public IDbDataAdapter GetDataAdapter(IDbCommand selectCommand)
		{
			IDbDataAdapter adapter = CreateDataAdapter();
			adapter.SelectCommand = selectCommand;

			return adapter;
		}

		public abstract IDbDataParameter GetParameter(string name);

		public abstract IDbDataParameter GetParameter(string name, string dbTypeName);

		public abstract IDbDataParameter GetParameter(string name, int dbTypeCode);

		public abstract IDbDataParameter[] GetParameters(string strSql, params object[] values);

		public virtual void Dispose()
		{
			this.connectionString = null;
		}

		#endregion

		#region ICommandBuiler Members

		public abstract void BuildCommands(IDbDataAdapter adapter);

		#endregion

		#region Abstract Members

		protected abstract IDbConnection CreateConnection();
		protected abstract IDbCommand CreateCommand();
		protected abstract IDbDataAdapter CreateDataAdapter();
		protected abstract string GetFormattedParameterName(string name);

		#endregion

		#region Helpers

		private void AppendParameters(IDbCommand command, params IDbDataParameter[] parameters)
		{
			if (null == parameters)
				return;

			foreach (IDbDataParameter parameter in parameters)
			{
				parameter.Value = parameter.Value ?? DBNull.Value;
				command.Parameters.Add(parameter);
			}
		}

		#endregion
	}
}

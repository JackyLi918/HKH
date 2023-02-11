using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using HKH.Data.Configuration;

namespace HKH.Data
{
    public class DataProvider : IDataProvider
    {
        #region Multiton

        private static Dictionary<string, IDataProvider> dataProviders = new Dictionary<string, IDataProvider>();

        /// <summary>
        /// Get default Instance of IDataProvider
        /// </summary>
        /// <returns></returns>
        public static IDataProvider GetInstance()
        {
            return GetInstance(DataBaseConfigurationManager.DefaultConfiguration);
        }

        /// <summary>
        /// Get special Instance of IDataProvider
        /// </summary>
        /// <param name="dbConfigurationName"></param>
        /// <returns></returns>
        public static IDataProvider GetInstance(string dbConfigurationName)
        {
            IDataProvider dataProvider = null;
            if (dataProviders.TryGetValue(dbConfigurationName, out dataProvider))
                return dataProvider;

            return GetInstance(DataBaseConfigurationManager.GetConfiguration(dbConfigurationName));
        }

        /// <summary>
        /// Get special Instance of IDataProvider
        /// </summary>
        /// <param name="dbConfiguration"></param>
        /// <returns></returns>
        public static IDataProvider GetInstance(HKHConnectionString dbConfiguration)
        {
            IDataProvider dataProvider = null;
            if (!dataProviders.TryGetValue(dbConfiguration.Name, out dataProvider))
            {
                lock (typeof(DataBaseObjectBuilderFactory))
                {
                    if (!dataProviders.TryGetValue(dbConfiguration.Name, out dataProvider))
                    {
                        dataProvider = new DataProvider(DataBaseObjectBuilderFactory.GetDataBaseObjectBuilder(dbConfiguration));
                        dataProviders.TryAdd(dbConfiguration.Name, dataProvider);
                    }
                }
            }

            return dataProvider;
        }

        /// <summary>
        /// Get special Instance of IDataProvider
        /// </summary>
        /// <param name="builderTypeName">the type of IDataBaseObjectBuilder--(TypeFullName,AssemblyName)</param>
        /// <param name="connectionString"></param>
        /// <param name="inCache">add this DataProvider instance into cache with ConnectionString as key if true</param>
        /// <returns></returns>
        public static IDataProvider GetInstance(string builderTypeName, string connectionString, bool inCache)
        {
            IDataProvider dataProvider = dataProviders.ContainsKey(connectionString) ? dataProviders[connectionString] : new DataProvider(DataBaseObjectBuilderFactory.GetDataBaseObjectBuilder(builderTypeName, connectionString));
            if (inCache)
                dataProviders.TryAdd(connectionString, dataProvider);
            return dataProvider;
        }

        /// <summary>
        /// Get special Instance of IDataProvider
        /// </summary>
        /// <param name="builderType">the type of IDataBaseObjectBuilder</param>
        /// <param name="connectionString"></param>
        /// <param name="inCache">add this DataProvider instance into cache with ConnectionString as key if true</param>
        /// <returns></returns>
        public static IDataProvider GetInstance(Type builderType, string connectionString, bool inCache)
        {
            IDataProvider dataProvider = dataProviders.ContainsKey(connectionString) ? dataProviders[connectionString] : new DataProvider(DataBaseObjectBuilderFactory.GetDataBaseObjectBuilder(builderType, connectionString));
            if (inCache)
                dataProviders.TryAdd(connectionString, dataProvider);
            return dataProvider;
        }

        #endregion

        #region Protected Variables

        protected IDataBaseObjectBuilder builder;

        #endregion

        #region Constructor

        internal DataProvider(IDataBaseObjectBuilder builder)
        {
            this.builder = builder;
        }

        #endregion

        #region IBaseDataProvider Members

        public IBaseDataBaseObjectBuilder Builder
        {
            get { return builder; }
        }

        #region Execute With NonTransaction

        public int ExecuteNonQuery(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(builder.GetCommand(strSql, commandType, parameters), null);
        }

        public object ExecuteScalar(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(builder.GetCommand(strSql, commandType, parameters), null);
        }

        public ISafeDataReader ExecuteReader(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(builder.GetCommand(strSql, commandType, parameters), null);
        }

        public DataTable ExecuteDataTable(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(builder.GetDataAdapter(strSql, commandType, parameters), null);
        }

        public DataSet ExecuteDataSet(string strSql, CommandType commandType, params IDbDataParameter[] parameters)
        {
            return ExecuteDataSet(builder.GetDataAdapter(strSql, commandType, parameters), null);
        }

        public void Fill(string strSql, CommandType commandType, DataTable dataTable, params IDbDataParameter[] parameters)
        {
            Fill(builder.GetDataAdapter(strSql, commandType, parameters), dataTable, null);
        }

        public void Fill(string strSql, CommandType commandType, DataSet dataSet, params IDbDataParameter[] parameters)
        {
            Fill(builder.GetDataAdapter(strSql, commandType, parameters), dataSet, null);
        }

        public int Update(DataTable dataTable)
        {
            return Update(builder.GetDataAdapter(string.Format("SELECT * FROM {0}", dataTable.TableName)), dataTable, null);
        }

        #endregion

        #region Execute With Transaction

        public int ExecuteNonQuery(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(builder.GetCommand(strSql, commandType, parameters), transaction);
        }

        public object ExecuteScalar(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(builder.GetCommand(strSql, commandType, parameters), transaction);
        }

        public ISafeDataReader ExecuteReader(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(builder.GetCommand(strSql, commandType, parameters), transaction);
        }

        public DataTable ExecuteDataTable(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(builder.GetDataAdapter(strSql, commandType, parameters), transaction);
        }

        public DataSet ExecuteDataSet(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteDataSet(builder.GetDataAdapter(strSql, commandType, parameters), transaction);
        }

        public void Fill(string strSql, CommandType commandType, DataTable dataTable, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            Fill(builder.GetDataAdapter(strSql, commandType, parameters), dataTable, transaction);
        }

        public void Fill(string strSql, CommandType commandType, DataSet dataSet, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            Fill(builder.GetDataAdapter(strSql, commandType, parameters), dataSet, transaction);
        }

        public DataTable FillSchema(string strSql, DataTable dataTable, SchemaType schemaType)
        {
            DbDataAdapter adapter = (DbDataAdapter)builder.GetDataAdapter(strSql);
            AssignConnection(adapter.SelectCommand, null);
            return adapter.FillSchema(dataTable, schemaType);
        }

        public DataTable[] FillSchema(string strSql, DataSet dataSet, SchemaType schemaType)
        {
            DbDataAdapter adapter = (DbDataAdapter)builder.GetDataAdapter(strSql);
            AssignConnection(adapter.SelectCommand, null);
            return adapter.FillSchema(dataSet, schemaType);
        }

        public DataTable[] FillSchema(string strSql, DataSet dataSet, SchemaType schemaType, string srcTable)
        {
            DbDataAdapter adapter = (DbDataAdapter)builder.GetDataAdapter(strSql);
            AssignConnection(adapter.SelectCommand, null);
            return adapter.FillSchema(dataSet, schemaType, srcTable);
        }

        public int Update(DataTable dataTable, IDbTransaction transaction)
        {
            return Update(builder.GetDataAdapter(string.Format("SELECT * FROM {0}", dataTable.TableName)), dataTable, transaction);
        }

        #endregion

        #endregion

        #region IDataProvider Members

        #region Execute With NonTransaction

        public int ExecuteNonQuery(string strSql)
        {
            return ExecuteNonQuery(strSql, CommandType.Text, null as IDbTransaction, null);
        }

        public int ExecuteNonQuery(string strSql, CommandType commandType)
        {
            return ExecuteNonQuery(strSql, commandType, null as IDbTransaction, null);
        }

        public int ExecuteNonQuery(string strSql, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public object ExecuteScalar(string strSql)
        {
            return ExecuteScalar(strSql, CommandType.Text, null as IDbTransaction, null);
        }

        public object ExecuteScalar(string strSql, CommandType commandType)
        {
            return ExecuteScalar(strSql, commandType, null as IDbTransaction, null);
        }

        public object ExecuteScalar(string strSql, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(strSql, CommandType.Text, parameters);
        }

        public ISafeDataReader ExecuteReader(string strSql)
        {
            return ExecuteReader(strSql, CommandType.Text, null as IDbTransaction, null);
        }

        public ISafeDataReader ExecuteReader(string strSql, CommandType commandType)
        {
            return ExecuteReader(strSql, commandType, null as IDbTransaction, null);
        }

        public ISafeDataReader ExecuteReader(string strSql, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(strSql, CommandType.Text, parameters);
        }

        public DataTable ExecuteDataTable(string strSql)
        {
            return ExecuteDataTable(strSql, CommandType.Text, null as IDbTransaction, null);
        }

        public DataTable ExecuteDataTable(string strSql, CommandType commandType)
        {
            return ExecuteDataTable(strSql, commandType, null as IDbTransaction, null);
        }

        public DataTable ExecuteDataTable(string strSql, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataSet ExecuteDataSet(string strSql)
        {
            return ExecuteDataSet(strSql, CommandType.Text, null as IDbTransaction, null);
        }

        public DataSet ExecuteDataSet(string strSql, CommandType commandType)
        {
            return ExecuteDataSet(strSql, commandType, null as IDbTransaction, null);
        }

        public DataSet ExecuteDataSet(string strSql, params IDbDataParameter[] parameters)
        {
            return ExecuteDataSet(strSql, CommandType.Text, parameters);
        }

        public void Fill(string strSql, DataTable dataTable)
        {
            Fill(strSql, CommandType.Text, dataTable, null as IDbTransaction);
        }

        public void Fill(string strSql, CommandType commandType, DataTable dataTable)
        {
            Fill(strSql, commandType, dataTable, null as IDbTransaction);
        }

        public void Fill(string strSql, DataTable dataTable, params IDbDataParameter[] parameters)
        {
            Fill(strSql, CommandType.Text, dataTable, parameters);
        }

        public void Fill(string strSql, DataSet dataSet)
        {
            Fill(strSql, CommandType.Text, dataSet, null as IDbTransaction);
        }

        public void Fill(string strSql, CommandType commandType, DataSet dataSet)
        {
            Fill(strSql, commandType, dataSet, null as IDbTransaction);
        }

        public void Fill(string strSql, DataSet dataSet, params IDbDataParameter[] parameters)
        {
            Fill(strSql, CommandType.Text, dataSet, parameters);
        }

        public int Update(DataSet dataSet)
        {
            return Update(dataSet.Tables[0]);
        }

        public int Update(DataSet dataSet, string srcTable)
        {
            return Update(dataSet.Tables[srcTable]);
        }

        #endregion

        #region Execute With Transaction

        public int ExecuteNonQuery(string strSql, IDbTransaction transaction)
        {
            return ExecuteNonQuery(strSql, CommandType.Text, transaction, null);
        }

        public int ExecuteNonQuery(string strSql, CommandType commandType, IDbTransaction transaction)
        {
            return ExecuteNonQuery(strSql, commandType, transaction, null);
        }

        public int ExecuteNonQuery(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(strSql, CommandType.Text, transaction, parameters);
        }

        public object ExecuteScalar(string strSql, IDbTransaction transaction)
        {
            return ExecuteScalar(strSql, CommandType.Text, transaction, null);
        }

        public object ExecuteScalar(string strSql, CommandType commandType, IDbTransaction transaction)
        {
            return ExecuteScalar(strSql, commandType, transaction, null);
        }

        public object ExecuteScalar(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(strSql, CommandType.Text, transaction, parameters);
        }

        public ISafeDataReader ExecuteReader(string strSql, IDbTransaction transaction)
        {
            return ExecuteReader(strSql, CommandType.Text, transaction, null);
        }

        public ISafeDataReader ExecuteReader(string strSql, CommandType commandType, IDbTransaction transaction)
        {
            return ExecuteReader(strSql, commandType, transaction, null);
        }

        public ISafeDataReader ExecuteReader(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(strSql, CommandType.Text, transaction, parameters);
        }

        public DataTable ExecuteDataTable(string strSql, IDbTransaction transaction)
        {
            return ExecuteDataTable(strSql, CommandType.Text, transaction, null);
        }

        public DataTable ExecuteDataTable(string strSql, CommandType commandType, IDbTransaction transaction)
        {
            return ExecuteDataTable(strSql, commandType, transaction, null);
        }

        public DataTable ExecuteDataTable(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(strSql, CommandType.Text, transaction, parameters);
        }

        public DataSet ExecuteDataSet(string strSql, IDbTransaction transaction)
        {
            return ExecuteDataSet(strSql, CommandType.Text, transaction, null);
        }

        public DataSet ExecuteDataSet(string strSql, CommandType commandType, IDbTransaction transaction)
        {
            return ExecuteDataSet(strSql, commandType, transaction, null);
        }

        public DataSet ExecuteDataSet(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            return ExecuteDataSet(strSql, CommandType.Text, transaction, parameters);
        }

        public void Fill(string strSql, DataTable dataTable, IDbTransaction transaction)
        {
            Fill(strSql, CommandType.Text, dataTable, transaction, null);
        }

        public void Fill(string strSql, CommandType commandType, DataTable dataTable, IDbTransaction transaction)
        {
            Fill(strSql, commandType, dataTable, transaction, null);
        }

        public void Fill(string strSql, DataTable dataTable, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            Fill(strSql, CommandType.Text, dataTable, transaction, parameters);
        }

        public void Fill(string strSql, DataSet dataSet, IDbTransaction transaction)
        {
            Fill(strSql, CommandType.Text, dataSet, transaction, null);
        }

        public void Fill(string strSql, CommandType commandType, DataSet dataSet, IDbTransaction transaction)
        {
            Fill(strSql, commandType, dataSet, transaction, null);
        }

        public void Fill(string strSql, DataSet dataSet, IDbTransaction transaction, params IDbDataParameter[] parameters)
        {
            Fill(strSql, CommandType.Text, dataSet, transaction, parameters);
        }

        public int Update(DataSet dataSet, IDbTransaction transaction)
        {
            return Update(dataSet.Tables[0], transaction);
        }

        public int Update(DataSet dataSet, string srcTable, IDbTransaction transaction)
        {
            return Update(dataSet.Tables[srcTable], transaction);
        }

        #endregion

        #endregion

        #region IDataParameterProvider Members

        public IDbDataParameter CreateParameter(string name)
        {
            return builder.GetParameter(name);
        }

        public IDbDataParameter CreateParameter(string name, object value)
        {
            IDbDataParameter parameter = builder.GetParameter(name);
            parameter.Value = value;
            return parameter;
        }

        public IDbDataParameter CreateParameter(string name, DbType type, object value)
        {
            return CreateParameter(name, type, 0, ParameterDirection.Input, 0, 0, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameter(string name, DbType type, int size, object value)
        {
            return CreateParameter(name, type, size, ParameterDirection.Input, 0, 0, string.Empty, DataRowVersion.Current, value);

        }

        public IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, object value)
        {
            return CreateParameter(name, type, size, direction, 0, 0, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value)
        {
            return CreateParameter(name, type, size, ParameterDirection.Input, 0, 0, srcColumn, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
        {
            IDbDataParameter parameter = builder.GetParameter(name);

            parameter.DbType = type;
            parameter.Size = size;
            parameter.Direction = direction;
            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.SourceColumn = srcColumn;
            parameter.SourceVersion = srcVersion;
            parameter.Value = value;

            return parameter;
        }

        #region Handle non-DbType parameter

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName)
        {
            return builder.GetParameter(name, dbTypeName);
        }

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, object value)
        {
            return CreateParameterWithDbTypeName(name, dbTypeName, 0, ParameterDirection.Input, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, object value)
        {
            return CreateParameterWithDbTypeName(name, dbTypeName, size, ParameterDirection.Input, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, object value)
        {
            return CreateParameterWithDbTypeName(name, dbTypeName, 0, direction, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value)
        {
            return CreateParameterWithDbTypeName(name, dbTypeName, 0, direction, srcColumn, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeName(string name, string dbTypeName, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
        {
            IDbDataParameter parameter = builder.GetParameter(name, dbTypeName);

            parameter.Size = size;
            parameter.Direction = direction;
            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.SourceColumn = srcColumn;
            parameter.SourceVersion = srcVersion;
            parameter.Value = value;

            return parameter;
        }


        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode)
        {
            return builder.GetParameter(name, dbTypeCode);
        }

        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, object value)
        {
            return CreateParameterWithDbTypeCode(name, dbTypeCode, 0, ParameterDirection.Input, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, object value)
        {
            return CreateParameterWithDbTypeCode(name, dbTypeCode, size, ParameterDirection.Input, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, object value)
        {
            return CreateParameterWithDbTypeCode(name, dbTypeCode, 0, direction, string.Empty, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, string srcColumn, DataRowVersion srcVersion, object value)
        {
            return CreateParameterWithDbTypeCode(name, dbTypeCode, 0, direction, srcColumn, DataRowVersion.Current, value);
        }

        public IDbDataParameter CreateParameterWithDbTypeCode(string name, int dbTypeCode, int size, ParameterDirection direction, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
        {
            IDbDataParameter parameter = builder.GetParameter(name, dbTypeCode);

            parameter.Size = size;
            parameter.Direction = direction;
            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.SourceColumn = srcColumn;
            parameter.SourceVersion = srcVersion;
            parameter.Value = value;

            return parameter;
        }

        #endregion

        #endregion

        #region Execute Extension

        public int ExecuteNonQueryEx(string strSql, params object[] parameters)
        {
            return ExecuteNonQuery(strSql, Builder.GetParameters(strSql, parameters));
        }

        public object ExecuteScalarEx(string strSql, params object[] parameters)
        {
            return ExecuteScalar(strSql, Builder.GetParameters(strSql, parameters));
        }

        public ISafeDataReader ExecuteReaderEx(string strSql, params object[] parameters)
        {
            return ExecuteReader(strSql, Builder.GetParameters(strSql, parameters));
        }

        public DataTable ExecuteDataTableEx(string strSql, params object[] parameters)
        {
            return ExecuteDataTable(strSql, Builder.GetParameters(strSql, parameters));
        }

        public DataSet ExecuteDataSetEx(string strSql, params object[] parameters)
        {
            return ExecuteDataSet(strSql, Builder.GetParameters(strSql, parameters));
        }

        public void FillEx(string strSql, DataTable dataTable, params object[] parameters)
        {
            Fill(strSql, dataTable, Builder.GetParameters(strSql, parameters));
        }

        public void FillEx(string strSql, DataSet dataSet, params object[] parameters)
        {
            Fill(strSql, dataSet, Builder.GetParameters(strSql, parameters));
        }

        public int ExecuteNonQueryEx(string strSql, IDbTransaction transaction, params object[] parameters)
        {
            return ExecuteNonQuery(strSql, transaction, Builder.GetParameters(strSql, parameters));
        }

        public object ExecuteScalarEx(string strSql, IDbTransaction transaction, params object[] parameters)
        {
            return ExecuteScalar(strSql, transaction, Builder.GetParameters(strSql, parameters));
        }

        public ISafeDataReader ExecuteReaderEx(string strSql, IDbTransaction transaction, params object[] parameters)
        {
            return ExecuteReader(strSql, transaction, Builder.GetParameters(strSql, parameters));
        }

        public DataTable ExecuteDataTableEx(string strSql, IDbTransaction transaction, params object[] parameters)
        {
            return ExecuteDataTable(strSql, transaction, Builder.GetParameters(strSql, parameters));
        }

        public DataSet ExecuteDataSetEx(string strSql, IDbTransaction transaction, params object[] parameters)
        {
            return ExecuteDataSet(strSql, transaction, Builder.GetParameters(strSql, parameters));
        }

        public void FillEx(string strSql, DataTable dataTable, IDbTransaction transaction, params object[] parameters)
        {
            Fill(strSql, dataTable, transaction, Builder.GetParameters(strSql, parameters));
        }

        public void FillEx(string strSql, DataSet dataSet, IDbTransaction transaction, params object[] parameters)
        {
            Fill(strSql, dataSet, transaction, Builder.GetParameters(strSql, parameters));
        }

        #endregion

        #region Batch Execute

        public void ExecuteBatch(SqlInfoList sqlList)
        {
            foreach (SqlInfo sql in sqlList)
            {
                ExecuteNonQuery(sql.Text, sql.Type, sql.Parameters);
            }
        }

        public void ExecuteBatch(SqlInfoList sqlList, IDbTransaction transaction)
        {
            foreach (SqlInfo sql in sqlList)
            {
                ExecuteNonQuery(sql.Text, sql.Type, transaction, sql.Parameters);
            }
        }

        public void ExecuteBatchInTransaction(SqlInfoList sqlList)
        {
            using (TransactionHelper helper = new TransactionHelper(null, this, TransactionOption.Required))
            {
                ExecuteBatch(sqlList, helper.Transaction);
                helper.TryCommitTransaction();
            }
        }

        #endregion

        #region Helpers

        private int ExecuteNonQuery(IDbCommand command, IDbTransaction transaction)
        {
            IDbConnection connection = null;

            try
            {
                connection = AssignConnection(command, transaction);
                return command.ExecuteNonQuery();
            }
            finally
            {
                TryCloseConnection(connection, transaction);
            }
        }

        private object ExecuteScalar(IDbCommand command, IDbTransaction transaction)
        {
            IDbConnection connection = null;

            try
            {
                connection = AssignConnection(command, transaction);
                return command.ExecuteScalar();
            }
            finally
            {
                TryCloseConnection(connection, transaction);
            }
        }

        private ISafeDataReader ExecuteReader(IDbCommand command, IDbTransaction transaction)
        {
            AssignConnection(command, transaction);
            return new SafeDataReader(command.ExecuteReader((transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default)));
        }

        private DataTable ExecuteDataTable(IDbDataAdapter adapter, IDbTransaction transaction)
        {
            DataTable dataTable = new DataTable();
            Fill(adapter, dataTable, transaction);
            return dataTable;
        }

        private DataSet ExecuteDataSet(IDbDataAdapter adapter, IDbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            Fill(adapter, dataSet, transaction);
            return dataSet;
        }

        private void Fill(IDbDataAdapter adapter, DataTable dataTable, IDbTransaction transaction)
        {
            IDbConnection connection = null;

            try
            {
                connection = AssignConnection(adapter.SelectCommand, transaction);
                ((DbDataAdapter)adapter).Fill(dataTable);
            }
            finally
            {
                TryCloseConnection(connection, transaction);
            }
        }

        private void Fill(IDbDataAdapter adapter, DataSet dataSet, IDbTransaction transaction)
        {
            IDbConnection connection = null;

            try
            {
                connection = AssignConnection(adapter.SelectCommand, transaction);
                adapter.Fill(dataSet);
            }
            finally
            {
                TryCloseConnection(connection, transaction);
            }
        }

        private int Update(IDbDataAdapter adapter, DataTable dataTable, IDbTransaction transaction)
        {
            IDbConnection connection = null;

            try
            {
                connection = AssignConnection(adapter.SelectCommand, transaction);
                builder.BuildCommands(adapter);
                return ((DbDataAdapter)adapter).Update(dataTable);
            }
            finally
            {
                TryCloseConnection(connection, transaction);
            }
        }

        private IDbConnection AssignConnection(IDbCommand command, IDbTransaction transaction)
        {
            IDbConnection connection = null;
            if (transaction != null)
            {
                connection = transaction.Connection;
                command.Connection = connection;
                command.Transaction = transaction;
            }
            else
            {
                connection = builder.GetConnection();
                command.Connection = connection;
                command.Connection.Open();
            }

            command.CommandTimeout = command.Connection.ConnectionTimeout;
            return connection;
        }

        private void TryCloseConnection(IDbConnection connection, IDbTransaction transaction)
        {
            if (transaction == null && connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }
        #endregion
    }
}

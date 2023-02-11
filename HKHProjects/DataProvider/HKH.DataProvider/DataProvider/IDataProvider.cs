using System.Data;

namespace HKH.Data
{
    public interface IDataProvider : IBaseDataProvider, IDataParameterProvider
    {
        #region Execute With NonTransaction

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql);

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, CommandType commandType);

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <returns></returns>
        object ExecuteScalar(string strSql);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, CommandType commandType);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, CommandType commandType);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, CommandType commandType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, CommandType commandType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataTable"></param>
        void Fill(string strSql, DataTable dataTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="dataTable"></param>
        void Fill(string strSql, CommandType commandType, DataTable dataTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="dataTable"></param>
        void Fill(string strSql, DataTable dataTable, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataSet"></param>
        void Fill(string strSql, DataSet dataSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="dataSet"></param>
        void Fill(string strSql, CommandType commandType, DataSet dataSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSet"></param>
        void Fill(string strSql, DataSet dataSet, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        int Update(DataSet dataSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        int Update(DataSet dataSet, string srcTable);

        #endregion

        #region Execute With Transaction

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, IDbTransaction transaction);

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, CommandType commandType, IDbTransaction transaction);

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, IDbTransaction transaction);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, CommandType commandType, IDbTransaction transaction);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, IDbTransaction transaction);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, CommandType commandType, IDbTransaction transaction);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, CommandType commandType, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, CommandType commandType, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataTable"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, DataTable dataTable, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="dataTable"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, CommandType commandType, DataTable dataTable, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="dataTable"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, DataTable dataTable, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataSet"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, DataSet dataSet, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="dataSet"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, CommandType commandType, DataSet dataSet, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSet"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, DataSet dataSet, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int Update(DataSet dataSet, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="srcTable"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int Update(DataSet dataSet, string srcTable, IDbTransaction transaction);

        #endregion

		#region Execute Extension

		/// <summary>
		/// Execute NonQuery command(Insert ,Delete ,Update) for sqltext only
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		int ExecuteNonQueryEx(string strSql, params object[] parameters);

		/// <summary>
		/// Execute Scalar for sqltext only
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		object ExecuteScalarEx(string strSql, params object[] parameters);

		/// <summary>
		/// Execute DataReader for sqltext only
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		ISafeDataReader ExecuteReaderEx(string strSql, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		DataTable ExecuteDataTableEx(string strSql, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		DataSet ExecuteDataSetEx(string strSql, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="dataTable"></param>
		void FillEx(string strSql, DataTable dataTable, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="dataSet"></param>
		void FillEx(string strSql, DataSet dataSet, params object[] parameters);

		/// <summary>
		/// Execute NonQuery command(Insert ,Delete ,Update)
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		int ExecuteNonQueryEx(string strSql, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// Execute Scalar
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		object ExecuteScalarEx(string strSql, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// Execute DataReader
		/// </summary>
		/// <param name="strSql">sqlStatement or sp name</param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		ISafeDataReader ExecuteReaderEx(string strSql, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		DataTable ExecuteDataTableEx(string strSql, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		DataSet ExecuteDataSetEx(string strSql, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="dataTable"></param>
		/// <param name="transaction"></param>
		void FillEx(string strSql, DataTable dataTable, IDbTransaction transaction, params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="parameters"></param>
		/// <param name="dataSet"></param>
		/// <param name="transaction"></param>
		void FillEx(string strSql, DataSet dataSet, IDbTransaction transaction, params object[] parameters);

		#endregion

		#region Batch Execute

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlList"></param>
		void ExecuteBatch(SqlInfoList sqlList);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlList"></param>
		/// <param name="transaction"></param>
		void ExecuteBatch(SqlInfoList sqlList, IDbTransaction transaction);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlList"></param>
		void ExecuteBatchInTransaction(SqlInfoList sqlList);

		#endregion
	}	
}

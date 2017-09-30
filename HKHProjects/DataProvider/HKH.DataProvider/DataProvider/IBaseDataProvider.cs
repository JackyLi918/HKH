using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HKH.Common;

namespace HKH.Data
{
    public interface IBaseDataProvider
    {
        IBaseDataBaseObjectBuilder Builder { get; }

        #region Execute With NonTransaction

        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, CommandType commandType, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="dataTable"></param>
        void Fill(string strSql, CommandType commandType, DataTable dataTable, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSet"></param>
        void Fill(string strSql, CommandType commandType, DataSet dataSet, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataTable"></param>
        /// <param name="schemaType"></param>
        /// <returns></returns>
        DataTable FillSchema(string strSql, DataTable dataTable, SchemaType schemaType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataSet"></param>
        /// <param name="schemaType"></param>
        /// <returns></returns>
        DataTable[] FillSchema(string strSql, DataSet dataSet, SchemaType schemaType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dataSet"></param>
        /// <param name="schemaType"></param>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        DataTable[] FillSchema(string strSql, DataSet dataSet, SchemaType schemaType, string srcTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        int Update(DataTable dataTable);

        #endregion

        #region Execute With Transaction
        /// <summary>
        /// Execute NonQuery command(Insert ,Delete ,Update)
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        object ExecuteScalar(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// Execute DataReader
        /// </summary>
        /// <param name="strSql">sqlStatement or sp name</param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        ISafeDataReader ExecuteReader(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string strSql, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="dataTable"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, CommandType commandType, DataTable dataTable, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSet"></param>
        /// <param name="transaction"></param>
        void Fill(string strSql, CommandType commandType, DataSet dataSet, IDbTransaction transaction, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        int Update(DataTable dataTable, IDbTransaction transaction);

        #endregion
    }
}

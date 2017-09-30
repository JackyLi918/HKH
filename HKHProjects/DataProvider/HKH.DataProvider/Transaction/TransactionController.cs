using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HKH.Data.Configuration;

namespace HKH.Data
{
    internal class TransactionController
    {
        #region Private Variables

        private IBaseDataBaseObjectBuilder objBuilder;
        private IDbTransaction transaction = null;

        #endregion

        #region Constructor

        public TransactionController()
            : this(DataBaseConfigurationManager.DefaultConfiguration)
        {
        }

        public TransactionController(HKHConnectionStringElement dbConfiguration)
            : this(DataBaseObjectBuilderFactory.GetDataBaseObjectBuilder(dbConfiguration))
        {
        }

        public TransactionController(IBaseDataBaseObjectBuilder objBuilder)
        {
            this.objBuilder = objBuilder;
        }

        #endregion

        #region Properties

        public IBaseDataBaseObjectBuilder Builder
        {
            get { return objBuilder; }
        }

        public bool HasTransaction
        {
            get { return transaction == null; }
        }

        public IDbTransaction Transaction
        {
            get { return transaction; }
        }

        #endregion

        #region Methods

        public IDbTransaction BeginTransaction()
        {
            IDbConnection connection = objBuilder.GetConnection();
            connection.Open();

            transaction = connection.BeginTransaction();

            return transaction;
        }

        public void CommitTransaction()
        {
            IDbConnection connection = transaction.Connection;
            transaction.Commit();
            connection.Close();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            IDbConnection connection = transaction.Connection;
            transaction.Rollback();
            connection.Close();
            transaction = null;
        }

        #endregion
    }
}

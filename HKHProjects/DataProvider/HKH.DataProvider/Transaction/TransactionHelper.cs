using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HKH.Data.Configuration;

namespace HKH.Data
{
    public class TransactionHelper : IDisposable
    {
        public static TransactionOption Option { get; set; } = TransactionOption.Required;

        private TransactionController controller = null;
        private IDbTransaction currentTransaction = null;
        private bool hasCommitted = false;

        #region Constructor

        public TransactionHelper(IDbTransaction transaction, IDataProvider dataProvider = null, TransactionOption options = TransactionOption.Unspecified)
        {
            if (transaction != null)
                currentTransaction = transaction;
            else
            {
                TransactionOption opt = (options == TransactionOption.Unspecified ? TransactionHelper.Option : options);
                opt = (opt == TransactionOption.Unspecified ? TransactionOption.Required : opt);

                if (opt == TransactionOption.Required)
                {
                    controller = dataProvider == null ? new TransactionController() : new TransactionController(dataProvider.Builder);
                    currentTransaction = controller.BeginTransaction();
                }
            }
        }

        #endregion

        public IDbTransaction Transaction
        {
            get { return currentTransaction; }
        }

        public void TryCommitTransaction()
        {
            if (currentTransaction != null)
            {
                if (controller != null)
                    controller.CommitTransaction();

                hasCommitted = true;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!hasCommitted && currentTransaction != null && controller != null)
                controller.RollbackTransaction();
        }

        #endregion
    }

    public enum TransactionOption
    {
        Unspecified,
        Opitional,
        Required
    }
}

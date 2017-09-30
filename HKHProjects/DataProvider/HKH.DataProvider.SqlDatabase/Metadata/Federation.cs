using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HKH.Data.SqlDatabase.Metadata
{
    /// <summary>
    /// Represents a federation in a database
    /// </summary>
    public class Federation
    {
        const string OPERATION_STATUS_QUERY = "select * from sys.dm_federation_operations";

        public Federation()
        {
            Members = new Dictionary<int, Member>();
        }

        /// <summary>
        /// All members in this federation
        /// </summary>
        public Dictionary<int,Member> Members { get; set; }

        /// <summary>
        /// Information on the database this Federation is contained in
        /// </summary>
        public FederatedDatabase Parent { get; set; }

        /// <summary>
        /// System issued ID for this Federation
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of the federation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the name of the Federation
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return Name;
        }

        public OperationStatus GetOperationStatus()
        {
            using (var cnn = new ReliableSqlConnection(this.Parent.ConnectionString))
            {
                cnn.Open();
                cnn.UseFederationRoot();

                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = OPERATION_STATUS_QUERY;
                    using (var rdr = cmd.ExecuteReaderWithRetry())
                    {
                        if (rdr.Read())
                        {
                            return new OperationStatus()
                            {
                                 OperationId = rdr.GetGuid(0),
                                 OperationType = rdr.GetString(1),
                                 FederationId = rdr.GetInt32(2),
                                 FederationName = rdr.GetString(3),
                                 PercentComplete = rdr.GetFloat(6)
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}

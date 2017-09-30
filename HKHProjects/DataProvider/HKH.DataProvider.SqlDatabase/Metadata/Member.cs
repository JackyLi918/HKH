using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HKH.Data.SqlDatabase.Metadata
{
    /// <summary>
    /// Represents a federation member (shard)
    /// </summary>
    public class Member : IComparable
    {
        const string MEDIAN_QUERY = "WITH CTE_Median(RID, {0}) AS " +
                                    "(SELECT top 50 percent ROW_NUMBER() OVER(ORDER BY {0} DESC) [RID], {0} FROM {1} ORDER BY {0} DESC) " +
                                    "SELECT top 1 {0} FROM CTE_Median ORDER BY {0} ASC ";

        public Member()
        {
            Tables = new List<FederatedTable>();
        }

        /// <summary>
        /// The federation this member belongs to
        /// </summary>
        public Federation Parent { get; set; }

        /// <summary>
        /// System issued ID for this member
        /// </summary>
        public int Id { get; set; }

        public string DistributionName { get; set; }

        /// <summary>
        /// Federation key range of values contained in this member
        /// </summary>
        public IFedRange Range { get; set; }

        public string RangeDescription { get { return Range.ToString(); } }

        public FedKeyType FedKeyType { get { return Range.Low.Type; } }

        /// <summary>
        /// List of the federated tables in this member
        /// </summary>
        public IEnumerable<FederatedTable> Tables { get; set; }

        /// <summary>
        /// Will split the federation at the given point into two members
        /// </summary>
        /// <param name="splitPoint"></param>
        /// <returns>The new Member that resulted from the split</returns>
        public void Split(FedKey splitPoint)
        {
            SplitMember(Parent.Name, DistributionName, splitPoint);
        }

        private void SplitMember(string federationName, string distributionName, FedKey splitPoint)
        {
            string command = String.Format("ALTER FEDERATION {0} SPLIT AT ({1}={2})", federationName, distributionName, splitPoint.ToFormattedString());

            using (var cnn = new ReliableSqlConnection(Parent.Parent.ConnectionString))
            {
                cnn.Open();
                cnn.UseFederationRoot();

                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get the Middle FedKey of this member's range, or of a tables values
        /// </summary>
        /// <param name="bGetMiddleOfRange"></param>
        /// <returns>The new FedKey in the middle</returns>
        public FedKey GetMiddleFedKey(bool bGetMiddleOfRange = true)
        {
            FedKey fedkey = null;

            // Query the table to get the median value, if no rows return middle of the pack (yeah, right!)
            if (!bGetMiddleOfRange)
            {
                string command = "";
                foreach (FederatedTable fedtable in Tables)
                {
                    command = String.Format(MEDIAN_QUERY, fedtable.ColumnList, fedtable.Name);
                    break;      // Get First Table only
                }

                using (var cnn = new ReliableSqlConnection(Parent.Parent.ConnectionString))
                {
                    cnn.Open();
                    cnn.UseFederationMember(this);

                    using (var cmd = cnn.CreateCommand())
                    {
                        cmd.CommandText = command;
                        using (var rdr = cmd.ExecuteReaderWithRetry())
                        {
                            while (rdr.Read())
                            {
                                fedkey = FedKey.CreateFedKey(this.FedKeyType, (object)rdr.GetValue(0));
                            }
                        }
                    }
                }
            }

            if (null == fedkey)
            {
                fedkey = this.Range.GetMidRange();
            }

            return fedkey;
        }

        public int CompareTo(object obj)
        {
            if (obj is Member)
            {
                var compare = obj as Member;
                return this.Range.Low.CompareTo(compare.Range.Low);
            }
            else
            {
                throw new ArgumentException("Object is not a Member");
            }
        }
    }
}

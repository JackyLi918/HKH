using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace HKH.Data.SqlDatabase.Metadata
{
    /// <summary>
    /// Object which represents a federated database, it contains a listing of all federations in the database
    /// </summary>
    public class FederatedDatabase
    {
        const string METADATA_QUERY = "select fed.federation_id, fed.name, mc.member_id, mc.distribution_name, dist.system_type_id, mc.range_low, mc.range_high from sys.federations fed inner join sys.federation_member_distributions MC on fed.federation_id = MC.federation_id inner join sys.federation_distributions dist on MC.federation_id = dist.federation_id and MC.distribution_name = dist.distribution_name order by federation_id";
        const string QUERY_COLUMNS = "select sys.all_objects.name TableName, sys.columns.name ColumnName from sys.federated_table_columns inner join sys.all_objects on sys.all_objects.object_id = sys.federated_table_columns.object_id inner join sys.columns on sys.columns.object_id = sys.federated_table_columns.object_id and sys.columns.column_id = sys.federated_table_columns.column_id";

            
        public FederatedDatabase()
        {
        }

        public FederatedDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            BuildMetaData();
        }

        /// <summary>
        /// The connection string used to access the database which contains federations
        /// </summary>
        internal string ConnectionString { get; set; }

        /// <summary>
        /// All federations which exist in this database
        /// </summary>
        public Dictionary<string, Federation> Federations { get; set; }

        private void BuildMetaData()
        {
            using (var cnn = new ReliableSqlConnection(ConnectionString))
            {
                cnn.Open();
                cnn.UseFederationRoot();

                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = FederatedDatabase.METADATA_QUERY;
                    using (var rdr = cmd.ExecuteReaderWithRetry())
                    {
                        Federations = new Dictionary<string, Federation>();

                        while (rdr.Read())
                        {
                            Federation currentFederation;

                            if (Federations.ContainsKey(rdr.GetString(1)))
                            { 
                                currentFederation = Federations[rdr.GetString(1)];
                            }
                            else
                            {
                                currentFederation = new Federation();
                                currentFederation.ID = rdr.GetInt32(0);
                                currentFederation.Name = rdr.GetString(1);
                                currentFederation.Parent = this;
                                Federations.Add(currentFederation.Name, currentFederation);
                            }

                            var m = new Member();
                            m.Parent = currentFederation;
                            m.Id = rdr.GetInt32(2);
                            m.DistributionName = rdr.GetString(3);

                            switch (rdr.GetByte(4))
                            {
                                case 56:
                                    System.Int32 lowInt = rdr.GetInt32(5);
                                    System.Int32 highInt = int.MaxValue;;
                                    if (!rdr.IsDBNull(6))
                                        highInt = rdr.GetInt32(6);

                                    m.Range = new FedRange(lowInt, highInt);
                                    break;
                                case 127:
                                    System.Int64 lowLong = rdr.GetInt64(5);
                                    System.Int64 highLong = long.MaxValue;;
                                    if (!rdr.IsDBNull(6))
                                        highLong = rdr.GetInt64(6);

                                    m.Range = new FedRange(lowLong, highLong);
                                    break;
                                case 36:
                                    System.Guid lowGuid = rdr.GetGuid(5);
                                    System.Guid highGuid = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
                                    if (!rdr.IsDBNull(6))
                                        highGuid = rdr.GetGuid(6);

                                    m.Range = new FedRange(lowGuid, highGuid);
                                    break;
                                case 165:
                                    System.Byte[] lowVarBinary = rdr.GetValue(5) as byte[];;
                                    System.Byte[] highVarBinary = null;
                                    if (!rdr.IsDBNull(6))
                                        highVarBinary = rdr.GetValue(6) as byte[];

                                    m.Range = new FedRange(lowVarBinary, highVarBinary);
                                    break;
                                default:
                                    break;
                            }

                            currentFederation.Members.Add(m.Id, m);
                        }
                    }
                }

                LoadMemberTablesAndColumns();
            }
        }

        private void LoadMemberTablesAndColumns(bool forceLoadOfEachMember = false)
        {
            foreach (var fed in this.Federations.Values)
            {
                if (forceLoadOfEachMember)
                {
                    foreach (var member in fed.Members.Values)
                    {
                        member.Tables = GetTablesAndColumns(fed.Members.Values.First());
                    }
                }
                else
                {
                    var tblcolList = GetTablesAndColumns(fed.Members.Values.First());

                    foreach (var member in fed.Members.Values)
                    {
                        member.Tables = tblcolList;
                    }
                }
            }
        }

        private IEnumerable<FederatedTable> GetTablesAndColumns(Member member)
        {
            List<FederatedTable> tables = new List<FederatedTable>();

            using (var cnn = new ReliableSqlConnection(ConnectionString))
            {
                cnn.Open();
                cnn.UseFederationMember(member);

                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = QUERY_COLUMNS;
                    using (var rdr = cmd.ExecuteReaderWithRetry())
                    {
                        while (rdr.Read())
                        {
                            FederatedTable currentTable = tables.SingleOrDefault(x => x.Name == rdr.GetString(0)); ;

                            if (currentTable == null)
                            {
                                currentTable = new FederatedTable();
                                currentTable.Name = rdr.GetString(0);
                                tables.Add(currentTable);
                            }

                            //currentTable.Columns.Add(new FederatedColumn() { Name = rdr.GetString(1) });
                            currentTable.Add(new FederatedColumn() { Name = rdr.GetString(1) });
                        }
                    }
                }
            }

            return tables;
        }

        /// <summary>
        /// Will return a string representation of this object, we use the Ddatasource element of the connection string
        /// </summary>
        /// <returns>String with name of the data source</returns>
        public override string ToString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder cnstr = new SqlConnectionStringBuilder(ConnectionString);
            return cnstr.DataSource;
        }

    }
}

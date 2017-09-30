using System.Collections.Generic;
using System.Linq;

namespace HKH.Data.SqlDatabase.Metadata
{
    /// <summary>
    /// Represents a table, in a federation member, which is a 'federated' table
    /// </summary>
    public class FederatedTable
    {
        public FederatedTable()
        {
            Columns = new List<FederatedColumn>();
        }

        /// <summary>
        /// Name of the table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Comma delimited list of the columns that are part of the federation key for this table
        /// </summary>
        public string ColumnList
        {
            get
            {
                string columns = string.Concat(Columns.Select(x => x.Name + ","));

                if (!string.IsNullOrEmpty(columns))
                    return columns.Substring(0, columns.Length - 1);
                else
                    return "";
            }
        }

        /// <summary>
        /// List of the columns that are part of the federation key for this table
        /// </summary>
        public IEnumerable<FederatedColumn> Columns { get; set; }

        /// <summary>
        /// Add a Federated Column to this list
        /// </summary>
        public void Add(FederatedColumn fc)
        {
            ((List<FederatedColumn>)Columns).Add(fc);
        }
    }
}

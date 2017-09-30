using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Data.SqlDatabase.Metadata
{
    public class OperationStatus
    {
        public Guid OperationId { get; set; }
        public string OperationType { get; set; }
        public int FederationId { get; set; }
        public string FederationName { get; set; }
        public float PercentComplete { get; set; }
    }
}

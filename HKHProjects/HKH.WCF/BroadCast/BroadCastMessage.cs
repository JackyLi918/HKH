using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HKH.WCF
{
	[DataContract]
	public class BroadCastMessage
	{
		[DataMember]
		public string Action { get; set; }

		[DataMember]
		public string Body { get; set; }
	}
}

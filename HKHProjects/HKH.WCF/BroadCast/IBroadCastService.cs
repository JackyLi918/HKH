using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HKH.WCF
{
	[ServiceContract(CallbackContract = typeof(IBroadCastReceiver))]
	[ServiceKnownType(typeof(BroadCastMessage))]
	public interface IBroadCastService
	{
		[OperationContract]
		void RegisterClient();

        [OperationContract]
        void BroadCast(BroadCastMessage msg);
	}

	[ServiceKnownType(typeof(BroadCastMessage))]
	public interface IBroadCastReceiver
	{
		[OperationContract(IsOneWay = true)]
		void Receive(BroadCastMessage message);
	}
}

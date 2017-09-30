using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HKH.WCF
{
	[ServiceContract(CallbackContract = typeof(IBroadCastReceiverCallback))]
	public interface IBroadCastServiceProxy
	{
		[OperationContract(Action = "http://tempuri.org/IBroadCastService/RegisterClient", ReplyAction = "http://tempuri.org/IBroadCastService/RegisterClientResponse")]
		void RegisterClient();

        [OperationContract(Action = "http://tempuri.org/IBroadCastService/BroadCast", ReplyAction = "http://tempuri.org/IBroadCastService/BroadCastResponse")]
        void BroadCast(BroadCastMessage msg);
	}

	public interface IBroadCastReceiverCallback
	{
		[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IBroadCastService/Receive")]
		void Receive(BroadCastMessage message);
	}

	public interface IBroadCastServiceChannel : IBroadCastServiceProxy, IClientChannel
	{
	}
}

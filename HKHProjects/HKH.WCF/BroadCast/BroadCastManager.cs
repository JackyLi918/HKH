using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using HKH.Tasks;

namespace HKH.WCF
{
    public class BroadCastManager
    {
        public static BroadCastServiceProxy ClientProxy { get; set; }

        public static void BroadCastServer(BroadCastMessage msg)
        {
            ParallelTaskPool.Enqueue(new ParallelTask<BroadCastMessage>(BroadCastManager.BroadCastCore, msg));
        }

        internal static void BroadCastCore(BroadCastMessage msg)
        {
            if (BroadCastService.ReceiverList == null || BroadCastService.ReceiverList.Count == 0)
                return;

            lock (BroadCastService.ReceiverList)
            {
                foreach (IBroadCastReceiver receiver in BroadCastService.ReceiverList)
                    receiver.Receive(msg);
            }
        }

        /// <summary>
        /// used by Server side
        /// </summary>
        /// <param name="msg"></param>
        public static void BroadCastClient(BroadCastMessage msg)
        {
            if (ClientProxy != null)
                ClientProxy.BroadCast(msg);
        }

        /// <summary>
        /// used by Client side
        /// </summary>
        /// <param name="client"></param>
        public static void RegisterClient(IBroadCastReceiverCallback client)
        {
            InstanceContext ctx = new InstanceContext(client);
            ClientProxy = new BroadCastServiceProxy(ctx);
            ClientProxy.RegisterClient();
        }
    }
}

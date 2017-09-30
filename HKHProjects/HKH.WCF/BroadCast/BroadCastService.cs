using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using HKH.Tasks;

namespace HKH.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BroadCastService : IBroadCastService, IDisposable
    {
        private static List<IBroadCastReceiver> _receiverList = new List<IBroadCastReceiver>();
        public static IList<IBroadCastReceiver> ReceiverList { get { return _receiverList; } }

        public void RegisterClient()
        {
            var receiver = OperationContext.Current.GetCallbackChannel<IBroadCastReceiver>();
            OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);
            ReceiverList.Add(receiver);
        }

        public void BroadCast(BroadCastMessage msg)
        {
            BroadCastManager.BroadCastServer(msg);
        }

        void Channel_Closing(object sender, EventArgs e)
        {
            lock (ReceiverList)
            {
                ReceiverList.Remove((IBroadCastReceiver)sender);
            }
        }

        public void Dispose()
        {
            ReceiverList.Clear();
        }
    }
}

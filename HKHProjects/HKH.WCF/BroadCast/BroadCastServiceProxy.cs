using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace HKH.WCF
{
    public partial class BroadCastServiceProxy : DuplexClientBase<IBroadCastServiceProxy>, IBroadCastServiceProxy
    {
        public BroadCastServiceProxy()
            : base(null)
        {
        }

        public BroadCastServiceProxy(InstanceContext callbackInstance) :
            base(callbackInstance)
        {
        }

        public BroadCastServiceProxy(InstanceContext callbackInstance, string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
        }

        public BroadCastServiceProxy(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public BroadCastServiceProxy(InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public BroadCastServiceProxy(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress) :
            base(callbackInstance, binding, remoteAddress)
        {
        }

        public void RegisterClient()
        {
            base.Channel.RegisterClient();
        }

        public void BroadCast(BroadCastMessage msg)
        {
            base.Channel.BroadCast(msg);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using HKH.Tasks;

namespace HKH.WCF
{
    public class BroadCastServiceHostFactory : ServiceHostFactory
    {
        public BroadCastServiceHostFactory()
        {
            if (ParallelTaskPool.State == TaskPoolState.Unstarted)
            {
                lock ("ParallelTaskPool")
                {
                    if (ParallelTaskPool.State == TaskPoolState.Unstarted)
                        ParallelTaskPool.Start();
                }
            }
        }
    }
}
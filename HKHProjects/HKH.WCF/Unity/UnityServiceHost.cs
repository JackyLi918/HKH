﻿using System;
using System.ServiceModel;
using Unity;

namespace HKH.WCF.Unity
{
    public class UnityServiceHost : ServiceHost
	{
		public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			foreach (var cd in this.ImplementedContracts.Values)
			{
				cd.Behaviors.Add(new UnityInstanceProvider(container));
			}
		}
	}
}

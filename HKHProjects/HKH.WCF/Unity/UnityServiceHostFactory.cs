using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace HKH.WCF.Unity
{
	public class UnityServiceHostFactory : ServiceHostFactory
	{
		protected static IUnityContainer container = new UnityContainer();
		protected static bool isInitialized = false;

		public UnityServiceHostFactory()
		{
			if (!isInitialized)
			{
				isInitialized = true;
				RegisterTypes();
			}
		}

		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			return new UnityServiceHost(container, serviceType, baseAddresses);
		}

		protected virtual void RegisterTypes()
		{
			Type baseType = typeof(IInjectable);
			container.RegisterTypes(
				AllClasses.FromLoadedAssemblies().Where(t => t.IsClass && t.GetInterfaces().Any(i => i == baseType)),
				WithMappings.FromMatchingInterface,
				WithName.Default,
				WithLifetime.ContainerControlled
				);
		}
	}
}

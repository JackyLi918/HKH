/*******************************************************
 * Filename: UnityWebServiceHostFactory.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/3/2013 11:24:21 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Unity;
using Unity.RegistrationByConvention;

namespace HKH.WCF.Unity
{
    /// <summary>
    /// UnityWebServiceHostFactory
    /// </summary>
    public class UnityWebServiceHostFactory : WebServiceHostFactory
	{
		protected static IUnityContainer container = new UnityContainer();
		protected static bool isInitialized = false;

		public UnityWebServiceHostFactory()
		{
			if (!isInitialized)
			{
				isInitialized = true;
				RegisterTypes();
			}
		}

		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			return new UnityWebServiceHost(container, serviceType, baseAddresses);
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
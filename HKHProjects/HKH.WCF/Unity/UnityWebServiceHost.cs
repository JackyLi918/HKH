/*******************************************************
 * Filename: UnityWebServiceHost.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/3/2013 11:20:08 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace HKH.WCF.Unity
{
	/// <summary>
	/// UnityWebServiceHost
	/// </summary>
	public class UnityWebServiceHost : WebServiceHost
	{
		public UnityWebServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
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
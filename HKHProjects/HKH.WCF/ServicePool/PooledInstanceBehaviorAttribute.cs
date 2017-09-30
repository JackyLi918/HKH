/*******************************************************
 * Filename: PooledInstanceBehaviorAttribute.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/29/2013 4:05:54 PM
 * Author:	JackyLi
 * 
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace HKH.WCF
{
	/// <summary>
	/// PooledInstanceBehaviorAttribute
	/// </summary>
	public class PooledInstanceBehaviorAttribute : Attribute, IContractBehavior, IContractBehaviorAttribute
	{
		#region Variables

		#endregion

		#region Properties

		#endregion

		#region Methods

		public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}

		public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
		{
			dispatchRuntime.InstanceProvider = new PooledInstanceProvider();
		}

		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}

		public Type TargetContract
		{
			get { return null; }
		}

		#endregion

		#region Helper

		#endregion
	}
}

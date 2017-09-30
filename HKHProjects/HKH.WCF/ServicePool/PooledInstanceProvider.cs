/*******************************************************
 * Filename: PooledInstanceProvider.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/29/2013 4:06:13 PM
 * Author:	JackyLi
 * 
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using HKH.ObjectPool;

namespace HKH.WCF
{
	/// <summary>
	/// PooledInstanceProvider
	/// </summary>
	public class PooledInstanceProvider : IInstanceProvider
	{
		#region Variables

		#endregion

		#region Properties

		#endregion

		#region Methods

		public object GetInstance(InstanceContext instanceContext, Message message)
		{
			return PooledObjectLocator.GetInstance(instanceContext.Host.Description.ServiceType);
		}

		public object GetInstance(InstanceContext instanceContext)
		{
			return GetInstance(instanceContext, null);
		}

		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			PooledObjectLocator.ReleaseInstance(instance as IPooledObject);
		}

		#endregion

		#region Helper

		#endregion
	}
}

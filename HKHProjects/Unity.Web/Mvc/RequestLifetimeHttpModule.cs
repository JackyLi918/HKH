/*******************************************************
 * Filename: RequestLifetimeHttpModule.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/27/2013 5:44:23 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Unity.Mvc
{
	/// <summary>
	/// RequestLifetimeHttpModule
	/// </summary>
	public class RequestLifetimeHttpModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.EndRequest += (sender, e) => UnityDependencyResolver.DisposeOfChildContainer();
		}

		public void Dispose()
		{
			// nothing to do here
		}
	}
}
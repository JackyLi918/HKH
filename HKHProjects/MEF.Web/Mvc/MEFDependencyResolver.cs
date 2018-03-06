/*******************************************************
 * Filename: MEFDependencyResolver.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	07/01/2015 3:29:37 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Web;
using System.Web.Mvc;

namespace MEF.Mvc
{
    /// <summary>
    /// MEFDependencyResolver
    /// </summary>
    public class MEFDependencyResolver : IDependencyResolver
	{
		private const string HttpContextKey = "perRequestContainer";
		private ComposablePartCatalog _catalog;

		public MEFDependencyResolver(ComposablePartCatalog catalog)
		{
			_catalog = catalog;
		}

		public CompositionContainer ChildContainer
		{
			get
			{
				var childContainer = HttpContext.Current.Items[HttpContextKey] as CompositionContainer;

				if (childContainer == null)
				{
					childContainer = new CompositionContainer(_catalog);
					HttpContext.Current.Items[HttpContextKey] = childContainer;
				}

				return childContainer;
			}
		}

		public object GetService(Type serviceType)
		{
			string contractName = AttributedModelServices.GetContractName(serviceType);
			return ChildContainer.GetExportedValueOrDefault<object>(contractName);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			string contractName = AttributedModelServices.GetContractName(serviceType);
			return ChildContainer.GetExportedValues<object>(contractName);
		}

		public static void DisposeOfChildContainer()
		{
			var childContainer = HttpContext.Current.Items[HttpContextKey] as CompositionContainer;

			if (childContainer != null)
			{
				childContainer.Dispose();
			}
		}
	}
}
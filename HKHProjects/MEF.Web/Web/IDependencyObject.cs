using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEF.Mvc;

namespace MEF.Web
{
	public interface IDependencyObject
	{
	}

	public static class WebMEF
	{
		internal static MEFDependencyResolver Resolver { get; set; }

		public static CompositionContainer Container
		{
			get
			{
				if (Resolver != null)
					return Resolver.ChildContainer;
				else
					return null;
			}
		}
	}
}

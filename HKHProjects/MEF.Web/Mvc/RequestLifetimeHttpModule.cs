using System.Web;

namespace MEF.Mvc
{
    /// <summary>
    /// RequestLifetimeHttpModule
    /// </summary>
    public class RequestLifetimeHttpModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.EndRequest += (sender, e) => MEFDependencyResolver.DisposeOfChildContainer();
		}

		public void Dispose()
		{
			// nothing to do here
		}
	}
}

/*******************************************************
 * Filename: IDependencyObject.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/27/2013 5:44:02 PM
 * Author:	JackyLi
 * 
*****************************************************/

using Unity.Mvc;

namespace Unity.Web
{
    public interface IDependencyObject
    {
    }

    public static class WebUnity
    {
        internal static UnityDependencyResolver Resolver { get; set; }

        public static IUnityContainer Container
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

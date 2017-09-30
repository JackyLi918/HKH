/*******************************************************
 * Filename: IDependencyObject.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/27/2013 5:44:02 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
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

/*******************************************************
 * Filename: PreApplicationStart.cs
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
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Unity.Mvc
{
	/// <summary>
	/// PreApplicationStart
	/// </summary>
	public class PreApplicationStart
	{
		private static bool _isStarting;
		public static void PreStart()
		{
			if (!_isStarting)
			{
				_isStarting = true;

				DynamicModuleUtility.RegisterModule(typeof(RequestLifetimeHttpModule));
			}
		}
	}
}
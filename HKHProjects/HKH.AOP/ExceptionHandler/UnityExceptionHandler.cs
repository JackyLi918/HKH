/*******************************************************
 * Filename: UnityExceptionHandler.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/2/2013 4:49:16 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace HKH.AOP
{
	/// <summary>
	/// UnityExceptionHandler
	/// </summary>
	public class UnityExceptionHandler : ICallHandler
	{
		public int Order { get; set; }

		public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
		{
			IMethodReturn retvalue = getNext()(input, getNext);
			if (retvalue.Exception != null)
			{
				if (HandleException(retvalue.Exception))
				{
					retvalue.Exception = null;
					Type returnType = ((MethodInfo)(input.MethodBase)).ReturnType;
					retvalue.ReturnValue = (returnType.IsValueType ? Activator.CreateInstance(returnType) : null);
				}
			}
			return retvalue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		/// <returns>whether the exception has been handled.</returns>
		protected virtual bool HandleException(Exception ex)
		{
			return true;
		}
	}
}
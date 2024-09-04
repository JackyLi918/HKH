using System;
using System.Runtime.Serialization;

namespace HKH.Common
{
	[Serializable]
	public class UnLogException : ApplicationException
	{
		/// <summary>
		/// Constructor takes problem message to be thrown
		/// invokes constructor on ApplicationException
		/// </summary>
		public UnLogException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor takes problem message and caught exception
		/// invokes constructor on ApplicationException
		/// </summary>
		public UnLogException(string message, System.Exception ex)
			:base(message, ex)
		{
		}
	}
}

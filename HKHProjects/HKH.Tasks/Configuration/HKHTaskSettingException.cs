using System;
using System.Runtime.Serialization;

namespace HKH.Tasks.Configuration
{
	[Serializable]
	internal class HKHTaskSettingException: ApplicationException
	{
		/// <summary>
		/// do not allow creation of exception with no message
		/// </summary>
		private HKHTaskSettingException()
		{
		}

		/// <summary>
		/// Constructor takes problem message to be thrown
		/// invokes constructor on ApplicationException
		/// </summary>
		public HKHTaskSettingException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor takes problem message and caught exception
		/// invokes constructor on ApplicationException
		/// </summary>
		public HKHTaskSettingException(string message, System.Exception ex)
			:base(message, ex)
		{
		}
	}
}

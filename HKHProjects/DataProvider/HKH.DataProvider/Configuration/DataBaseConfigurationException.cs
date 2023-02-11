using System;
using System.Runtime.Serialization;

namespace HKH.Data.Configuration
{
	[Serializable]
	internal class DataBaseConfigurationException: ApplicationException
	{
		/// <summary>
		/// do not allow creation of exception with no message
		/// </summary>
		private DataBaseConfigurationException()
		{
		}

		/// <summary>
		/// Constructor takes problem message to be thrown
		/// invokes constructor on ApplicationException
		/// </summary>
		public DataBaseConfigurationException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor takes problem message and caught exception
		/// invokes constructor on ApplicationException
		/// </summary>
		public DataBaseConfigurationException(string message, System.Exception ex)
			:base(message, ex)
		{
		}

		protected DataBaseConfigurationException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

	}
}

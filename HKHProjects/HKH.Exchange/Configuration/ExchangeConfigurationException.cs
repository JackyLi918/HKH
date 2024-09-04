using System;
using System.Runtime.Serialization;

namespace HKH.Exchange.Configuration
{
    [Serializable]
    internal class ExchangeConfigurationException : ApplicationException
    {
        /// <summary>
        /// do not allow creation of exception with no message
        /// </summary>
        private ExchangeConfigurationException()
        {
        }

        /// <summary>
        /// Constructor takes problem message to be thrown
        /// invokes constructor on ApplicationException
        /// </summary>
        public ExchangeConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor takes problem message and caught exception
        /// invokes constructor on ApplicationException
        /// </summary>
        public ExchangeConfigurationException(string message, System.Exception ex)
            : base(message, ex)
        {
        }
    }
}

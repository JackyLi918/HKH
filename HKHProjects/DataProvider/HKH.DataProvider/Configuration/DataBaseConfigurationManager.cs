using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HKH.Data.Configuration
{
    public class DataBaseConfigurationManager
    {
        private static HKHConnectionStrings _connectionStrings = null;
        private static HKHConnectionString _defaultConnectionString = null;

        public static void Load(IConfiguration configuration)
        {
            _connectionStrings = HKHConnectionStrings.Load(configuration);

            if (_connectionStrings.Count > 0)
            {
                _defaultConnectionString = _connectionStrings.FirstOrDefault(x => x.Value.IsDefault).Value;

                //set the first one is default if no any config is marked as defaut explicitly.
                if (_defaultConnectionString == null)
                {
                    _defaultConnectionString = _connectionStrings.First().Value;
                    //_defaultConnectionString.IsDefault = true;
                }
            }
        }
        public static HKHConnectionString DefaultConfiguration
        {
            get { return _defaultConnectionString; }
        }

        public static HKHConnectionStrings Configurations
        {
            get { return _connectionStrings; }
        }

        public static HKHConnectionString GetConfiguration(string configurationName)
        {
            return _connectionStrings[configurationName];
        }
    }
}

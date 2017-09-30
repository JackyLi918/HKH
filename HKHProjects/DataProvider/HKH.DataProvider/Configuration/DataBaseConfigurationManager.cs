using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace HKH.Data.Configuration
{
	public class DataBaseConfigurationManager
	{
		private static HKHConnectionStringsSection _connectionStrings = null;
		private static HKHConnectionStringElement _defaultConnectionString = null;

		static DataBaseConfigurationManager()
		{
			_connectionStrings = (HKHConnectionStringsSection)ConfigurationManager.GetSection(HKHConnectionStringsSection.TagName);

			if (_connectionStrings.ConnectionStrings.Count > 0)
			{
				foreach (HKHConnectionStringElement config in _connectionStrings.ConnectionStrings)
				{
					if (config.IsDefault)
					{
						_defaultConnectionString = config;
						break;
					}
				}

				//set the first one is default if no any config is marked as defaut explicitly.
				if (_defaultConnectionString == null)
				{
					_defaultConnectionString = _connectionStrings.ConnectionStrings[0];
					//_defaultConnectionString.IsDefault = true;
				}
			}
			else
				throw new HKH.Data.DataBaseConfigurationException("No availiable connectionstring!");
		}

		public static HKHConnectionStringElement DefaultConfiguration
		{
			get { return _defaultConnectionString; }
		}

		public static HKHConnectionStringCollection Configurations
		{
			get { return _connectionStrings.ConnectionStrings; }
		}

		public static HKHConnectionStringElement GetConfiguration(string configurationName)
		{
			return _connectionStrings.ConnectionStrings[configurationName];
		}
	}
}

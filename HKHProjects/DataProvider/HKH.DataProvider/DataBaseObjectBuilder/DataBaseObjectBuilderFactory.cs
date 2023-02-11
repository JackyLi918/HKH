using System;
using HKH.Data.Configuration;

namespace HKH.Data
{
	internal static class DataBaseObjectBuilderFactory
	{
		#region private variable

		private static Type iBuilderType = typeof(IDataBaseObjectBuilder);

		#endregion

		#region Get the Instance of IDataBaseObjectBuilder

		/// <summary>
		/// Get default Instance of IDataBaseObjectBuilder
		/// </summary>
		/// <returns></returns>
		internal static IDataBaseObjectBuilder GetDataBaseObjectBuilder()
		{
			return GetDataBaseObjectBuilder(DataBaseConfigurationManager.DefaultConfiguration);
		}

		/// <summary>
		/// Get special Instance of IDataBaseObjectBuilder
		/// </summary>
		/// <param name="dbConfigName"></param>
		/// <returns></returns>
		internal static IDataBaseObjectBuilder GetDataBaseObjectBuilder(string dbConfigurationName)
		{
			return GetDataBaseObjectBuilder(DataBaseConfigurationManager.GetConfiguration(dbConfigurationName));
		}

		/// <summary>
		/// Get special Instance of IDataBaseObjectBuilder
		/// </summary>
		/// <param name="dbConfigName"></param>
		/// <param name="Conn"></param>
		/// <param name="Trans"></param>
		/// <returns></returns>
		internal static IDataBaseObjectBuilder GetDataBaseObjectBuilder(HKHConnectionString dbConfiguration)
		{
			return GetDataBaseObjectBuilder(Type.GetType(dbConfiguration.Builder), dbConfiguration.ConnectionString);
		}

		/// <summary>
		/// Get special Instance of IDataBaseObjectBuilder
		/// </summary>
		/// <param name="builderTypeName">the type of IDataBaseObjectBuilder--(TypeFullName,AssemblyName)</param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		internal static IDataBaseObjectBuilder GetDataBaseObjectBuilder(string builderTypeName, string connectionString)
		{
			return GetDataBaseObjectBuilder(Type.GetType(builderTypeName), connectionString);
		}

		/// <summary>
		/// Get special Instance of IDataBaseObjectBuilder
		/// </summary>
		/// <param name="builderType">the type of IDataBaseObjectBuilder</param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		internal static IDataBaseObjectBuilder GetDataBaseObjectBuilder(Type builderType, string connectionString)
		{
			if (IsValidBuilderType(builderType))
			{
				return (IDataBaseObjectBuilder)Activator.CreateInstance(builderType, new object[] { connectionString });
			}

			return null;
		}

		internal static bool IsValidBuilderType(Type builderType)
		{
			if (!iBuilderType.IsAssignableFrom(builderType))
			{
				throw new DataBaseConfigurationException("The BuilderType must implement IDataBaseObjectBuilder.");
			}

			return true;
		}

		#endregion
	}
}

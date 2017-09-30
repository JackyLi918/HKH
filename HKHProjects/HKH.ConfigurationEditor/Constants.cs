/*******************************************************
 * Filename: Constants.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/31/2014 1:10:00 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.ConfigurationEditor
{
	/// <summary>
	/// Constants
	/// </summary>
	public class Constants
	{
		public const string PROVIDERNAME_ENTITY = "System.Data.EntityClient";
		public const string PROVIDERNAME_SQLSERVER = "System.Data.SqlClient";

		public const string KEY_DATASOURCE = "Data Source";
		public const string KEY_DATABASE = "Initial Catalog";
		public const string KEY_INTEGRATEDSECURITY = "Integrated Security";
		public const string KEY_USERID = "User ID";
		public const string KEY_PASSWORD = "Password";
	}

	//public static class SqlConnectionStringBuilderExtension
	//{
	//	public static string Datasource(this SqlConnectionStringBuilder connStrBuilder, string serverName = null)
	//	{
	//		if (serverName == null)
	//			return connStrBuilder[Constants.KEY_DATASOURCE].ToString();
	//		else
	//			connStrBuilder[Constants.KEY_DATASOURCE] = serverName;

	//		return serverName;
	//	}

	//	public static string Database(this SqlConnectionStringBuilder connStrBuilder, string database = null)
	//	{
	//		if (database == null)
	//			return connStrBuilder[Constants.KEY_DATABASE].ToString();
	//		else
	//			connStrBuilder[Constants.KEY_DATABASE] = database;

	//		return database;
	//	}

	//	public static bool IntegratedSecurity(this SqlConnectionStringBuilder connStrBuilder, bool? winAuth = null)
	//	{
	//		if (winAuth == null)
	//			return Convert.ToBoolean(connStrBuilder[Constants.KEY_INTEGRATEDSECURITY]);
	//		else
	//			connStrBuilder[Constants.KEY_INTEGRATEDSECURITY] = winAuth.Value;

	//		return winAuth.Value;
	//	}

	//	public static string UserId(this SqlConnectionStringBuilder connStrBuilder, string userid = null)
	//	{
	//		if (userid == null)
	//			return connStrBuilder[Constants.KEY_USERID].ToString();
	//		else
	//			connStrBuilder[Constants.KEY_USERID] = userid;

	//		return userid;
	//	}

	//	public static string Password(this SqlConnectionStringBuilder connStrBuilder, string password = null)
	//	{
	//		if (password == null)
	//			return connStrBuilder[Constants.KEY_PASSWORD].ToString();
	//		else
	//			connStrBuilder[Constants.KEY_PASSWORD] = password;

	//		return password;
	//	}
	//}
}
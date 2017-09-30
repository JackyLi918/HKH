using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;
using HKH.Common;
using HKH.Data;
using System.Runtime.InteropServices;

namespace HKH.Tools.DbLocator
{
    public class SqlServerLocator
    {
        private const string ConnStr = "Data Source={0};Initial Catalog={1};";
        private const string WLogin = "Integrated Security=True;";
        private const string SLogin = "User ID={0};Password={1};";
        private const string builderType = "HKH.Data.SqlServer.SqlServerObjectBuilder,HKH.DataProvider";

        /// <summary>
        /// 获取所有Sql-Server服务器
        /// </summary>
        /// <returns></returns>
        public static List<string> GetServers()
        {
            List<string> servers = GetServersByODBC();//.Combine(GetServersByODBC()).Distinct().ToList();
            servers.Sort();
            return servers;
        }

        /// <summary>
        /// 获取指定服务器上的所有数据库
        /// </summary>
        /// <param name="srvName"></param>
        /// <param name="isSqlLogin">是否使用SQL Server登录方式</param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static List<string> GetDatabases(string srvName, bool isSqlLogin, string userName, string pwd)
        {
            List<string> databases = new List<string>();
            try
            {
                IDataProvider provider = DataProvider.GetInstance(builderType, GetConnectionString(srvName, "master", isSqlLogin, userName, pwd), false);
                using (ISafeDataReader reader = provider.ExecuteReader("sys.sp_databases", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString("DATABASE_NAME"));
                    }
                }
            }
            catch { }
            return databases;
        }

        /// <summary>
        /// 测试链接
        /// </summary>
        /// <param name="srvName"></param>
        /// <param name="dbName">要测试的数据库名称，默认为master</param>
        /// <param name="isSqlLogin">是否使用SQL Server登录方式</param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool TestConnection(string srvName, string dbName, bool isSqlLogin, string userName, string pwd)
        {
            IDbConnection conn = null;
            try
            {
                IDataProvider provider = DataProvider.GetInstance(builderType, GetConnectionString(srvName, "master", isSqlLogin, userName, pwd), false);
                conn = provider.Builder.GetConnection();
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private static string GetConnectionString(string srvName, string dbName, bool isSqlLogin, string userName, string pwd)
        {
            //Data Source=LWT;Initial Catalog=master;Integrated Security=True
            //Data Source=LWT;Initial Catalog=master;User ID=sa;Password=sa123

            if (isSqlLogin)
                return string.Format(ConnStr, srvName, dbName) + string.Format(SLogin, userName, pwd);
            else
                return string.Format(ConnStr, srvName, dbName) + WLogin;
        }

        #region SQLDMO获取不到实例名，不用
        /*
        /// <summary>
        /// 获取所有Sql-Server服务器
        /// </summary>
        /// <returns></returns>
        public static List<string> GetServers()
        {
            SQLDMO.Application app = null;
            SQLDMO.NameList serverList = null;
            try
            {
                app = new SQLDMO.Application();

                serverList = app.ListAvailableSQLServers();
                List<string> serverNames = new List<string>();
                for (int i = 1; i < serverList.Count; i++)
                {
                    serverNames.Add(serverList.Item(i));
                }
                return serverNames;
            }
            catch
            {
                return null;
            }
            finally
            {
                serverList = null;
                app = null;
            }
        }

        /// <summary>
        /// 获取指定服务器上的所有数据库
        /// </summary>
        /// <param name="srvName"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static List<string> GetDatabases(string srvName, string userName, string pwd)
        {
            SQLDMO.SQLServer server = null;
            try
            {
                server = new SQLDMO.SQLServer();
                server.Connect(srvName, userName, pwd);
                List<string> databases = new List<string>();
                for (int i = 1; i < server.Databases.Count; i++)
                    databases.Add(server.Databases.Item(i, Type.Missing).Name);
                return databases;
            }
            catch
            {
                return null;
            }
            finally
            {
                server.DisConnect();
                server.Close();
                server = null;
            }
        }

        /// <summary>
        /// 测试链接
        /// </summary>
        /// <param name="srvName"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool TestConnection(string srvName, string userName, string pwd)
        {
            SQLDMO.SQLServer server = null;
            try
            {
                server = new SQLDMO.SQLServer();
                server.Connect(srvName, userName, pwd);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                server.DisConnect();
                server.Close();
                server = null;
            }
        }
         */
        #endregion

        #region 获取Sql-Server服务器

        /// <summary>
        /// 获取所有Sql-Server服务器
        /// </summary>
        /// <returns></returns>
        public static List<string> GetServersByNet()
        {
            List<string> serverNames = new List<string>();
            try
            {
                foreach (DataRow row in SqlDataSourceEnumerator.Instance.GetDataSources().Rows)
                {
                    if (row["InstanceName"] != null && row["InstanceName"].ToString() != string.Empty)
                        serverNames.Add(row["ServerName"].ToString() + @"\" + row["InstanceName"].ToString());
                    else
                        serverNames.Add(row["ServerName"].ToString());
                }
            }
            catch { }

            return serverNames;
        }

        //配合微软的方法使用， 两种方法都不能得到所有实例
        [DllImport("odbc32.dll")]
        private static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);
        [DllImport("odbc32.dll")]
        private static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);
        [DllImport("odbc32.dll")]
        private static extern short SQLFreeHandle(short hType, IntPtr handle);
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
        private static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, short inStringLength, StringBuilder outString, short outStringLength, out short outLengthNeeded);
        private const short SQL_HANDLE_ENV = 1;
        private const short SQL_HANDLE_DBC = 2;
        private const int SQL_ATTR_ODBC_VERSION = 200;
        private const int SQL_OV_ODBC3 = 3;
        private const short SQL_SUCCESS = 0;
        private const short SQL_NEED_DATA = 99;
        private const short DEFAULT_RESULT_SIZE = 1024;
        private const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";
        private SqlServerLocator() { }

        private static List<string> GetServersByODBC()
        {
            List<string> retval = null;
            string txt = string.Empty;
            IntPtr henv = IntPtr.Zero;
            IntPtr hconn = IntPtr.Zero;
            StringBuilder inString = new StringBuilder(SQL_DRIVER_STR);
            StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
            short inStringLength = (short)inString.Length;
            short lenNeeded = 0;
            try
            {
                if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
                {
                    if (SQL_SUCCESS == SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (IntPtr)SQL_OV_ODBC3, 0))
                    {
                        if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
                        {
                            if (SQL_NEED_DATA == SQLBrowseConnect(hconn, inString, inStringLength, outString, DEFAULT_RESULT_SIZE, out lenNeeded))
                            {
                                if (DEFAULT_RESULT_SIZE < lenNeeded)
                                {
                                    outString.Capacity = lenNeeded;
                                    if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, lenNeeded, out lenNeeded))
                                    {
                                        throw new ApplicationException("Unabled to aquire SQL Servers from ODBC driver.");
                                    }
                                }

                                txt = outString.ToString();
                                int start = txt.IndexOf("{") + 1;
                                int len = txt.IndexOf("}") - start;
                                if ((start > 0) && (len > 0))
                                {
                                    txt = txt.Substring(start, len);
                                }
                                else
                                {
                                    txt = string.Empty;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                txt = string.Empty;
            }
            finally
            {
                if (hconn != IntPtr.Zero)
                {
                    SQLFreeHandle(SQL_HANDLE_DBC, hconn);
                }
                if (henv != IntPtr.Zero)
                {
                    SQLFreeHandle(SQL_HANDLE_ENV, hconn);
                }
            }
            if (txt.Length > 0)
            {
                retval = txt.Split(",".ToCharArray()).ToList();
            }
            return retval;
        }

        #endregion
    }
}

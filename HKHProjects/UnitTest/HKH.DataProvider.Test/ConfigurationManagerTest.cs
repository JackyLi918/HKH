using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common.Security;
using HKH.Data.Configuration;

namespace HKH.DataProvider.Test
{
	[TestClass]
	public class ConfigurationManagerTest
	{
		[TestMethod]
		public void TestDefaultConfiguration()
		{
			Assert.AreEqual(DataBaseConfigurationManager.DefaultConfiguration.Name, "sqlConn");
		}

		[TestMethod]
		public void TestGetConfiguration()
		{
            Assert.IsNotNull(DataBaseConfigurationManager.GetConfiguration("sqlConn"));
		}

		[TestMethod]
		public void TestDbEncryption()
		{
			string explainText = "Data Source=localhost;Initial Catalog=northwind;User ID=sa;Password=sa;Integrated Security=false;Connect Timeout=80;EnList=false;";
            DataBaseEncryption dbEncryption = new DataBaseEncryption();
			string encryptedText = dbEncryption.Encrypt(explainText);
			string decryptedText = dbEncryption.Decrypt(encryptedText);
			Assert.AreEqual(explainText, decryptedText);
		}
	}
}

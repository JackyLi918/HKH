using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common.Security;

namespace HKH.Common.Security.Test
{
	[TestClass]
	public class DesTest
	{
		private DES des;

		private string key = "JackyLi.918@hotmail.com";

		[TestInitialize]
		public void Init()
		{
			des = new DES(key);
		}

		[TestMethod]
		public void TestEncryptAndDecrypt()
		{
			string test = "I'm testing DES encryption.";

			string encrypt = des.Encrypt(test);

			Assert.AreEqual(test, des.Decrypt(encrypt));
		}
	}
}

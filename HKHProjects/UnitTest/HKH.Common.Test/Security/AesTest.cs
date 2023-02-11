using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKH.Common.Security;

namespace HKH.Common.Security.Test
{
	[TestClass]
	public class AesTest
	{
		private AES aes128;
		private AES aes192;
		private AES aes256;

		private string key = "JackyLi.918@hotmail.com";

		[TestInitialize]
		public void Init()
		{
			aes128 = new AES(AESKeySize.Bits128, key);
			aes192 = new AES(AESKeySize.Bits192, key);
			aes256 = new AES(AESKeySize.Bits256, key);
		}

		[TestMethod]
		public void TestEncryptAndDecrypt()
		{
			string test = "I'm testing AES encryption.";

			string encrypt128 = aes128.Encrypt(test);
			string encrypt192 = aes192.Encrypt(test);
			string encrypt256 = aes256.Encrypt(test);

			Assert.AreEqual(test, aes128.Decrypt(encrypt128));
			Assert.AreEqual(test, aes192.Decrypt(encrypt192));
			Assert.AreEqual(test, aes256.Decrypt(encrypt256));
		}
	}
}

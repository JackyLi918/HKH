using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Common.Security
{
	public class Base64 : SymmetricEncryption
	{
		public override string Encrypt(string toEncrypt)
		{
			return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(toEncrypt));
		}

		public override string Decrypt(string toDecrypt)
		{
			return ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(toDecrypt));
		}

		public static Base64 Create()
		{
			return new Base64();
		}
	}
}

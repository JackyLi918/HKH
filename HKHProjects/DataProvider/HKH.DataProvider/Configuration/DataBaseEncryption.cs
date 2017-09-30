using System;
using System.Collections.Generic;
using System.Text;
using HKH.Common.Security;

namespace HKH.Data.Configuration
{
	public class DataBaseEncryption : AES
	{
		private const string DEFAULT_KEY = "JackyLi.918@hotmail.com";
		private const string DEFAULT_IV = "Honkerhero";

		public DataBaseEncryption()
			: base(AesKeySize.Bits128, DEFAULT_KEY,DEFAULT_IV)
		{
		}
	}
}

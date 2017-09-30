using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace HKH.Common.Security
{
	public enum AesKeySize
	{
		Bits128 = 128,
		Bits192 = 192,
		Bits256 = 256
	}  // key size, in bits, for construtor

	public class AES : SymmetricEncryption
	{
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keySize"></param>
		/// <param name="key"></param>
		public AES(AesKeySize keySize, string key)
			: this(keySize, key, null)
		{
		}

		public AES(AesKeySize keySize, string key, string iv)
		{
			algorithm = new AesCryptoServiceProvider();
			algorithm.KeySize = (int)keySize;

			Initialize(key, iv, CipherMode.CBC);
		}

		#endregion

		public static AES Create(AesKeySize keySize, string key, string iv = null)
		{
			return new AES(keySize, key, iv);
		}
	}
}

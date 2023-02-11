using System.Security.Cryptography;

namespace HKH.Common.Security
{
	public enum AESKeySize
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
		public AES(AESKeySize keySize, string key)
			: this(keySize, key, null)
		{
		}

		public AES(AESKeySize keySize, string key, string iv)
		{
			algorithm = Aes.Create();
			algorithm.KeySize = (int)keySize;

			Initialize(key, iv, CipherMode.CBC);
		}

		#endregion

		public static AES Create(AESKeySize keySize, string key, string iv = null)
		{
			return new AES(keySize, key, iv);
		}
	}
}

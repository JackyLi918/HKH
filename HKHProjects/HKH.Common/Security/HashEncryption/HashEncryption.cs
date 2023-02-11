using System;
using System.Security.Cryptography;
using System.Text;

namespace HKH.Common.Security
{
	public abstract class HashEncryption : IEncryption
	{
		protected HashAlgorithm algorithm;

		#region IEncryption Members

		public virtual string Encrypt(string toEncrypt)
		{
			byte[] inputByteArray = Encoding.Default.GetBytes(toEncrypt);
			byte[] resultBypeArray = algorithm.ComputeHash(inputByteArray);

			StringBuilder builder = new StringBuilder();
			foreach (byte b in resultBypeArray)
				builder.AppendFormat("{0:X2}", b);

			return builder.ToString();
		}

		public string Decrypt(string toDecrypt)
		{
			throw new InvalidOperationException("Hash encryption doesn't support decrypt.");
		}

		#endregion
	}
}

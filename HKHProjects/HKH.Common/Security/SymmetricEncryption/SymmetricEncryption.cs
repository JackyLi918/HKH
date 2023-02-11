using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HKH.Common.Security
{
	public abstract class SymmetricEncryption : IEncryption
	{
		protected SymmetricAlgorithm algorithm;

		#region IEncryption Members

		public virtual string Encrypt(string toEncrypt)
		{
			byte[] inputByteArray = Encoding.Default.GetBytes(toEncrypt);

			MemoryStream memoryStream = new MemoryStream();
			CryptoStream encStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
			encStream.Write(inputByteArray, 0, inputByteArray.Length);
			encStream.FlushFinalBlock();

			StringBuilder builder = new StringBuilder();
			foreach (byte b in memoryStream.ToArray())
				builder.AppendFormat("{0:X2}", b);

			encStream.Close();
			memoryStream.Close();

			return builder.ToString();
		}

		public virtual string Decrypt(string toDecrypt)
		{
			byte[] inputByteArray = new byte[toDecrypt.Length / 2];
			for (int i = 0; i < inputByteArray.Length; i++)
				inputByteArray[i] = Convert.ToByte(toDecrypt.Substring(i * 2, 2), 16);

			MemoryStream memoryStream = new MemoryStream();
			CryptoStream decStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Write);
			decStream.Write(inputByteArray, 0, inputByteArray.Length);
			decStream.FlushFinalBlock();

			string result = Encoding.Default.GetString(memoryStream.ToArray());

			decStream.Close();
			memoryStream.Close();

			return result;
		}

		#endregion

		#region Helper

		protected void Initialize(string key,string iv,CipherMode mode)
		{
			int keyLength = algorithm.Key.Length;
			int ivLength = algorithm.IV.Length;

			algorithm.Key = ASCIIEncoding.ASCII.GetBytes(key.PadRight(keyLength, '*').Substring(0, keyLength));
			algorithm.IV = ASCIIEncoding.ASCII.GetBytes((iv ?? key.Reverse()).PadRight(ivLength, '*').Substring(0, ivLength));
			algorithm.Mode = mode;
		}

		#endregion
	}
}

namespace HKH.Common.Security
{
	public interface IEncryption
	{
		/// <summary>
		/// Encrypt a string
		/// </summary>
		/// <param name="toEncrypt">string to encrypt</param>
		/// <returns>encrypted string</returns>
		string Encrypt(string toEncrypt);

		/// <summary>
		/// Decrypt a string
		/// </summary>
		/// <param name="toDecrypt">string to decrypt</param>
		/// <returns>decrypted string</returns>
		string Decrypt(string toDecrypt);
	}
}

using System.Security.Cryptography;

namespace HKH.Common.Security
{
	public class DES : SymmetricEncryption
	{
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public DES(string key)
			: this(key, null)
		{
		}

		public DES(string key, string iv)
		{
			algorithm = System.Security.Cryptography.DES.Create();
			Initialize(key, iv, CipherMode.CBC);
		}

		#endregion

		public static DES Create(string key, string iv = null)
		{
			return new DES(key, iv);
		}
	}
}

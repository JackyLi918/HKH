namespace HKH.Common.Security
{
	public class MD5 : HashEncryption
	{
		public MD5()
		{
			algorithm = System.Security.Cryptography.MD5.Create();
		}

		public static MD5 Create()
		{
			return new MD5();
		}
	}
}

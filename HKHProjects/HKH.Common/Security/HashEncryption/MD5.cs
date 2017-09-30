using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

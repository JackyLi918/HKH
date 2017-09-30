using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Common.Security
{
    public enum SHAType
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class SHA : HashEncryption
    {
        public SHA()
            : this(SHAType.SHA1)
        {
        }

        public SHA(SHAType sType)
        {
            switch (sType)
            {
                case SHAType.SHA256:
                    algorithm = System.Security.Cryptography.SHA256Managed.Create();
                    break;
                case SHAType.SHA384:
                    algorithm = System.Security.Cryptography.SHA384Managed.Create();
                    break;
                case SHAType.SHA512:
                    algorithm = System.Security.Cryptography.SHA512Managed.Create();
                    break;
                default:
                    algorithm = System.Security.Cryptography.SHA1Managed.Create();
                    break;
            }
        }

        public static SHA Create()
        {
            return new SHA();
        }
    }
}

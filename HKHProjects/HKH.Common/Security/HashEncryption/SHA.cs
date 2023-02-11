using System.Security.Cryptography;

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
                    algorithm = SHA256.Create();
                    break;
                case SHAType.SHA384:
                    algorithm = SHA384.Create();
                    break;
                case SHAType.SHA512:
                    algorithm = SHA512.Create();
                    break;
                default:
                    algorithm = SHA1.Create();
                    break;
            }
        }

        public static SHA Create()
        {
            return new SHA();
        }
    }
}

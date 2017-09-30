using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace HKH.Common.Security
{
    public class TripleDES : SymmetricEncryption
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public TripleDES(string key)
            : this(key, null)
        {
        }

        public TripleDES(string key, string iv)
        {
            algorithm = new TripleDESCryptoServiceProvider();

            Initialize(key, iv, CipherMode.CBC);
        }

        #endregion

        public static TripleDES Create(string key, string iv = null)
        {
            return new TripleDES(key, iv);
        }
    }
}

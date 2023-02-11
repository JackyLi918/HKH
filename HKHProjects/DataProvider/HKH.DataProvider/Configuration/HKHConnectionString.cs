using System;
using System.Reflection;
using HKH.Common;
using HKH.Common.Security;
using Microsoft.Extensions.Configuration;

namespace HKH.Data.Configuration
{
    public class HKHConnectionString : INullable
    {
#if NET6_0_OR_GREATER
               private const string _defaultBuilder = "HKH.Data.SqlServer.MsSqlServerObjectBuilder,HKH.DataProvider.MsSqlServer"; 
#else
        private const string _defaultBuilder = "HKH.Data.SqlServer.SqlServerObjectBuilder,HKH.DataProvider.SqlServer";
#endif
        private const string _defaultAlgo = "HKH.Data.Configuration.DataBaseEncryption,HKH.DataProvider";

        public string Name { get; set; }
        private string _connectionString = string.Empty;
        private string _explainConnectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_explainConnectionString))
                {
                    _explainConnectionString = GetEncryptionAlgo().Decrypt(_connectionString);
                }
                return _explainConnectionString;
            }
            set
            {
                _connectionString = value;
                _explainConnectionString = string.Empty;
            }
        }
        /// <summary>
        /// database object builder
        /// </summary>
        public string Builder { get; set; }
        /// <summary>
        /// indicate whether this configuraton is default
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// indicate whether the connectionstring will be encryped.
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// gets or sets the encrypt algo
        /// </summary>
        public string Algo { get; set; }

        public IEncryption GetEncryptionAlgo()
        {
            if (!Encrypt)
                return NullEncryptionAlgo.Instance;

            var algo = (string.IsNullOrEmpty(Algo) ? _defaultAlgo : Algo);

            Type algoType = Type.GetType(algo);
            if (!typeof(IEncryption).IsAssignableFrom(algoType))
            {
                throw new DataBaseConfigurationException("The EncryptAlgo must implement IEncryption.");
            }

            return Activator.CreateInstance(algoType, null) as IEncryption;
        }

        public override string ToString()
        {
            return ConnectionString;
        }

        internal static HKHConnectionString Load(IConfigurationSection section)
        {
            if (section != null)
            {
                var connString = new HKHConnectionString();

                var name = section.GetSection("name").Value;
                if (string.IsNullOrEmpty(name)) { throw new DataBaseConfigurationException("name is required."); }
                connString.Name = name;

                var connectionString = section.GetSection("connectionString").Value;
                if (string.IsNullOrEmpty(connectionString)) { throw new DataBaseConfigurationException("connectionString is required."); }
                connString.ConnectionString = connectionString;

                var builder = section.GetSection("builder").Value;
                connString.Builder = (string.IsNullOrEmpty(builder) ? _defaultBuilder : builder);

                connString.IsDefault = section.GetSection("isDefault").Value.SafeToBool();
                connString.Encrypt = section.GetSection("encrypt").Value.SafeToBool();
                connString.Algo = section.GetSection("algo").Value;

                return connString;
            }
            return null;
        }

        #region INullable 成员

        public static HKHConnectionString Null
        {
            get { return NullHKHConnectionString.Instance; }
        }

        public virtual bool IsNull
        {
            get { return false; }
        }

        #endregion
    }
    internal sealed class NullHKHConnectionString : HKHConnectionString
    {
        private static NullHKHConnectionString self = new NullHKHConnectionString();

        #region Constructor

        private NullHKHConnectionString()
        {
            Name = "null";
            ConnectionString = Constants.Comma;
        }
        #endregion

        public static NullHKHConnectionString Instance
        {
            get { return self; }
        }

        #region Base Class Overrides

        public override bool IsNull
        {
            get { return true; }
        }

        #endregion
    }
    internal sealed class NullEncryptionAlgo : IEncryption
    {
        private static NullEncryptionAlgo self = null;

        static NullEncryptionAlgo()
        {
            self = new NullEncryptionAlgo();
        }

        internal static NullEncryptionAlgo Instance
        {
            get { return self; }
        }
        #region IEncryption Members

        public string Encrypt(string toEncrypt)
        {
            return toEncrypt;
        }

        public string Decrypt(string toDecrypt)
        {
            return toDecrypt;
        }
        #endregion
    }
}

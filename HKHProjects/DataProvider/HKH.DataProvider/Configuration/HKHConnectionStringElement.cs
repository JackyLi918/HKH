using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using HKH.Common;
using HKH.Common.Security;

namespace HKH.Data.Configuration
{
    public class HKHConnectionStringElement : ConfigurationElement, INullable
    {
        #region Const

        private const string _name = "name";
        private const string _connectionString = "connectionString";
        private const string _builder = "builder";
        private const string _defaultBuilder = "HKH.Data.SqlServer.SqlServerObjectBuilder,HKH.DataProvider";
        private const string _isDefault = "isDefault";
        private const string _encrypt = "encrypt";
        private const string _algo = "algo";
        private const string _defaultAlgo = "HKH.Data.Configuration.DataBaseEncryption,HKH.DataProvider";

        #endregion

        #region Static, to improve performance

        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        private static readonly ConfigurationProperty _propName = new ConfigurationProperty(_name, typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propConnectionString = new ConfigurationProperty(_connectionString, typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propBuilder = new ConfigurationProperty(_builder, typeof(string), _defaultBuilder);
        private static readonly ConfigurationProperty _propIsDefault = new ConfigurationProperty(_isDefault, typeof(bool), false);
        private static readonly ConfigurationProperty _propEncrypt = new ConfigurationProperty(_encrypt, typeof(bool), false);
        private static readonly ConfigurationProperty _propAlgo = new ConfigurationProperty(_algo, typeof(string), _defaultAlgo);

        #endregion

        #region Contructor

        static HKHConnectionStringElement()
        {
            _properties.Add(_propName);
            _properties.Add(_propConnectionString);
            _properties.Add(_propBuilder);
            _properties.Add(_propIsDefault);
            _properties.Add(_propEncrypt);
            _properties.Add(_propAlgo);
        }

        public HKHConnectionStringElement()
        {
        }

        public HKHConnectionStringElement(string name, string connectionString)
            : this(name, connectionString, _defaultBuilder)
        {
        }

        public HKHConnectionStringElement(string name, string connectionString, string builder)
            : this(name, connectionString, builder, false)
        {
        }

        public HKHConnectionStringElement(string name, string connectionString, string builder, bool isDefault)
            : this()
        {
            Name = name;
            ConnectionString = connectionString;
            Builder = builder;
            IsDefault = isDefault;
        }

        #endregion

        #region Element Attributes
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(_name,
            DefaultValue = "",
            IsKey = true,
            IsRequired = true)]
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)this[_propName]; }
            set { this[_propName] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(_connectionString,
            DefaultValue = "",
            IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                if (Encrypt)
                    return GetEncryptionAlgo().Decrypt((string)this[_propConnectionString]);
                return (string)this[_propConnectionString];
            }
            set
            {
                this[_propConnectionString] = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(_builder,
            DefaultValue = _defaultBuilder,
            IsRequired = false)]
        public string Builder
        {
            get { return (string)this[_propBuilder]; }
            set { this[_propBuilder] = value; }
        }


        /// <summary>
        /// indicate whether this configuraton is default
        /// </summary>
        [ConfigurationProperty(_isDefault,
            DefaultValue = false,
            IsRequired = false)]
        public bool IsDefault
        {
            get { return (bool)this[_propIsDefault]; }
            set { this[_propIsDefault] = value; }
        }

        /// <summary>
        /// indicate whether the connectionstring will be encryped.
        /// </summary>
        [ConfigurationProperty(_encrypt,
            DefaultValue = false,
            IsRequired = false)]
        public bool Encrypt
        {
            get { return (bool)this[_propEncrypt]; }
            set { this[_propEncrypt] = value; }
        }

        /// <summary>
        /// gets or sets the encrypt algo
        /// </summary>
        [ConfigurationProperty(_algo,
            DefaultValue = _defaultAlgo,
            IsRequired = false)]
        public string Algo
        {
            get { return (string)this[_propAlgo]; }
            set { this[_propAlgo] = value; }
        }

        public IEncryption GetEncryptionAlgo()
        {
            if (!Encrypt || string.IsNullOrEmpty(Algo))
                return NullEncryptionAlgo.Instance;

            Type algoType = Type.GetType(Algo);
            if (!typeof(IEncryption).IsAssignableFrom(algoType))
            {
                throw new HKH.Data.DataBaseConfigurationException("The EncryptAlgo must implement IEncryption.");
            }

            return Activator.CreateInstance(algoType, null) as IEncryption;
        }

        /// <summary>
        /// 
        /// </summary>
        internal string Key
        {
            get { return Name; }
        }

        #endregion

        public override string ToString()
        {
            return ConnectionString;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        #region INullable Members

        public static HKHConnectionStringElement Null
        {
            get { return NullHKHConnectionStringElement.Instance; }
        }

        public virtual bool IsNull
        {
            get { return false; }
        }

        #endregion
    }

    internal sealed class NullHKHConnectionStringElement : HKHConnectionStringElement
    {
        private static NullHKHConnectionStringElement self = new NullHKHConnectionStringElement();

        #region Constructor

        private NullHKHConnectionStringElement()
            : base("Null", Constants.Comma, string.Empty)
        {
        }
        #endregion

        public static NullHKHConnectionStringElement Instance
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

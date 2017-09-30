using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HKH.Data.Configuration
{
    public class HKHConnectionStringsSection : ConfigurationSection
    {
        public const string TagName = "hkhConnectionStrings";

        private static ConfigurationProperty _propConnectionStrings = new ConfigurationProperty(null, typeof(HKHConnectionStringCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        static HKHConnectionStringsSection()
        {
            _properties.Add(_propConnectionStrings);
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        protected override object GetRuntimeObject()
        {
            this.SetReadOnly();
            return this;
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public HKHConnectionStringCollection ConnectionStrings
        {
            get { return (HKHConnectionStringCollection)this[_propConnectionStrings]; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HKH.Tasks.Configuration
{
    public class HKHTaskSection : ConfigurationSection
    {
        public const string TagName = "hkh.tasks";

        private static ConfigurationProperty _hkhTasks = new ConfigurationProperty(null, typeof(HKHTaskCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        static HKHTaskSection()
        {
            _properties.Add(_hkhTasks);
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
        public HKHTaskCollection HKHTasks
        {
            get { return (HKHTaskCollection)this[_hkhTasks]; }
        }
    }
}

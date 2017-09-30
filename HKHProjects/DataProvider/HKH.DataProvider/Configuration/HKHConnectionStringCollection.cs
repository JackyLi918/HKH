using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HKH.Data.Configuration
{
    [ConfigurationCollection(typeof(HKHConnectionStringElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class HKHConnectionStringCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HKHConnectionStringElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HKHConnectionStringElement)element).Name;
        }

        // Properties
        public new HKHConnectionStringElement this[string name]
        {
            get
            {
                return (HKHConnectionStringElement)base.BaseGet(name);
            }
        }

        public HKHConnectionStringElement this[int index]
        {
            get
            {
                return (HKHConnectionStringElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        #region Add/Remove/Clear

        public void Add(HKHConnectionStringElement element)
        {
            this.BaseAdd(element);
        }

        public void BaseAdd (int index, HKHConnectionStringElement element)
        {
            if (index == -1)
            {
                base.BaseAdd(element, false);
            }
            else
            {
                base.BaseAdd(index, element);
            }
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public int IndexOf(HKHConnectionStringElement element)
        {
            return base.BaseIndexOf(element);
        }

        public void Remove(HKHConnectionStringElement element)
        {
            if (base.BaseIndexOf(element) >= 0)
            {
                base.BaseRemove(element.Key);
            }
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        #endregion
    }
}

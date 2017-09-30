using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HKH.Tasks.Configuration
{
    [ConfigurationCollection(typeof(HKHTaskElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class HKHTaskCollection : ConfigurationElementCollection
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
            return new HKHTaskElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HKHTaskElement)element).Name;
        }

        // Properties
        public new HKHTaskElement this[string name]
        {
            get
            {
                return (HKHTaskElement)base.BaseGet(name);
            }
        }

        public HKHTaskElement this[int index]
        {
            get
            {
                return (HKHTaskElement)base.BaseGet(index);
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

        public void Add(HKHTaskElement element)
        {
            this.BaseAdd(element);
        }

        public void BaseAdd (int index, HKHTaskElement element)
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

        public int IndexOf(HKHTaskElement element)
        {
            return base.BaseIndexOf(element);
        }

        public void Remove(HKHTaskElement element)
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

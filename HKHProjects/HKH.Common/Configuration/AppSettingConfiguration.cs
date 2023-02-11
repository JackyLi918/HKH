using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace HKH.Configuration
{
    public abstract class AppSettingConfiguration
    {
        public AppSettingConfiguration()
        {
            Initialize();
        }
        protected virtual void Initialize()
        {
            foreach (PropertyInfo p in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.IsDefined(typeof(AppSettingAttribute)))
                {
                    AppSettingAttribute attr = Attribute.GetCustomAttribute(p, typeof(AppSettingAttribute)) as AppSettingAttribute;
                    if (string.IsNullOrEmpty(attr.Key))
                        attr.Key = p.Name;

                    dynamic value = attr.GetValue();
                    if (value == null)
                        value = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null;
                    p.SetValue(this, Convert.ChangeType(value, p.PropertyType));
                }
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AppSettingAttribute : Attribute
    {
        public string Key { get; set; }
        public object DefaultValue { get; set; }
        public dynamic GetValue()
        {
            string value = null;
            if (TryGetAppSetting(Key, out value))
                return value;
            else
                return DefaultValue;
        }
        private bool TryGetAppSetting(string key, out string value)
        {
            bool result = false;
            value = string.Empty;

            //if (ConfigurationManager.AppSettings.AllKeys.Any(k => k == key))
            //{
            //    result = true;
            //    value = ConfigurationManager.AppSettings[key];
            //}

            return result;
        }
    }
}

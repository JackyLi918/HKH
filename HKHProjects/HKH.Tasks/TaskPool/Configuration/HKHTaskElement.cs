using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using HKH.Common;
using HKH.Common.Security;

namespace HKH.Tasks.Configuration
{
    public class HKHTaskElement : ConfigurationElement, INullable
    {
        #region Const

        private const string _name = "name";
        private const string _taskType = "taskType";
        private const string _taskQueueType = "taskQueueType";
        private const string _maxTaskCount = "maxTaskCount";

        private const string _defaultTaskQueueType = "HKH.Tasks.ParallelTaskQueue, HKH.Tasks";
        private const int _defautMaxTaskCount = 100;

        #endregion

        #region Static, to improve performance

        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        private static readonly ConfigurationProperty _propName = new ConfigurationProperty(_name, typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propTaskType = new ConfigurationProperty(_taskType, typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propTaskQueueType = new ConfigurationProperty(_taskQueueType, typeof(string), _defaultTaskQueueType);
        private static readonly ConfigurationProperty _propMaxTaskCountt = new ConfigurationProperty(_maxTaskCount, typeof(int), _defautMaxTaskCount);

        #endregion

        #region Contructor

        static HKHTaskElement()
        {
            _properties.Add(_propName);
            _properties.Add(_propTaskType);
            _properties.Add(_propTaskQueueType);
            _properties.Add(_propMaxTaskCountt);
        }

        public HKHTaskElement()
        {
        }

        public HKHTaskElement(string name, string taskType)
            : this(name, taskType, _defaultTaskQueueType)
        {
        }

        public HKHTaskElement(string name, string taskType, string taskQueueType)
            : this(name, taskType, taskQueueType, _defautMaxTaskCount)
        {
        }

        public HKHTaskElement(string name, string taskType, string taskQueueType, int maxTaskCount)
            : this()
        {
            Name = name;
            TaskType = Type.GetType(taskType);
            TaskQueueType = Type.GetType(taskQueueType);
            MaxTaskCount = maxTaskCount;
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
        [ConfigurationProperty(_taskType,
            DefaultValue = "",
            IsRequired = true)]
        public Type TaskType
        {
            get { return GetTaskType(); }
            set { this[_propTaskType] = value.AssemblyQualifiedName; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(_taskQueueType,
            DefaultValue = _defaultTaskQueueType,
            IsRequired = false)]
        public Type TaskQueueType
        {
            get { return GetTaskQueueType(); }
            set { this[_propTaskQueueType] = value.AssemblyQualifiedName; }
        }

        /// <summary>
        /// indicate whether this configuraton is default
        /// </summary>
        [ConfigurationProperty(_maxTaskCount,
            DefaultValue = _defautMaxTaskCount,
            IsRequired = false)]
        public int MaxTaskCount
        {
            get { return (int)this[_propMaxTaskCountt]; }
            set { this[_propMaxTaskCountt] = value; }
        }

        private Type GetTaskType()
        {
            Type taskType = Type.GetType(this[_propTaskType].ToString());
            if (!typeof(IParallelTask).IsAssignableFrom(taskType))
            {
                throw new ConfigurationErrorsException("The TaskType must implement IParallelTask.");
            }

            return taskType;
        }

        private Type GetTaskQueueType()
        {
            Type taskQueueType = Type.GetType(this[_propTaskQueueType].ToString());
            if (!typeof(IParallelTaskQueue).IsAssignableFrom(taskQueueType))
            {
                throw new ConfigurationErrorsException("The TaskQueueType must implement IParallelTaskQueue.");
            }

            return taskQueueType;
        }

        /// <summary>
        /// 
        /// </summary>
        internal string Key
        {
            get { return Name; }
        }

        #endregion

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        #region INullable Members

        public static HKHTaskElement Null
        {
            get { return NullHKHTaskElement.Instance; }
        }

        public virtual bool IsNull
        {
            get { return false; }
        }

        #endregion
    }

    internal sealed class NullHKHTaskElement : HKHTaskElement
    {
        private static NullHKHTaskElement self = new NullHKHTaskElement();

        #region Constructor

        private NullHKHTaskElement()
            : base("Null", Constants.Comma, string.Empty)
        {
        }
        #endregion

        public static NullHKHTaskElement Instance
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
}

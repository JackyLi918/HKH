using System;
using System.Configuration;
using HKH.Common;
using HKH.Common.Security;
using Microsoft.Extensions.Configuration;

namespace HKH.Tasks.Configuration
{
    public class HKHTaskSetting : INullable
    {
        private const string _defaultTaskQueueType = "HKH.Tasks.ParallelTaskQueue, HKH.Tasks";
        private const int _defautMaxTaskCount = 100;

        public string Name { get; set; }
        /// <summary>
        /// sub class of IParallelTask
        /// </summary>
        public Type TaskType { get; set; }
        /// <summary>
        /// sub class of IParallelTaskQueue
        /// </summary>
        public Type TaskQueueType { get; set; }
        /// <summary>
        /// max thread count is running,  other is inqueue to wait.
        /// </summary>
        public int MaxTaskCount { get; set; }

        private static Type GetTaskType(string taskTypeName)
        {
            Type taskType = Type.GetType(taskTypeName);
            if (!typeof(IParallelTask).IsAssignableFrom(taskType))
            {
                throw new HKHTaskSettingException("The TaskType must implement IParallelTask.");
            }

            return taskType;
        }

        private static Type GetTaskQueueType(string taskQueueTypeName)
        {
            Type taskQueueType = Type.GetType(taskQueueTypeName);
            if (!typeof(IParallelTaskQueue).IsAssignableFrom(taskQueueType))
            {
                throw new HKHTaskSettingException("The TaskQueueType must implement IParallelTaskQueue.");
            }

            return taskQueueType;
        }

        internal static HKHTaskSetting Load(IConfigurationSection section)
        {
            if (section != null)
            {
                var taskConfig = new HKHTaskSetting();

                var name = section.GetSection("name").Value;
                if (string.IsNullOrEmpty(name)) { throw new HKHTaskSettingException("name is required."); }
                taskConfig.Name = name;

                var taskTypeName = section.GetSection("taskType").Value;
                if (string.IsNullOrEmpty(taskTypeName)) { throw new HKHTaskSettingException("taskType is required."); }
                taskConfig.TaskType = GetTaskType(taskTypeName);

                var taskQueueTypeName = section.GetSection("taskQueueType").Value;
                taskConfig.TaskQueueType = GetTaskQueueType(string.IsNullOrEmpty(taskQueueTypeName) ? _defaultTaskQueueType : taskQueueTypeName);

                var maxTaskCount = section.GetSection("maxTaskCount").Value;
                taskConfig.MaxTaskCount = (string.IsNullOrEmpty(maxTaskCount) ? _defautMaxTaskCount : maxTaskCount.SafeToInt());

                return taskConfig;
            }
            return null;
        }

        #region INullable 成员

        public static HKHTaskSetting Null
        {
            get { return NullHKHTaskSetting.Instance; }
        }

        public virtual bool IsNull
        {
            get { return false; }
        }

        #endregion
    }
    internal sealed class NullHKHTaskSetting : HKHTaskSetting
    {
        private static NullHKHTaskSetting self = new NullHKHTaskSetting();

        #region Constructor

        private NullHKHTaskSetting()
        {
            Name = "null";
        }
        #endregion

        public static NullHKHTaskSetting Instance
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

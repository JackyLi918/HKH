﻿/*******************************************************
 * Filename: ParallelTaskPool.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:09:53 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HKH.Tasks.Configuration;
using Microsoft.Extensions.Configuration;

namespace HKH.Tasks
{
    /// <summary>
    /// ParallelTaskPool
    /// </summary>
    public class ParallelTaskPool
    {
        #region Variables

        private static TaskPoolState _state = TaskPoolState.Unstarted;
        private static ConcurrentDictionary<string, IParallelTaskQueue> _pool = new ConcurrentDictionary<string, IParallelTaskQueue>();

        #endregion

        #region Properties

        public static TaskPoolState State
        {
            get { return _state; }
        }

        public static bool IsIdle
        {
            get
            {
                if (_state == TaskPoolState.Unstarted || _state == TaskPoolState.Stopping)
                    return true;
                else if (_state == TaskPoolState.Running)
                {
                    return !_pool.Any(kvp => !kvp.Value.Dispatcher.IsIdle);
                }

                return false;
            }
        }

        #endregion

        #region Methods

        public static void Start(IConfiguration configuration)
        {
            if (_state == TaskPoolState.Unstarted)
            {
                _state = TaskPoolState.Starting;

                Init(configuration);

                foreach (var queue in _pool)
                {
                    new Thread(queue.Value.Dispatcher.Start).Start();
                }
                _state = TaskPoolState.Running;
            }
        }

        public static void Stop()
        {
            if (_state == TaskPoolState.Running)
            {
                _state = TaskPoolState.Stopping;
                foreach (var kvp in _pool)
                {
                    kvp.Value.Dispatcher.Stop();
                }

                while (_pool.Any(kvp => !kvp.Value.Dispatcher.IsIdle))
                    Thread.Sleep(1000);

                foreach (var kvp in _pool)
                {
                    kvp.Value.Dispose();
                }

                _pool.Clear();
                _state = TaskPoolState.Unstarted;
            }
        }

        public static void Enqueue(IParallelTask task)
        {
            string taskType = task.GetType().FullName;
            if (!_pool.ContainsKey(taskType))
            {
                throw new IndexOutOfRangeException(string.Format("The Task Queue to {0} no exists.", taskType));
            }
            if (!_pool[taskType].Contains(task))
                _pool[taskType].TryAdd(task);
        }

        public static void AddTaskQueue(HKHTaskSetting setting)
        {
            string taskType = setting.TaskType.FullName;

            if (!_pool.ContainsKey(taskType))
            {
                ParallelTaskQueue taskQueue = Activator.CreateInstance(setting.TaskQueueType) as ParallelTaskQueue;
                ParallelTaskDispatcher dispatcher = new ParallelTaskDispatcher(setting, taskQueue);
                taskQueue.Dispatcher = dispatcher;

                if (_pool.TryAdd(setting.TaskType.FullName, taskQueue) && _state == TaskPoolState.Running)
                {
                    new Thread(dispatcher.Start).Start();
                }
            }
        }

        #endregion

        #region Helper

        private static void Init(IConfiguration configuration)
        {
            var settings = HKHTaskSettings.Load(configuration);
            if (settings?.Count > 0)
            {
                foreach (var setting in settings.Values)
                {
                    AddTaskQueue(setting);
                }
            }
        }

        #endregion
    }

    public enum TaskPoolState
    {
        Unstarted,
        Starting,
        Running,
        Stopping
    }
}
/*******************************************************
 * Filename: ParallelTaskDispatcher.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:13:36 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HKH.Tasks.Configuration;

namespace HKH.Tasks
{
	/// <summary>
	/// ParallelTaskDispatcher
	/// </summary>
	public class ParallelTaskDispatcher : IDisposable
	{
		#region Variables

		int _runningTaskCount = 0;
		bool _isRunning = false;

		int _sleepInterval = 100;
		HKHTaskElement _setting = null;
		ParallelTaskQueue _queue = null;

		#endregion

		public ParallelTaskDispatcher(HKHTaskElement setting, ParallelTaskQueue taskQueue)
		{
			_setting = setting;
			_queue = taskQueue;
		}

		#region Properties

		public HKHTaskElement Setting
		{
			get { return _setting; }
		}

		public ParallelTaskQueue TaskQueue
		{
			get { return _queue; }
		}

		public bool IsIdle
		{
			get { return _runningTaskCount == 0; }
		}

		#endregion

		#region Methods

		public void Start()
		{
			_isRunning = true;
			ExecuteTasks();
		}

		public void Stop()
		{
			_isRunning = false;
		}

		private void ExecuteTasks()
		{
			while (_isRunning)
			{
				try
				{
					if (TaskQueue.Count == 0)
					{
						TaskQueue.LoadTasks();
						Thread.Sleep(_sleepInterval);
					}
					else
					{
						if (_runningTaskCount < Setting.MaxTaskCount)
						{
							Interlocked.Increment(ref _runningTaskCount);

							Task.Factory.StartNew<ParallelTaskResult>(() =>
							{
								IParallelTask _task = null;
								if (TaskQueue.TryDequeue(out _task))
									return _task.Execute();
								else
									return null;
							})
							.ContinueWith<ParallelTaskResult>(t =>
							{
								if (t.IsFaulted)
								{
									t.Result.Task.OnException(t.Result, t.Exception);
								}

								Interlocked.Decrement(ref _runningTaskCount);
								return t.Result;
							});
						}
						else
							Thread.Sleep(_sleepInterval);
					}
				}
				catch
				{
					//keep thread was not aborted.
				}
			}

			while (_runningTaskCount > 0)
				Thread.Sleep(_sleepInterval);
		}

		#endregion

		#region Helper

		#endregion

		public void Dispose()
		{
			while (_runningTaskCount > 0)
				Thread.Sleep(_sleepInterval);

			_setting = null;
			_queue = null;
		}
	}
}
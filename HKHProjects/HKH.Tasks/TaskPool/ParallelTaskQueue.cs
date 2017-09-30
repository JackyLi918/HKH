/*******************************************************
 * Filename: ParallelTaskQueue.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 12:23:50 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Tasks
{
	/// <summary>
	/// ParallelTaskQueue
	/// </summary>
	public class ParallelTaskQueue : ConcurrentQueue<IParallelTask>, IParallelTaskQueue
	{
		#region Variables

		#endregion

		#region Properties

		public ParallelTaskDispatcher Dispatcher { get; set; }

		#endregion

		#region Methods

		public virtual void LoadTasks()
		{

		}

		public virtual void OnStop()
		{

		}

		public virtual void Dispose()
		{
			this.OnStop();

			this.Dispatcher.Dispose();
			this.Dispatcher = null;
		}

		#endregion

		#region Helper

		#endregion
	}
}
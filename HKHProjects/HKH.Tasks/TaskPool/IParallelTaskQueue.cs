/*******************************************************
 * Filename: IParallelTaskQueue.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/6/2013 1:06:46 PM
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
	/// IParallelTaskQueue
	/// </summary>
	public interface IParallelTaskQueue : IProducerConsumerCollection<IParallelTask>, IDisposable
	{
		#region Variables

		#endregion

		#region Properties

		ParallelTaskDispatcher Dispatcher { get; set; }

		bool IsEmpty { get; }

		#endregion

		#region Methods

		void LoadTasks();

		void OnStop();

		#endregion

		#region Helper

		#endregion
	}
}
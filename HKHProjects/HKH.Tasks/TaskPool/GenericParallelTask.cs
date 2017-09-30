/*******************************************************
 * Filename: GenericParallelTask.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/30/2013 11:52:28 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Tasks
{
	/// <summary>
	/// GenericParallelTask
	/// </summary>
    public class ParallelTask<T> : ParallelTaskBase, IParallelTask
	{
		#region Variables

		Action<T> _callback;
		T _tState;

		#endregion

		public ParallelTask(Action<T> callback, T state)
		{
			this._callback = callback;
			this._tState = state;
		}

		#region Properties

		#endregion

		#region Methods

		protected override void ExecuteCore(ParallelTaskResult result)
		{
			_callback(_tState);
		}

		#endregion

		#region Helper

		#endregion
	}
}
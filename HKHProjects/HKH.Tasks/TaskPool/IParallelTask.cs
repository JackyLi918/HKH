/*******************************************************
 * Filename: IParallelTask.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:04:49 AM
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
	/// IParallelTask
	/// </summary>
	public interface IParallelTask 
	{
		#region Variables

		#endregion

		#region Properties

		string Id { get; set; }

		ParallelTaskState State { get; }

		#endregion

		#region Methods

		ParallelTaskResult Execute();

		void OnException(ParallelTaskResult result,AggregateException ex);

		#endregion

		#region Helper

		#endregion
	}
}
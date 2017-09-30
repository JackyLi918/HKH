/*******************************************************
 * Filename: TaskResult.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:20:31 AM
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
	/// TaskResult
	/// </summary>
	public class ParallelTaskResult
	{
		#region Variables

		#endregion

		#region Properties

		public ExecutionStatus Status { get; set; }

		public string Message { get; set; }

		public Exception Exception { get; set; }

		public IParallelTask Task { get; set; }

		public object ExecutionResult { get; set; }

		#endregion

		#region Methods

		#endregion

		#region Helper

		#endregion
	}
}
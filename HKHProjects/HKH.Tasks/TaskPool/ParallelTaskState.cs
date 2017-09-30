/*******************************************************
 * Filename: TaskExecuteState.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:21:31 AM
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
	/// TaskExecuteState
	/// </summary>
	public enum ParallelTaskState
	{
		Queued = 0,
		Scheduled = 1,
		Processing = 2,
		Finished = 3,
		Error = 4,
		Canceled = 5,
		Canceling = 6
	}
}
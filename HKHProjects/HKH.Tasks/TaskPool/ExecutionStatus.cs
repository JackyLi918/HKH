/*******************************************************
 * Filename: ExecutionStatus.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:49:17 AM
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
	/// ExecutionStatus
	/// </summary>
	public enum ExecutionStatus
	{
		OK = 0,
		Fail = 1,
		Error = 2,
		Canceled = 3
	}
}
/*******************************************************
 * Filename: MemberOperation.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/26/2013 5:55:47 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Data.SqlDatabase
{
	public class MemberOperation
	{
		public Exception Exception { get; set; }

		public Metadata.Member Member { get; set; }
	}
}
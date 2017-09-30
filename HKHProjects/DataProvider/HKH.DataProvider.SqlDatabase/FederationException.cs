/*******************************************************
 * Filename: FederationException.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/26/2013 5:56:13 PM
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
	[Serializable]
	public class FederationException : Exception
	{
		public FederationException()
		{
		}

		public IEnumerable<MemberOperation> Operations { get; set; }
	}
}
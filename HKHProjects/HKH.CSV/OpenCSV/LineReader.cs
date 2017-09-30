/*******************************************************
 * Filename: LineReader.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/05/2015 4:07:50 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.CSV
{
	/// <summary>
	/// LineReader
	/// </summary>
	public class LineReader
	{
		#region Variables

		private StreamReader reader;
		private bool keepCarriageReturns;

		#endregion

		#region Properties

		#endregion

		public LineReader(StreamReader reader, bool keepCarriageReturns)
		{
			this.reader = reader;
			this.keepCarriageReturns = keepCarriageReturns;
		}

		#region Methods

		public string ReadLine()
		{
			return keepCarriageReturns ? readUntilNewline() : reader.ReadLine();
		}

		#endregion

		#region Helper

		private string readUntilNewline()
		{
			StringBuilder sb = new StringBuilder();
			for (int c = reader.Read(); c > -1 && c != '\n'; c = reader.Read())
			{
				sb.Append((char)c);
			}

			return sb.Length > 0 ? sb.ToString() : null;
		}

		#endregion
	}
}
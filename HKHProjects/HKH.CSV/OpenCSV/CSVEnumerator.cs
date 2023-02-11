/*******************************************************
 * Filename: CSVEnumerator.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/05/2015 4:44:49 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HKH.CSV
{
	/// <summary>
	/// 
	/// </summary>
	internal class CSVEnumerator : IEnumerator<IEnumerable<string>>, IEnumerator
	{
		private CSVReader reader;
		private IEnumerable<string> nextLine;

		public CSVEnumerator(CSVReader reader)
		{
			this.reader = reader;
			nextLine = reader.ReadLine();
		}

		public IEnumerable<string> Current
		{
			get { return nextLine; }
		}

		public void Dispose()
		{
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			nextLine = reader.ReadLine();
			return nextLine != null && nextLine.Count() > 0;
		}

		public void Reset()
		{
		}

		bool IEnumerator.MoveNext()
		{
			return MoveNext();
		}

		void IEnumerator.Reset()
		{
			Reset();
		}
	}
}
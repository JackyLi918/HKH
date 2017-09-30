using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace HKH.Data
{
	public interface ICommandBuiler
	{
		#region Methods

		void BuildCommands(IDbDataAdapter adapter);

		#endregion
	}
}

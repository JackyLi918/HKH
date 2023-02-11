using System.Data;

namespace HKH.Data
{
	public interface ICommandBuiler
	{
		#region Methods

		void BuildCommands(IDbDataAdapter adapter);

		#endregion
	}
}

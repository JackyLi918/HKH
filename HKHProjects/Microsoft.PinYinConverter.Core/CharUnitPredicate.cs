namespace Microsoft.International.Converters.PinYinConverter
{
	internal class CharUnitPredicate
	{
		private char ExpectedChar;

		internal CharUnitPredicate(char ch)
		{
			ExpectedChar = ch;
		}

		internal bool Match(CharUnit charUnit)
		{
			return charUnit.Char == ExpectedChar;
		}
	}
}
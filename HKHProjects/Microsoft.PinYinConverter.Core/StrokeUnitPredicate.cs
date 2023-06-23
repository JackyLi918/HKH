namespace Microsoft.International.Converters.PinYinConverter
{
	internal class StrokeUnitPredicate
	{
		private int ExpectedStrokeNum;

		internal StrokeUnitPredicate(int strokeNum)
		{
			ExpectedStrokeNum = strokeNum;
		}

		internal bool Match(StrokeUnit strokeUnit)
		{
			return strokeUnit.StrokeNumber == ExpectedStrokeNum;
		}
	}
}
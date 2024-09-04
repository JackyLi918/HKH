using System.Globalization;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class PinyinUnitPredicate
	{
		private string ExpectedPinyin;

		internal PinyinUnitPredicate(string pinyin)
		{
			ExpectedPinyin = pinyin;
		}

		internal bool Match(PinyinUnit pinyinUnit)
		{
			return string.Compare(pinyinUnit.Pinyin, ExpectedPinyin, ignoreCase: true, CultureInfo.CurrentCulture) == 0;
		}
	}
}
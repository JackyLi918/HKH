using System.IO;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class CharUnit
	{
		internal char Char;

		internal byte StrokeNumber;

		internal byte PinyinCount;

		internal short[] PinyinIndexList;

		internal static CharUnit Deserialize(BinaryReader binaryReader)
		{
			CharUnit charUnit = new CharUnit();
			charUnit.Char = binaryReader.ReadChar();
			charUnit.StrokeNumber = binaryReader.ReadByte();
			charUnit.PinyinCount = binaryReader.ReadByte();
			charUnit.PinyinIndexList = new short[charUnit.PinyinCount];
			for (int i = 0; i < charUnit.PinyinCount; i++)
			{
				charUnit.PinyinIndexList[i] = binaryReader.ReadInt16();
			}
			return charUnit;
		}

		internal void Serialize(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Char);
			binaryWriter.Write(StrokeNumber);
			binaryWriter.Write(PinyinCount);
			for (int i = 0; i < PinyinCount; i++)
			{
				binaryWriter.Write(PinyinIndexList[i]);
			}
		}
	}
}

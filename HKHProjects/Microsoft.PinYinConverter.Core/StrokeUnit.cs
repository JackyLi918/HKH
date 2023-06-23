using System.IO;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class StrokeUnit
	{
		internal byte StrokeNumber;

		internal short CharCount;

		internal char[] CharList;

		internal static StrokeUnit Deserialize(BinaryReader binaryReader)
		{
			StrokeUnit strokeUnit = new StrokeUnit();
			strokeUnit.StrokeNumber = binaryReader.ReadByte();
			strokeUnit.CharCount = binaryReader.ReadInt16();
			strokeUnit.CharList = new char[strokeUnit.CharCount];
			for (int i = 0; i < strokeUnit.CharCount; i++)
			{
				strokeUnit.CharList[i] = binaryReader.ReadChar();
			}
			return strokeUnit;
		}

		internal void Serialize(BinaryWriter binaryWriter)
		{
			if (CharCount != 0)
			{
				binaryWriter.Write(StrokeNumber);
				binaryWriter.Write(CharCount);
				binaryWriter.Write(CharList);
			}
		}
	}
}

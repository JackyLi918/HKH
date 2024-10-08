using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class StrokeDictionary
	{
		internal const short MaxStrokeNumber = 48;

		internal int Length;

		internal int Count;

		internal short Offset;

		internal readonly byte[] Reserved = new byte[24];

		internal List<StrokeUnit> StrokeUnitTable;

		internal readonly short EndMark = short.MaxValue;

		internal void Serialize(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Length);
			binaryWriter.Write(Count);
			binaryWriter.Write(Offset);
			binaryWriter.Write(Reserved);
			for (int i = 0; i < Count; i++)
			{
				StrokeUnitTable[i].Serialize(binaryWriter);
			}
			binaryWriter.Write(EndMark);
		}

		internal static StrokeDictionary Deserialize(BinaryReader binaryReader)
		{
			StrokeDictionary strokeDictionary = new StrokeDictionary();
			binaryReader.ReadInt32();
			strokeDictionary.Length = binaryReader.ReadInt32();
			strokeDictionary.Count = binaryReader.ReadInt32();
			strokeDictionary.Offset = binaryReader.ReadInt16();
			binaryReader.ReadBytes(24);
			strokeDictionary.StrokeUnitTable = new List<StrokeUnit>();
			for (int i = 0; i < strokeDictionary.Count; i++)
			{
				strokeDictionary.StrokeUnitTable.Add(StrokeUnit.Deserialize(binaryReader));
			}
			binaryReader.ReadInt16();
			return strokeDictionary;
		}

		internal StrokeUnit GetStrokeUnitByIndex(int index)
		{
			if (index < 0 || index >= Count)
			{
				throw new ArgumentOutOfRangeException("index", "The index is out of range.");
			}
			return StrokeUnitTable[index];
		}

		internal StrokeUnit GetStrokeUnit(int strokeNum)
		{
			if (strokeNum <= 0 || strokeNum > 48)
			{
				throw new ArgumentOutOfRangeException("strokeNum");
			}
			StrokeUnitPredicate @object = new StrokeUnitPredicate(strokeNum);
			return StrokeUnitTable.Find(@object.Match);
		}
	}
}

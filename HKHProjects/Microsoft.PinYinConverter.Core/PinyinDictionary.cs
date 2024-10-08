using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class PinyinDictionary
	{
		internal short Length;

		internal short Count;

		internal short Offset;

		internal readonly byte[] Reserved = new byte[8];

		internal List<PinyinUnit> PinyinUnitTable;

		internal readonly short EndMark = short.MaxValue;

		internal void Serialize(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Length);
			binaryWriter.Write(Count);
			binaryWriter.Write(Offset);
			binaryWriter.Write(Reserved);
			for (int i = 0; i < Count; i++)
			{
				PinyinUnitTable[i].Serialize(binaryWriter);
			}
			binaryWriter.Write(EndMark);
		}

		internal static PinyinDictionary Deserialize(BinaryReader binaryReader)
		{
			PinyinDictionary pinyinDictionary = new PinyinDictionary();
			binaryReader.ReadInt32();
			pinyinDictionary.Length = binaryReader.ReadInt16();
			pinyinDictionary.Count = binaryReader.ReadInt16();
			pinyinDictionary.Offset = binaryReader.ReadInt16();
			binaryReader.ReadBytes(8);
			pinyinDictionary.PinyinUnitTable = new List<PinyinUnit>();
			for (int i = 0; i < pinyinDictionary.Count; i++)
			{
				pinyinDictionary.PinyinUnitTable.Add(PinyinUnit.Deserialize(binaryReader));
			}
			binaryReader.ReadInt16();
			return pinyinDictionary;
		}

		internal int GetPinYinUnitIndex(string pinyin)
		{
			PinyinUnitPredicate @object = new PinyinUnitPredicate(pinyin);
			return PinyinUnitTable.FindIndex(@object.Match);
		}

		internal PinyinUnit GetPinYinUnit(string pinyin)
		{
			PinyinUnitPredicate @object = new PinyinUnitPredicate(pinyin);
			return PinyinUnitTable.Find(@object.Match);
		}

		internal PinyinUnit GetPinYinUnitByIndex(int index)
		{
			if (index < 0 || index >= Count)
			{
				throw new ArgumentOutOfRangeException("index", "The index is out of range.");
			}
			return PinyinUnitTable[index];
		}
	}
}
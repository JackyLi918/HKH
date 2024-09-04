using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.International.Converters.PinYinConverter
{
	internal class HomophoneDictionary
	{
		internal int Length;

		internal short Offset;

		internal short Count;

		internal readonly byte[] Reserved = new byte[8];

		internal List<HomophoneUnit> HomophoneUnitTable;

		internal readonly short EndMark = short.MaxValue;

		internal void Serialize(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Length);
			binaryWriter.Write(Count);
			binaryWriter.Write(Offset);
			binaryWriter.Write(Reserved);
			for (int i = 0; i < Count; i++)
			{
				HomophoneUnitTable[i].Serialize(binaryWriter);
			}
			binaryWriter.Write(EndMark);
		}

		internal static HomophoneDictionary Deserialize(BinaryReader binaryReader)
		{
			HomophoneDictionary homophoneDictionary = new HomophoneDictionary();
			binaryReader.ReadInt32();
			homophoneDictionary.Length = binaryReader.ReadInt32();
			homophoneDictionary.Count = binaryReader.ReadInt16();
			homophoneDictionary.Offset = binaryReader.ReadInt16();
			binaryReader.ReadBytes(8);
			homophoneDictionary.HomophoneUnitTable = new List<HomophoneUnit>();
			for (int i = 0; i < homophoneDictionary.Count; i++)
			{
				homophoneDictionary.HomophoneUnitTable.Add(HomophoneUnit.Deserialize(binaryReader));
			}
			binaryReader.ReadInt16();
			return homophoneDictionary;
		}

		internal HomophoneUnit GetHomophoneUnit(int index)
		{
			if (index < 0 || index >= Count)
			{
				throw new ArgumentOutOfRangeException("index", "The index is out of range.");
			}
			return HomophoneUnitTable[index];
		}

		internal HomophoneUnit GetHomophoneUnit(PinyinDictionary pinyinDictionary, string pinyin)
		{
			return GetHomophoneUnit(pinyinDictionary.GetPinYinUnitIndex(pinyin));
		}
	}
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Microsoft.International.Converters.PinYinConverter
{
    public class ChineseChar
    {
        private const short MaxPolyphoneNum = 8;

        private static CharDictionary charDictionary;

        private static PinyinDictionary pinyinDictionary;

        private static StrokeDictionary strokeDictionary;

        private static HomophoneDictionary homophoneDictionary;

        private char chineseCharacter;

        private short strokeNumber;

        private bool isPolyphone;

        private short pinyinCount;

        private string[] pinyinList = new string[8];

        public short PinyinCount => pinyinCount;

        public short StrokeNumber => strokeNumber;

        public bool IsPolyphone => isPolyphone;

        public ReadOnlyCollection<string> Pinyins => new ReadOnlyCollection<string>(pinyinList);

        public char ChineseCharacter => chineseCharacter;

        static ChineseChar()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string resourceType;
            byte[] resourceData;
            using (Stream stream = executingAssembly.GetManifestResourceStream("Microsoft.PinYinConverter.Core.PinyinDictionary.resources"))
            {
                using (ResourceReader resourceReader = new ResourceReader(stream))
                {
                    resourceReader.GetResourceData("PinyinDictionary", out resourceType, out resourceData);
                    using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(resourceData)))
                    {
                        pinyinDictionary = PinyinDictionary.Deserialize(binaryReader);
                    }
                }
            }
            using (Stream stream2 = executingAssembly.GetManifestResourceStream("Microsoft.PinYinConverter.Core.CharDictionary.resources"))
            {
                using (ResourceReader resourceReader2 = new ResourceReader(stream2))
                {
                    resourceReader2.GetResourceData("CharDictionary", out resourceType, out resourceData);
                    using (BinaryReader binaryReader2 = new BinaryReader(new MemoryStream(resourceData)))
                    {
                        charDictionary = CharDictionary.Deserialize(binaryReader2);
                    }
                }
            }
            using (Stream stream3 = executingAssembly.GetManifestResourceStream("Microsoft.PinYinConverter.Core.HomophoneDictionary.resources"))
            {
                using (ResourceReader resourceReader3 = new ResourceReader(stream3))
                {
                    resourceReader3.GetResourceData("HomophoneDictionary", out resourceType, out resourceData);
                    using (BinaryReader binaryReader3 = new BinaryReader(new MemoryStream(resourceData)))
                    {
                        homophoneDictionary = HomophoneDictionary.Deserialize(binaryReader3);
                    }
                }
            }
            using (Stream stream4 = executingAssembly.GetManifestResourceStream("Microsoft.PinYinConverter.Core.StrokeDictionary.resources"))
            {
                using (ResourceReader resourceReader4 = new ResourceReader(stream4))
                {
                    resourceReader4.GetResourceData("StrokeDictionary", out resourceType, out resourceData);
                    using (BinaryReader binaryReader4 = new BinaryReader(new MemoryStream(resourceData)))
                    {
                        strokeDictionary = StrokeDictionary.Deserialize(binaryReader4);
                    }
                }
            }
        }

        public ChineseChar(char ch)
        {
            if (!IsValidChar(ch))
            {
                throw new NotSupportedException("The character is not in extended character set of Simplified Chinese.");
            }
            chineseCharacter = ch;
            CharUnit charUnit = charDictionary.GetCharUnit(ch);
            strokeNumber = charUnit.StrokeNumber;
            pinyinCount = charUnit.PinyinCount;
            isPolyphone = charUnit.PinyinCount > 1;
            for (int i = 0; i < pinyinCount; i++)
            {
                PinyinUnit pinYinUnitByIndex = pinyinDictionary.GetPinYinUnitByIndex(charUnit.PinyinIndexList[i]);
                pinyinList[i] = pinYinUnitByIndex.Pinyin;
            }
        }

        public bool HasSound(string pinyin)
        {
            if (pinyin == null)
            {
                throw new ArgumentNullException("HasSound_pinyin");
            }
            for (int i = 0; i < PinyinCount; i++)
            {
                if (string.Compare(Pinyins[i], pinyin, ignoreCase: true, CultureInfo.CurrentCulture) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsHomophone(char ch)
        {
            return IsHomophone(chineseCharacter, ch);
        }

        public static bool IsHomophone(char ch1, char ch2)
        {
            CharUnit charUnit = charDictionary.GetCharUnit(ch1);
            CharUnit charUnit2 = charDictionary.GetCharUnit(ch2);
            return ExistSameElement(charUnit.PinyinIndexList, charUnit2.PinyinIndexList);
        }

        public int CompareStrokeNumber(char ch)
        {
            CharUnit charUnit = charDictionary.GetCharUnit(ch);
            return StrokeNumber - charUnit.StrokeNumber;
        }

        public static char[] GetChars(string pinyin)
        {
            if (pinyin == null)
            {
                throw new ArgumentNullException("pinyin");
            }
            if (!IsValidPinyin(pinyin))
            {
                return null;
            }
            HomophoneUnit homophoneUnit = homophoneDictionary.GetHomophoneUnit(pinyinDictionary, pinyin);
            return homophoneUnit.HomophoneList;
        }

        public static bool IsValidPinyin(string pinyin)
        {
            if (pinyin == null)
            {
                throw new ArgumentNullException("pinyin");
            }
            if (pinyinDictionary.GetPinYinUnitIndex(pinyin) < 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidChar(char ch)
        {
            CharUnit charUnit = charDictionary.GetCharUnit(ch);
            return charUnit != null;
        }

        public static bool IsValidStrokeNumber(short strokeNumber)
        {
            if (strokeNumber < 0 || strokeNumber > 48)
            {
                return false;
            }
            StrokeUnit strokeUnit = strokeDictionary.GetStrokeUnit(strokeNumber);
            return strokeUnit != null;
        }

        public static short GetHomophoneCount(string pinyin)
        {
            if (pinyin == null)
            {
                throw new ArgumentNullException("pinyin");
            }
            if (!IsValidPinyin(pinyin))
            {
                return -1;
            }
            return homophoneDictionary.GetHomophoneUnit(pinyinDictionary, pinyin).Count;
        }

        public static short GetStrokeNumber(char ch)
        {
            if (!IsValidChar(ch))
            {
                return -1;
            }
            CharUnit charUnit = charDictionary.GetCharUnit(ch);
            return charUnit.StrokeNumber;
        }

        public static char[] GetChars(short strokeNumber)
        {
            if (!IsValidStrokeNumber(strokeNumber))
            {
                return null;
            }
            StrokeUnit strokeUnit = strokeDictionary.GetStrokeUnit(strokeNumber);
            return strokeUnit.CharList;
        }

        public static short GetCharCount(short strokeNumber)
        {
            if (!IsValidStrokeNumber(strokeNumber))
            {
                return -1;
            }
            return strokeDictionary.GetStrokeUnit(strokeNumber).CharCount;
        }

        private static bool ExistSameElement<T>(T[] array1, T[] array2) where T : IComparable
        {
            int num = 0;
            int num2 = 0;
            while (num < array1.Length && num2 < array2.Length)
            {
                if (array1[num].CompareTo(array2[num2]) < 0)
                {
                    num++;
                    continue;
                }
                if (array1[num].CompareTo(array2[num2]) > 0)
                {
                    num2++;
                    continue;
                }
                return true;
            }
            return false;
        }
    }
}
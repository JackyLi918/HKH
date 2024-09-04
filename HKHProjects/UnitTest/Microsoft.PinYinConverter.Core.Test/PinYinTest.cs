using Microsoft.International.Converters.PinYinConverter;

namespace Microsoft.PinYinConverter.Core.Test
{
    [TestClass]
    public class PinYinTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string s = "²âÊÔA1";
            string r = "";
            s.Do(c => r += (ChineseChar.IsValidChar(c) ? new ChineseChar(c).Pinyins[0][0] : c));
           
            Assert.AreEqual("CSA1", r.ToUpper());
        }
    }
}
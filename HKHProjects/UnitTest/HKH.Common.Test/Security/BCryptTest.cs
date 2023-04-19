using HKH.Common.Security;

namespace HKH.Common.Security.Test
{
    [TestClass]
    public class BCryptTest
    {
         [TestMethod]
        public void TestHashAnd()
        {
            string test = "I'm testing BCrypt encryption.";

            string encrypt1=BCrypt.HashPassword(test);
            string encrypt2 = BCrypt.HashPassword(test);

            Assert.AreNotEqual(encrypt1, encrypt2);
            Assert.IsTrue(BCrypt.Verify(test,encrypt1));
            Assert.IsTrue(BCrypt.Verify(test, encrypt2));
        }
    }
}

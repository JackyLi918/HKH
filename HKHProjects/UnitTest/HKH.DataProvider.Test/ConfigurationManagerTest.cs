using HKH.Data.Configuration;
using Microsoft.Extensions.Configuration;

namespace HKH.DataProvider.Test
{
    [TestClass]
    public class ConfigurationManagerTest
    {
        [TestInitialize]
        public void Init()
        {
            var configurationBuilder = new ConfigurationBuilder()//			.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();
            DataBaseConfigurationManager.Load(configuration);
        }
        [TestMethod]
        public void TestDefaultConfiguration()
        {
            Assert.AreEqual(DataBaseConfigurationManager.DefaultConfiguration.Name, "sqlConn");
        }

        [TestMethod]
        public void TestGetConfiguration()
        {
            Assert.IsNotNull(DataBaseConfigurationManager.GetConfiguration("sqlConn"));
        }

        [TestMethod]
        public void TestDbEncryption()
        {
            string explainText = "Data Source=localhost;Initial Catalog=northwind;User ID=sa;Password=sa;Integrated Security=false;Connect Timeout=80;EnList=false;";
            DataBaseEncryption dbEncryption = new DataBaseEncryption();
            string encryptedText = dbEncryption.Encrypt(explainText);
            string decryptedText = dbEncryption.Decrypt(encryptedText);
            Assert.AreEqual(explainText, decryptedText);
        }
    }
}

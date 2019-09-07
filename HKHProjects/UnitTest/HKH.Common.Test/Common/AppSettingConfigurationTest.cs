using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKH.Common.Test
{
    [TestClass]
    public class AppSettingConfigurationTest
    {
        [TestMethod]
        public void TestInitialize()
        {
            AppSettingTest setting = new AppSettingTest();

            Assert.AreEqual(setting.TestString, "bbb");
            Assert.AreEqual(setting.TestInt, 1);
            Assert.AreEqual(setting.TestBool, true);

            Assert.AreEqual(setting.TestStringDef, null);
            Assert.AreEqual(setting.TestIntDef, 0);
            Assert.AreEqual(setting.TestBoolDef, false);

            Assert.AreEqual(setting.TestStringDef2, "");
            Assert.AreEqual(setting.TestIntDef2, 100);
            Assert.AreEqual(setting.TestBoolDef2, true);
        }
    }
    public class AppSettingTest : AppSettingConfiguration
    {
        [AppSetting]
        public string TestString { get; private set; }
        [AppSetting]
        public int TestInt { get; private set; }
        [AppSetting]
        public bool TestBool { get; private set; }

        [AppSetting]
        public string TestStringDef { get; private set; }
        [AppSetting]
        public int TestIntDef { get; private set; }
        [AppSetting]
        public bool TestBoolDef { get; private set; }

        [AppSetting(DefaultValue = "")]
        public string TestStringDef2 { get; private set; }
        [AppSetting(DefaultValue = 100)]
        public int TestIntDef2 { get; private set; }
        [AppSetting(DefaultValue = true)]
        public bool TestBoolDef2 { get; private set; }
    }
}

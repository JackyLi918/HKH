using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.Test
{
    [TestClass]
    public class ExchangeConfigurationTest
    {
        [TestMethod]
        public void LoadFromJson()
        {
            var self = ExchangeConfiguration.Load("sample.json");

            Assert.IsNotNull(self);
            Assert.IsNotNull(self["1"]);
        }
    }
}

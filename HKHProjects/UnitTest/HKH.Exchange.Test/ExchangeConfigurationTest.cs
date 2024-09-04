using HKH.Exchange.Configuration;
using HKH.Exchange.Excel;

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

        [TestMethod]
        public void ExportTest()
        {
            var orderExport = new TestExportViewModel()
            {
                StoreName = "北京中心库",
                OutTime = DateTime.Today,
                Comment = "领导已同意",
                Accounting = "财务人员",
                Auditor = "审批人员",
                Author = "经手人员"
            };

            for (int i = 0; i < 10; i++)
            {
                var orderItem = new TestItemExportViewModel()
                {
                    AccountNo = "111111111111",
                    AccountName = "测试人员",
                    Amount = 200000,
                    Comment = "北京中心库废电池款",
                    BankNo = "中国工商银行北京支行"
                };
                orderExport.Items.Add(orderItem);
            }

            string template = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\付款审批清单.xlsx";

            var exportor = new TestExportor();
            var target = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Excel\\" + $"{orderExport.StoreName}{DateTime.Today.ToString("yyyy-MM-dd")}付款审批清单.xlsx";
            exportor.Fill(template, target, orderExport.Items, orderExport);
        }
    }

    public abstract class TestExportBase<TBody> : NPOIExportList<TBody> where TBody : class
    {
        protected TestExportBase(string tableID) : base("sample.json", tableID)
        {
            ExportId = "1";
        }
    }
    public class TestExportor : TestExportBase<TestItemExportViewModel>
    {
        public TestExportor() : base("1")
        {
        }
    }
    public class TestExportViewModel
    {
        public string StoreName { get; set; }
        public DateTime OutTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public string Accounting { get; set; }
        public string Auditor { get; set; }
        public string Author { get; set; }

        public IList<TestItemExportViewModel> Items { get; set; } = new List<TestItemExportViewModel>();
    }

    public class TestItemExportViewModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public string BankNo { get; set; }
    }
}

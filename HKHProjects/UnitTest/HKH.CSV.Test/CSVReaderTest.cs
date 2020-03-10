using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKH.CSV.Test
{
    [TestClass]
    public class CSVReaderTest
    {
        string csv = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\csv.csv";
        string csv1 = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\csv_1.csv";

        [TestMethod]
        public void ReadStringTest()
        {
            using (var reader = new CSVReader(csv))
            {
                var result = reader.ReadAll();

                Assert.AreEqual(result.Count(), 4);
            }
        }

        [TestMethod]
        public void ReadDataTableTest()
        {
            var reader = new CSVReader(csv);

            var result = reader.ReadTable();
            reader.Close();
            Assert.AreEqual(result.Rows.Count, 4);

            reader = new CSVReader(csv);
            result = reader.ReadTable(true);
            Assert.AreEqual(result.Rows.Count, 3);
            reader.Close();
        }

        [TestMethod]
        public void ReadRepeatColumnTest()
        {
            using (var reader = new CSVReader(csv))
            {
                var result = reader.ReadTableSample(1, true);
                Assert.AreEqual(result.Columns[2].ColumnName, "Column3");
            }

            using (var reader = new CSVReader(csv1))
            {
                var result = reader.ReadTableSample(1, true);
                Assert.AreEqual(result.Columns[2].ColumnName, "Alert_1");
            }
        }
    }
}

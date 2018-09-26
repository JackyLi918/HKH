using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HKH.Linq.Data.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKH.Linq.Test
{
    [TestClass]
    public class DbContextTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var nor = new Northwind();
            //var cust = new Customer
            //{
            //    CustomerID = "XX1",
            //    CompanyName = "Company1",
            //    ContactName = "Contact1",
            //    City = "Seattle",
            //    Country = "USA"
            //};
            //var result = nor.Customers.Insert(cust);

            var cust = new Customer
            {
                ID = "XX1",
                City = "Beijing", // moved to Portland!
                Country = "China"
            };

            var result = nor.Customers.PartialUpdate(cust);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var nor = new Northwind();
            var result = nor.Customers.Where(c => c.City == "London").ToList();
        }
        [TestMethod]
        public void TestMethod3()
        {
            var nor = new Northwind();
            var result = nor.Customers.Where(c => c.City == "London").ToList();
            nor.Customers.Update(result[0]);
        }

        [TestMethod]
        public void TestInsert()
        {
            var nor = new Northwind();
            TestTable tt = new TestTable() { Name = "Test" };
            var result = nor.TestTables.Insert(tt,t=>t.ID);
        }
    }

    [Table(Name = "Customers", View = "vCustomers")]
    public class Customer
    {
        [Column(Name = "CustomerID", IsPrimaryKey = true, IsGenerated = false)]
        public string ID;
        public string ContactName;
        public string CompanyName;
        public string Phone;
        public string City;
        public string Country;
        [Column(IsReadOnly = true)]
        public byte[] rVersion;
        [NotMapped]
        public string Test;
    }
    public class TestTable
    {
        [Column(IsPrimaryKey = true, IsGenerated = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int AA { get; set; }
    }
    public class Northwind : HKH.Linq.Data.SqlServer.SqlServerDbContext
    {
        public Northwind()
            : base(HKH.Data.DataProvider.GetInstance(typeof(HKH.Data.SqlServer.SqlServerObjectBuilder), GetConnectionString(), false), new CompositiveMapping())
        {
        }

        private static string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource = @".\SQLEX2017";
            builder.InitialCatalog = "Northwind";
            builder.IntegratedSecurity = true;
            builder.MultipleActiveResultSets = true;
            return builder.ToString();

            //string databaseFile = Path.GetFullPath(@"Northwind.mdf");
            //return string.Format(@"Data Source=.\SQLEX2014;Integrated Security=True;Connect Timeout=30;User Instance=True;MultipleActiveResultSets=true;AttachDbFilename='{0}'", databaseFile);
        }

        public virtual IEntityTable<Customer> Customers
        {
            get { return GetTable<Customer>(); }
        }
        public virtual IEntityTable<TestTable> TestTables
        {
            get { return GetTable<TestTable>(); }
        }
    }
}

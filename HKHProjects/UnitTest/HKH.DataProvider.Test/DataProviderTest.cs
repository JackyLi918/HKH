using System.Data;

namespace HKH.Data.Test
{
    [TestClass]
    public class DataProviderTest
    {
        //[TestMethod]
        //public void TestConnection()
        //{
        //    DataTable dt = HKH.Data.DataProvider.GetInstance().ExecuteDataTable("SELECT * FROM MasterTable");
        //    Assert.AreNotEqual(dt, null);
        //}

        //[TestMethod]
        //public void TestSQLServerGetParameters()
        //{
        //    Guid masterid = new Guid("A8D1D355-3C4E-484B-8AA7-1F5DD0972C45");
        //    IDbDataParameter[] parameters = HKH.Data.DataProvider.GetInstance().Builder.GetParameters("Insert Into SubTable(MasterID,Memo) Values(@Memo,@MasterID);Select @@Identity;", "Memo", masterid);
        //    Assert.AreEqual(parameters.Length, 2);
        //    Assert.AreEqual(parameters[0].ParameterName, "@Memo");
        //    Assert.AreEqual(parameters[0].Value, "Memo");
        //    Assert.AreEqual(parameters[1].ParameterName, "@MasterID");
        //    Assert.AreEqual(parameters[1].Value, masterid);
        //}

        //[TestMethod]
        //public void TestOleGetParameters()
        //{
        //    Guid masterid = new Guid("A8D1D355-3C4E-484B-8AA7-1F5DD0972C45");
        //    IDbDataParameter[] parameters = HKH.Data.DataProvider.GetInstance("oleConn").Builder.GetParameters("Insert Into SubTable(MasterID,Memo) Values(?,?);", masterid, "Memo");
        //    Assert.AreEqual(parameters.Length, 2);
        //    Assert.AreEqual(parameters[0].ParameterName, "@param1");
        //    Assert.AreEqual(parameters[0].Value, masterid);
        //    Assert.AreEqual(parameters[1].ParameterName, "@param2");
        //    Assert.AreEqual(parameters[1].Value, "Memo");
        //}

		//[TestMethod]
		//public void TestDapper()
		//{
		//	IEnumerable<MasterTable> aa = DataProvider.GetInstance().Query<MasterTable>("select * from Master where [name] <> @name", new { name="abc" });
		//}
    }

    class MasterTable
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Creator { get; set; }
    }
}

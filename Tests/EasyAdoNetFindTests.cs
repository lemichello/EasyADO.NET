using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EasyAdoNetFindTests
    {
        [SetUp]
        public void Init()
        {
            _adoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Person")]
        [TestCase("Role")]
        public void When_EasyAdoNet_Expect_FindAll(string tableName)
        {
            var result = _adoNet.Find(tableName);

            Assert.IsTrue(result.HasRows, "Expected and result readers must have the same rows count");
        }

        [TestCase("EmptyTable")]
        public void When_EasyAdoNet_Expect_FindNothing(string tableName)
        {
            var result = _adoNet.Find(tableName);
            
            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExisting")]
        [TestCase("Another")]
        public void When_EasyAdoNet_Expect_NotFindAll(string tableName)
        {
            Assert.Throws<SqlException>(() => { _adoNet.Find(tableName); });
        }

        private EasyAdoNet _adoNet;

        private const string ConnectionString = "Data Source=MAKS\\SQLEXPRESS;Initial Catalog=Test;" +
                                                "Integrated Security=True";
    }
}
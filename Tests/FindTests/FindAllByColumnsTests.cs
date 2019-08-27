using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindTests
{
    [TestFixture]
    public class FindAllByColumnsTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons", new[] {"Name", "Surname"})]
        [TestCase("Roles", new[] {"Name"})]
        public void When_Find_Has_Rows(string tableName, params string[] columns)
        {
            var result = _easyAdoNet.Find(tableName, columns);

            Assert.IsTrue(result.HasRows);
        }

        [TestCase("EmptyTable", new[] {"EmptyName"})]
        public void When_Find_HasNot_Rows(string tableName, params string[] columns)
        {
            var result = _easyAdoNet.Find(tableName, columns);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExistent", new[] {"NotExistent"})]
        [TestCase("Persons")]
        public void When_Find_Throws_ArgumentException(string tableName, params string[] columns)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName, columns); });
        }

        [TestCase(null, new[] {"Name"})]
        [TestCase("Persons", null)]
        public void When_Find_Throws_ArgumentNullException(string tableName, params string[] columns)
        {
            Assert.Throws<ArgumentNullException>(() => { _easyAdoNet.Find(tableName, columns); });
        }

        [TestCase("Persons", new[] {"NotExisting", "AnotherNotExisting"})]
        public void When_Find_Throws_SqlException(string tableName, params string[] columns)
        {
            Assert.Throws<SqlException>(() => { _easyAdoNet.Find(tableName, columns); });
        }

        private EasyAdoNet _easyAdoNet;
    }
}
using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindTests
{
    [TestFixture]
    public class FindColumnsByPredicateTests
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons", "WHERE Name = 'Maksym'", new[] {"Name", "Surname"})]
        [TestCase("Roles", "WHERE Name = 'Admin'", new[] {"Name"})]
        public void When_Find_Has_Rows(string tableName, string predicate, params string[] columns)
        {
            var result = _easyAdoNet.Find(tableName, predicate, columns);

            Assert.IsTrue(result.HasRows);
        }

        [TestCase("Persons", "WHERE Name = 'NotExists'", new[] {"Name", "Surname"})]
        [TestCase("Roles", "WHERE Name = 'NotExists'", new[] {"Name"})]
        public void When_Find_HasNot_Rows(string tableName, string predicate, params string[] columns)
        {
            var result = _easyAdoNet.Find(tableName, predicate, columns);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExistent", "WHERE Name = 'NotExists'", new[] {"NotExists"})]
        [TestCase("Persons", "", new[] {"NotExists"})]
        [TestCase("Persons", "WHERE Name = 'NotExists'")]
        public void When_Find_Throws_ArgumentException(string tableName, string predicate, params string[] columns)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName, predicate, columns); });
        }

        [TestCase(null, "WHERE Name = 'NotExists'", new[] {"NotExists"})]
        [TestCase("Persons", null, new[] {"NotExists"})]
        [TestCase("Persons", "WHERE Name = 'NotExists'", null)]
        public void When_Find_Throws_ArgumentNullException(string tableName, string predicate, params string[] columns)
        {
            Assert.Throws<ArgumentNullException>(() => { _easyAdoNet.Find(tableName, predicate, columns); });
        }

        [TestCase("Persons", "WHERE NotExists = 'NotExists'", new[] {"Name"})]
        [TestCase("Roles", "WHERE Name = 'NotExists'", new[] {"NotExists"})]
        public void When_Find_Throws_SqlException(string tableName, string predicate, params string[] columns)
        {
            Assert.Throws<SqlException>(() => { _easyAdoNet.Find(tableName, predicate, columns); });
        }

        private EasyAdoNet _easyAdoNet;

        private const string ConnectionString = "Data Source=MAKS\\SQLEXPRESS;Initial Catalog=Test;" +
                                                "Integrated Security=True";
    }
}
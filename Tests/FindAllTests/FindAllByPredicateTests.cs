using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindAllTests
{
    [TestFixture]
    public class FindAllByPredicateTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons", "WHERE Name = 'Maksym'")]
        [TestCase("Roles", "WHERE PersonId = 1")]
        public void When_FindAll_Has_Rows(string tableName, string condition)
        {
            var result = _easyAdoNet.FindAll(tableName, condition);

            Assert.IsTrue(result.HasRows);
        }

        [TestCase("Persons", "WHERE Name = 'NotExist'")]
        [TestCase("Roles", "WHERE Name = 'NotExist'")]
        [TestCase("EmptyTable", "WHERE EmptyName = 'NotExist'")]
        public void When_FindAll_HasNot_Rows(string tableName, string condition)
        {
            var result = _easyAdoNet.FindAll(tableName, condition);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("Persons", "")]
        [TestCase("NotExist", "WHERE Name = 'Maksym'")]
        public void When_FindAll_Throws_ArgumentException(string tableName, string condition)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.FindAll(tableName, condition));
        }

        [TestCase(null, "WHERE Name = 'Maksym'")]
        [TestCase("Persons", null)]
        public void When_FindAll_Throws_ArgumentNullException(string tableName, string condition)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.FindAll(tableName, condition));
        }

        [TestCase("Persons", "WHERE NotCorrect")]
        public void When_FindAll_Throws_SqlException(string tableName, string condition)
        {
            Assert.Throws<SqlException>(() => { _easyAdoNet.FindAll(tableName, condition); });
        }

        private EasyAdoNet _easyAdoNet;

        private const string ConnectionString = "data source=(LocalDb)\\MSSQLLocalDB;" +
                                                "initial catalog=EasyAdoNetTest;integrated security=True;" +
                                                "MultipleActiveResultSets=True;App=EntityFramework";
    }
}
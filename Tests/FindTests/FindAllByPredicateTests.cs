using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindTests
{
    [TestFixture]
    public class FindAllByPredicateTests
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons", "Name = 'Maksym'")]
        [TestCase("Roles", "PersonId = 1")]
        public void When_Find_Has_Rows(string tableName, string condition)
        {
            var result = _easyAdoNet.Find(tableName, condition);

            Assert.IsTrue(result.HasRows);
        }

        [TestCase("Persons", "Name = 'NotExist'")]
        [TestCase("Roles", "Name = 'NotExist'")]
        [TestCase("EmptyTable", "EmptyName = 'NotExist'")]
        public void When_Find_HasNot_Rows(string tableName, string condition)
        {
            var result = _easyAdoNet.Find(tableName, condition);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("Persons", "")]
        [TestCase("NotExist", "Name = 'Maksym'")]
        public void When_Find_Throws_ArgumentException(string tableName, string condition)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Find(tableName, condition));
        }

        [TestCase(null, "Name = 'Maksym'")]
        [TestCase("Persons", null)]
        public void When_Find_Throws_ArgumentNullException(string tableName, string condition)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.Find(tableName, condition));
        }

        [TestCase("Persons", "NotCorrect")]
        public void When_Find_Throws_SqlException(string tableName, string condition)
        {
            Assert.Throws<SqlException>(() => { _easyAdoNet.Find(tableName, condition); });
        }

        private EasyAdoNet _easyAdoNet;

        private const string ConnectionString = "Data Source=MAKS\\SQLEXPRESS;Initial Catalog=Test;" +
                                                "Integrated Security=True";
    }
}
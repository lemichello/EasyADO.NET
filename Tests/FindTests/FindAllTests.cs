using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindTests
{
    [TestFixture]
    public class FindAllTests
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons")]
        [TestCase("Roles")]
        public void When_Find_Has_Rows(string tableName)
        {
            var result = _easyAdoNet.Find(tableName);

            Assert.IsTrue(result.HasRows, "Expected and result readers must have the same rows count");
        }

        [TestCase("EmptyTable")]
        public void When_Find_HasNot_Rows(string tableName)
        {
            var result = _easyAdoNet.Find(tableName);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExisting")]
        [TestCase("Another")]
        public void When_Find_Throws_ArgumentException(string tableName)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName); });
        }
        
        private EasyAdoNet _easyAdoNet;

        private const string ConnectionString = "Data Source=MAKS\\SQLEXPRESS;Initial Catalog=Test;" +
                                                "Integrated Security=True";
    }
}
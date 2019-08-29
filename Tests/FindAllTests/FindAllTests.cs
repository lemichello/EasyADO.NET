using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindAllTests
{
    [TestFixture]
    public class FindAllTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Persons")]
        [TestCase("Roles")]
        public void When_FindAll_Has_Rows(string tableName)
        {
            var result = _easyAdoNet.FindAll(tableName);

            Assert.IsTrue(result.HasRows, "Expected and result readers must have the same rows count");
        }

        [TestCase("EmptyTable")]
        public void When_FindAll_HasNot_Rows(string tableName)
        {
            var result = _easyAdoNet.FindAll(tableName);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExisting")]
        [TestCase("Another")]
        public void When_FindAll_Throws_ArgumentException(string tableName)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.FindAll(tableName));
        }

        private EasyAdoNet _easyAdoNet;
    }
}
using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.EntityFramework.Entities;
using Tests.Integration_Tests.Utils;

namespace Tests.Integration_Tests.FindAllTests
{
    [TestFixture]
    public class FindAllTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_FindAllResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.AsNoTracking().ToList();
            var actualReader       = _easyAdoNet.FindAll("Persons");
            var actualCollection   = new List<Person>(expectedCollection.Count);

            while (actualReader.Read())
            {
                actualCollection.Add(actualReader.ToPerson());
            }

            Assert.AreEqual(expectedCollection, actualCollection);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using NUnit.Framework;
using Tests.EntityFramework.Entities;

namespace Tests.Integration_Tests.FindTests
{
    [TestFixture]
    public class FindColumnsByPredicateTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_FindByPredicateResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons
                .Where(p => p.Name == "Maksym")
                .Select(p => new Person {Surname = p.Surname})
                .ToList();
            var actualReader = _easyAdoNet.Find("Persons", "WHERE Name = 'Maksym'",
                new[] {"Surname"});
            var actualCollection = new List<Person>(expectedCollection.Count);

            while (actualReader.Read())
            {
                actualCollection.Add(new Person {Surname = actualReader[0].ToString()});
            }

            Assert.AreEqual(expectedCollection, actualCollection);
        }
    }
}
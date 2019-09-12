using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using NUnit.Framework;
using Tests.EntityFramework.Entities;
using Tests.Integration_Tests.Utils;

namespace Tests.Integration_Tests.FindAllTests
{
    [TestFixture]
    public class FindAllByConditionsTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_FindAllGenericResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.Where(p => p.Name == "Maksym").ToList();
            var actualCollection = _easyAdoNet.FindAll<Person>("Persons", new Dictionary<string, object>
            {
                {"Name", "Maksym"}
            });

            Assert.AreEqual(expectedCollection, actualCollection);
        }

        [Test]
        public void When_FindAllResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.Where(p => p.Name == "Maksym").ToList();
            var actualReader = _easyAdoNet.FindAll("Persons", new Dictionary<string, object>
            {
                {"Name", "Maksym"}
            });
            var actualCollection = new List<Person>(expectedCollection.Count);

            while (actualReader.Read())
            {
                actualCollection.Add(actualReader.ToPerson());
            }

            Assert.AreEqual(expectedCollection, actualCollection);
        }
    }
}
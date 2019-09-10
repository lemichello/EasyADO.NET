using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Integration_Tests.UpdateTests
{
    [TestFixture]
    public class UpdateByPredicateTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_UpdateResult_EqualsTo_ExpectedResult()
        {
            _easyAdoNet.Update("Persons", "WHERE Name = 'Maksym' AND Surname = 'Lemich'",
                new Dictionary<string, object>
                {
                    {"Surname", "Smith"}
                });

            Assert.IsTrue(Context.Persons.First().Surname == "Smith");
        }
    }
}
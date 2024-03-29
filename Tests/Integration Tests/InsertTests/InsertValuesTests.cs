using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Integration_Tests.InsertTests
{
    [TestFixture]
    public class InsertValuesTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_Insert_Inserts_Values()
        {
            var actualId = _easyAdoNet.Insert("Persons", new Dictionary<string, object>
            {
                {"Name", "NewName"},
                {"Surname", "NewSurname"}
            });

            Assert.AreEqual(Context.Persons.Last().Id, actualId);
        }
    }
}
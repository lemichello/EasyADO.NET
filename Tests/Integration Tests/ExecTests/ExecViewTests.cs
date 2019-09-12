using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.EntityFramework.Entities;

namespace Tests.Integration_Tests.ExecTests
{
    [TestFixture]
    public class ExecViewTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            Context.Database.ExecuteSqlCommand("CREATE VIEW [Persons Names] AS SELECT Id, Name, Surname FROM Persons;");
            Context.Database.ExecuteSqlCommand("CREATE VIEW [Empty View] AS SELECT EmptyName FROM EmptyTable;");
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_ExecViewGenericResult_EqualsTo_EmptyResult()
        {
            var actualCollection = _easyAdoNet.ExecView<EmptyTable>("Empty View");

            Assert.IsEmpty(actualCollection);
        }

        [Test]
        public void When_ExecViewGenericResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.FromSql($"SELECT * FROM Persons Names").ToList();
            var actualCollection   = _easyAdoNet.ExecView<Person>("Persons Names");

            Assert.AreEqual(expectedCollection, actualCollection);
        }

        [Test]
        public void When_ExecViewResult_EqualsTo_EmptyResult()
        {
            var actualReader     = _easyAdoNet.ExecView("Empty View");
            var actualCollection = new List<EmptyTable>();

            while (actualReader.Read())
            {
                actualCollection.Add(new EmptyTable
                {
                    EmptyName = actualReader[0].ToString()
                });
            }

            Assert.IsEmpty(actualCollection);
        }

        [Test]
        public void When_ExecViewResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.FromSql($"SELECT * FROM Persons Names").ToList();
            var actualReader       = _easyAdoNet.ExecView("Persons Names");
            var actualCollection   = new List<Person>();

            while (actualReader.Read())
            {
                actualCollection.Add(new Person
                {
                    Id      = (int) actualReader[0],
                    Name    = actualReader[1].ToString(),
                    Surname = actualReader[2].ToString()
                });
            }

            Assert.AreEqual(expectedCollection, actualCollection);
        }
    }
}
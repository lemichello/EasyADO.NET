using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.EntityFramework.Entities;
using Tests.Integration_Tests.Utils;

namespace Tests.Integration_Tests.ExecTests
{
    [TestFixture]
    public class ExecProcedureTests : BaseTestFixture
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [PersonsNames] " +
                "AS SELECT * FROM Persons;");

            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [EmptyProcedure] AS SELECT EmptyName FROM EmptyTable;");
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_ExecProcedureSelectResult_EqualsTo_EmptyResult()
        {
            var actualReader     = _easyAdoNet.ExecProcedure("EmptyProcedure");
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
        public void When_ExecProcedureSelectResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.ToList();
            var actualReader       = _easyAdoNet.ExecProcedure("PersonsNames");
            var actualCollection   = new List<Person>();

            while (actualReader.Read())
            {
                actualCollection.Add(actualReader.ToPerson());
            }

            Assert.AreEqual(expectedCollection, actualCollection);
        }
    }
}
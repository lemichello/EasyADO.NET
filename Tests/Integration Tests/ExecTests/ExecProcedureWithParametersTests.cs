using System.Collections.Generic;
using System.Linq;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.EntityFramework.Entities;

namespace Tests.Integration_Tests.ExecTests
{
    [TestFixture]
    public class ExecProcedureWithParametersTests : BaseTestFixture
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
                "CREATE PROCEDURE [PersonsNames] @Name nvarchar(MAX), @Surname nvarchar(MAX) " +
                "AS SELECT Id, Name, Surname FROM Persons WHERE Name = @Name AND Surname = @Surname;");

            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [InsertPerson] @Name nvarchar(MAX), @Surname nvarchar(MAX) " +
                "AS INSERT INTO Persons(Name, Surname) VALUES (@Name, @Surname);");
        }

        private EasyAdoNet _easyAdoNet;

        [Test]
        public void When_ExecProcedureGenericSelectResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.FromSql("PersonsNames 'Maksym', 'Lemich'").ToList();
            var actualCollection = _easyAdoNet.ExecProcedure<Person>("PersonsNames", new Dictionary<string, object>
            {
                {"Name", "Maksym"},
                {"Surname", "Lemich"}
            });

            Assert.AreEqual(expectedCollection, actualCollection);
        }

        [Test]
        public void When_ExecProcedureInsertResult_EqualsTo_ExpectedResult()
        {
            _easyAdoNet.ExecProcedure("InsertPerson", new Dictionary<string, object>
            {
                {"Name", "NewPersonName"},
                {"Surname", "NewPersonSurname"}
            });

            var lastPerson = Context.Persons.Last();

            Assert.IsTrue(lastPerson.Name == "NewPersonName" && lastPerson.Surname == "NewPersonSurname");
        }

        [Test]
        public void When_ExecProcedureSelectResult_EqualsTo_ExpectedResult()
        {
            var expectedCollection = Context.Persons.FromSql("PersonsNames 'Maksym', 'Lemich'").ToList();
            var actualReader = _easyAdoNet.ExecProcedure("PersonsNames", new Dictionary<string, object>
            {
                {"Name", "Maksym"},
                {"Surname", "Lemich"}
            });
            var actualCollection = new List<Person>();

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
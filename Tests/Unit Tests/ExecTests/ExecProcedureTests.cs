using System;
using System.Data.SqlClient;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Unit_Tests.ExecTests
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
                "AS SELECT Name, Surname FROM Persons;");

            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [EmptyProcedure] AS SELECT EmptyName FROM EmptyTable;");
        }

        [TestCase("PersonsNames")]
        public void When_ExecProcedure_Has_Rows(string procedureName)
        {
            var result = _easyAdoNet.ExecProcedure(procedureName);

            Assert.IsTrue(result.HasRows);
        }

        [TestCase("EmptyProcedure")]
        public void When_ExecProcedure_HasNot_Rows(string procedureName)
        {
            var result = _easyAdoNet.ExecProcedure(procedureName);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase(null)]
        public void When_ExecProcedure_Throws_ArgumentNullException(string procedureName)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.ExecProcedure(procedureName));
        }

        [TestCase("NotExisting")]
        public void When_ExecProcedure_Throws_SqlException(string procedureName)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.ExecProcedure(procedureName));
        }

        private EasyAdoNet _easyAdoNet;
    }
}
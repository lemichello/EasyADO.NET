using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Unit_Tests.ExecTests
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
                "AS SELECT Name, Surname FROM Persons WHERE Name = @Name AND Surname = @Surname;");

            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [InsertPerson] @Name nvarchar(MAX), @Surname nvarchar(MAX) " +
                "AS INSERT INTO Persons(Name, Surname) VALUES (@Name, @Surname);");

            Context.Database.ExecuteSqlCommand(
                "CREATE PROCEDURE [EmptyProcedure] AS SELECT EmptyName FROM EmptyTable;");
        }

        private EasyAdoNet _easyAdoNet;

        private static readonly object[] CorrectParametersForSelect =
        {
            new object[]
            {
                "PersonsNames",
                new Dictionary<string, object>
                {
                    {"Name", "Maksym"},
                    {"Surname", "Lemich"}
                }
            }
        };

        private static readonly object[] CorrectParametersForInsert =
        {
            new object[]
            {
                "InsertPerson",
                new Dictionary<string, object>
                {
                    {"Name", "NewName"},
                    {"Surname", "NewSurname"}
                }
            }
        };

        private static readonly object[] NullParameters =
        {
            new object[]
            {
                null,
                new Dictionary<string, object>
                {
                    {"Name", "Maksym"},
                    {"Surname", "Lemich"}
                }
            },
            new object[]
            {
                "PersonsNames",
                null
            },
            new object[]
            {
                "PersonsNames",
                new Dictionary<string, object>
                {
                    {"Name", "Maksym"},
                    {"Surname", null}
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "PersonsNames",
                new Dictionary<string, object>()
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "NotExists",
                new Dictionary<string, object>
                {
                    {"Name", "Maksym"},
                    {"Surname", "Lemich"}
                }
            },
            new object[]
            {
                "PersonsNames",
                new Dictionary<string, object>
                {
                    {"NotExists", "Maksym"},
                    {"Surname", "Lemich"}
                }
            }
        };

        [Test, TestCaseSource(nameof(CorrectParametersForSelect))]
        public void When_ExecProcedure_Has_Rows(string procedureName, Dictionary<string, object> parameters)
        {
            var result = _easyAdoNet.ExecProcedure(procedureName, parameters);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(CorrectParametersForInsert))]
        public void When_ExecProcedure_Inserts_Values(string procedureName, Dictionary<string, object> parameters)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_ExecProcedure_Throws_ArgumentException(string procedureName,
            Dictionary<string, object> parameters)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_ExecProcedure_Throws_ArgumentNullException(string procedureName,
            Dictionary<string, object> parameters)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_ExecProcedure_Throws_SqlException(string procedureName,
            Dictionary<string, object> parameters)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }
    }
}
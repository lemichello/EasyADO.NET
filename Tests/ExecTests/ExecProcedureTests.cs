using System;
using System.Data.SqlClient;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.ExecTests
{
    [TestFixture]
    public class ExecProcedureTests : BaseTestFixture
    {
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

        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [Test, TestCaseSource(nameof(CorrectParametersForSelect))]
        public void When_ExecProcedure_Has_Rows(string procedureName, params Tuple<string, object>[] parameters)
        {
            var result = _easyAdoNet.ExecProcedure(procedureName, parameters);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(ParametersForEmptySelect))]
        public void When_ExecProcedure_HasNot_Rows(string procedureName, params Tuple<string, object>[] parameters)
        {
            var result = _easyAdoNet.ExecProcedure(procedureName, parameters);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(CorrectParametersForInsert))]
        public void When_ExecProcedure_Inserts_Values(string procedureName, params Tuple<string, object>[] parameters)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_ExecProcedure_Throws_ArgumentNullException(string procedureName,
            params Tuple<string, object>[] parameters)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_ExecProcedure_Throws_SqlException(string procedureName,
            params Tuple<string, object>[] parameters)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.ExecProcedure(procedureName, parameters));
        }

        private EasyAdoNet _easyAdoNet;

        private static readonly object[] CorrectParametersForSelect =
        {
            new object[]
            {
                "PersonsNames",
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            }
        };

        private static readonly object[] CorrectParametersForInsert =
        {
            new object[]
            {
                "InsertPerson",
                new[]
                {
                    new Tuple<string, object>("Name", "NewName"),
                    new Tuple<string, object>("Surname", "NewSurname")
                }
            }
        };

        private static readonly object[] ParametersForEmptySelect =
        {
            new object[]
            {
                "EmptyProcedure",
                new Tuple<string, object>[] { }
            }
        };

        private static readonly object[] NullParameters =
        {
            new object[]
            {
                null,
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
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
                new[]
                {
                    null,
                    new Tuple<string, object>("Surname", "Lemich")
                }
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "NotExists",
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            },
            new object[]
            {
                "PersonsNames",
                new[]
                {
                    new Tuple<string, object>("NotExists", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            }
        };
    }
}
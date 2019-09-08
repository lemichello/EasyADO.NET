using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Unit_Tests.InsertTests
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

        private static readonly object[] CorrectParameters =
        {
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("Name", "NewName"),
                    new Tuple<string, object>("Surname", "NewSurname")
                }
            },
            new object[]
            {
                "Roles",
                new[]
                {
                    new Tuple<string, object>("Name", "NewRole"),
                    new Tuple<string, object>("PersonId", 2)
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "Persons",
                new Tuple<string, object>[] { }
            },
            new object[]
            {
                "NotExisting",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                }
            }
        };

        private static readonly object[] NullParameters =
        {
            new object[]
            {
                null,
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                }
            },
            new object[]
            {
                "Persons",
                null
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("NotExists", "NewName"),
                    new Tuple<string, object>("Surname", "NewSurname")
                }
            },
            new object[]
            {
                "Roles",
                new[]
                {
                    new Tuple<string, object>("Name", "NewRole"),
                    new Tuple<string, object>("NotExists", 2)
                }
            }
        };

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Insert_Inserts_Values(string tableName, params Tuple<string, object>[] values)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Insert_Throws_ArgumentException(string tableName, params Tuple<string, object>[] values)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Insert_Throws_ArgumentNullException(string tableName, params Tuple<string, object>[] values)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Insert_Throws_SqlException(string tableName, params Tuple<string, object>[] values)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Insert(tableName, values));
        }
    }
}
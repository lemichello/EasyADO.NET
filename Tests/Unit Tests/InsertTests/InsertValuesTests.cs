using System;
using System.Collections.Generic;
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
                new Dictionary<string, object>
                {
                    {"Name", "NewName"},
                    {"Surname", "NewSurname"}
                }
            },
            new object[]
            {
                "Roles",
                new Dictionary<string, object>
                {
                    {"Name", "NewRole"},
                    {"PersonId", 2}
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "Persons",
                new Dictionary<string, object>()
            },
            new object[]
            {
                "NotExisting",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
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
                    {"Name", "Admin"}
                }
            },
            new object[]
            {
                "Persons",
                null
            },
            new object[]
            {
                "Persons",
                new Dictionary<string, object>
                {
                    {"Name", null}
                }
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "Persons",
                new Dictionary<string, object>
                {
                    {"NotExists", "NewName"},
                    {"Surname", "NewSurname"}
                }
            },
            new object[]
            {
                "Roles",
                new Dictionary<string, object>
                {
                    {"Name", "NewRole"},
                    {"NotExists", 2}
                }
            }
        };

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Insert_Inserts_Values(string tableName, Dictionary<string, object> values)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Insert_Throws_ArgumentException(string tableName, Dictionary<string, object> values)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Insert_Throws_ArgumentNullException(string tableName, Dictionary<string, object> values)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.Insert(tableName, values));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Insert_Throws_SqlException(string tableName, Dictionary<string, object> values)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Insert(tableName, values));
        }
    }
}
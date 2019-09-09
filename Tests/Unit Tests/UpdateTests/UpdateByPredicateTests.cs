using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Unit_Tests.UpdateTests
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

        private static readonly object[] CorrectParameters =
        {
            new object[]
            {
                "Persons",
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Roles",
                "WHERE Name = 'Admin'",
                new Dictionary<string, object>
                {
                    {"Name", "NotAdmin"}
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "NotExists",
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>()
            }
        };

        private static readonly object[] NullParameters =
        {
            new object[]
            {
                null,
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                null,
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                "WHERE Name = 'Maksym'",
                null
            },
            new object[]
            {
                "Persons",
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", null}
                }
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "Persons",
                "WHERE NotExists = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                "WHERE Name = 'Maksym'",
                new Dictionary<string, object>
                {
                    {"NotExists", "Mark"},
                    {"Surname", "Smith"}
                }
            }
        };

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Update_Throws_ArgumentException(string tableName, string predicate,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Update(tableName, predicate, updatingValues));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Update_Throws_ArgumentNullException(string tableName, string predicate,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.Update(tableName, predicate, updatingValues));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Update_Throws_SqlException(string tableName, string predicate,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Update(tableName, predicate, updatingValues));
        }

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Update_Updates_Record(string tableName, string predicate,
            Dictionary<string, object> updatingValues)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.Update(tableName, predicate, updatingValues));
        }
    }
}
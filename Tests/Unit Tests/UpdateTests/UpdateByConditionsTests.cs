using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Unit_Tests.UpdateTests
{
    [TestFixture]
    public class UpdateByConditionsTests : BaseTestFixture
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
                    {"Name", "Maksym"},
                    {"Surname", "Lemich"}
                },
                new Dictionary<string, object>
                {
                    {"Name", "Mark"}
                }
            },
            new object[]
            {
                "Roles",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
                },
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
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
                },
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                new Dictionary<string, object>(),
                new Dictionary<string, object>
                {
                    {"Name", "Mark"}
                }
            },
            new object[]
            {
                "Persons",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
                },
                new Dictionary<string, object>()
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
                },
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
                new Dictionary<string, object>
                {
                    {"Name", "Admin"},
                },
                null
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "Persons",
                new Dictionary<string, object>
                {
                    {"NotExists", "Admin"}
                },
                new Dictionary<string, object>
                {
                    {"Name", "Mark"},
                    {"Surname", "Smith"}
                }
            },
            new object[]
            {
                "Persons",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
                },
                new Dictionary<string, object>
                {
                    {"NotExists", "Mark"},
                    {"Surname", "Smith"}
                }
            }
        };

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Update_Throws_ArgumentException(string tableName,
            Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Update_Throws_ArgumentNullException(string tableName,
            Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<ArgumentNullException>(
                () => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Update_Throws_SqlException(string tableName,
            Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Update_Updates_Record(string tableName, Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }
    }
}
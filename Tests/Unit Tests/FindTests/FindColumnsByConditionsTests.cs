using System;
using System.Data.SqlClient;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Unit_Tests.FindTests
{
    [TestFixture]
    public class FindColumnsByConditionsTests : BaseTestFixture
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
                    "Name",
                    "Surname"
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            },
            new object[]
            {
                "Roles",
                new[]
                {
                    "PersonId"
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Admin"),
                }
            }
        };

        private static readonly object[] EmptyTableParameters =
        {
            new object[]
            {
                "EmptyTable",
                new[]
                {
                    "EmptyName"
                },
                new[]
                {
                    new Tuple<string, object>("EmptyName", "Maksym"),
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "Persons",
                new[]
                {
                    "Name",
                    "Surname"
                },
                new Tuple<string, object>[] { }
            },
            new object[]
            {
                "Persons",
                new string[] { },
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                }
            },
            new object[]
            {
                "NotExisting",
                new[]
                {
                    "Name",
                    "Surname"
                },
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
                    "Name"
                },
                new[]
                {
                    new Tuple<string, object>("EmptyName", "Maksym"),
                }
            },
            new object[]
            {
                "EmptyTable",
                null,
                new[]
                {
                    new Tuple<string, object>("EmptyName", "Maksym"),
                }
            },
            new object[]
            {
                "EmptyTable",
                new[]
                {
                    "Name"
                },
                null
            },
            new object[]
            {
                "EmptyTable",
                new[]
                {
                    "Name"
                },
                new Tuple<string, object>[]
                {
                    null
                }
            }
        };

        private static readonly object[] IncorrectSqlParameters =
        {
            new object[]
            {
                "Persons",
                new[]
                {
                    "NotExists"
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                }
            },
            new object[]
            {
                "Persons",
                new[]
                {
                    "Name",
                    "Surname"
                },
                new[]
                {
                    new Tuple<string, object>("NotExists", "Maksym"),
                }
            }
        };

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Find_Has_Rows(string tableName, string[] columns, params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, columns, conditions);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(EmptyTableParameters))]
        public void When_Find_HasNot_Rows(string tableName, string[] columns, params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, columns, conditions);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Find_Throws_ArgumentException(string tableName, string[] columns,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Find(tableName, columns, conditions));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Find_Throws_ArgumentNullException(string tableName, string[] columns,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.Find(tableName, columns, conditions));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Find_Throws_SqlException(string tableName, string[] columns,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Find(tableName, columns, conditions));
        }
    }
}
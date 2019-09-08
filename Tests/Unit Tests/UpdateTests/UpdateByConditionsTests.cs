using System;
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
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark")
                }
            },
            new object[]
            {
                "Roles",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                },
                new[]
                {
                    new Tuple<string, object>("Name", "NotAdmin")
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "NotExists",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
                }
            },
            new object[]
            {
                "Persons",
                new Tuple<string, object>[] { },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark")
                }
            },
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                },
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
                    new Tuple<string, object>("Name", "Admin")
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
                }
            },
            new object[]
            {
                "Persons",
                null,
                new[]
                {
                    new Tuple<string, object>("Name", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
                }
            },
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                },
                null
            },
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin"),
                    null
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
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
                    new Tuple<string, object>("NotExists", "Admin")
                },
                new[]
                {
                    new Tuple<string, object>("Name", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
                }
            },
            new object[]
            {
                "Persons",
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym")
                },
                new[]
                {
                    new Tuple<string, object>("NotExists", "Mark"),
                    new Tuple<string, object>("Surname", "Smith")
                }
            }
        };

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Update_Throws_ArgumentException(string tableName, Tuple<string, object>[] equalityConditions,
            params Tuple<string, object>[] updatingValues)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Update_Throws_ArgumentNullException(string tableName,
            Tuple<string, object>[] equalityConditions,
            params Tuple<string, object>[] updatingValues)
        {
            Assert.Throws<ArgumentNullException>(
                () => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(IncorrectSqlParameters))]
        public void When_Update_Throws_SqlException(string tableName,
            Tuple<string, object>[] equalityConditions,
            params Tuple<string, object>[] updatingValues)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Update_Updates_Record(string tableName, Tuple<string, object>[] equalityConditions,
            params Tuple<string, object>[] updatingValues)
        {
            Assert.DoesNotThrow(() => _easyAdoNet.Update(tableName, equalityConditions, updatingValues));
        }
    }
}
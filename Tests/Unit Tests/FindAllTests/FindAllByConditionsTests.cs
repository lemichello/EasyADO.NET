using System;
using System.Collections.Generic;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.Unit_Tests.FindAllTests
{
    [TestFixture]
    public class FindAllByConditionsTests : BaseTestFixture
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
                }
            },
            new object[]
            {
                "Roles",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"},
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "Incorrect",
                new Dictionary<string, object>
                {
                    {"Name", "Maksym"},
                    {"Surname", "Lemich"}
                }
            },
            new object[]
            {
                "NotExisting",
                new Dictionary<string, object>
                {
                    {"Name", "Admin"}
                }
            },
            new object[]
            {
                "NotExisting",
                new Dictionary<string, object>()
            }
        };

        private static readonly object[] EmptyTableParameters =
        {
            new object[]
            {
                "EmptyTable",
                new Dictionary<string, object>
                {
                    {"EmptyName", "Maksym"},
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
                    {"EmptyName", "Maksym"},
                }
            },
            new object[]
            {
                "EmptyTable",
                null
            },
            new object[]
            {
                null,
                new Dictionary<string, object>
                {
                    {"EmptyName", null}
                }
            }
        };

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_FindAll_Has_Rows(string tableName,
            Dictionary<string, object> conditions)
        {
            var result = _easyAdoNet.FindAll(tableName, conditions);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(EmptyTableParameters))]
        public void When_FindAll_HasNot_Rows(string tableName,
            Dictionary<string, object> conditions)
        {
            var result = _easyAdoNet.FindAll(tableName, conditions);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_FindAll_Throws_ArgumentException(string tableName,
            Dictionary<string, object> conditions)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.FindAll(tableName, conditions));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_FindAll_Throws_ArgumentNullException(string tableName,
            Dictionary<string, object> conditions)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.FindAll(tableName, conditions));
        }
    }
}
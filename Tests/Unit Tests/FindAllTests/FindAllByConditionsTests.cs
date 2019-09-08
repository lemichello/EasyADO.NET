using System;
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
                    new Tuple<string, object>("Name", "Admin"),
                }
            }
        };

        private static readonly object[] IncorrectParameters =
        {
            new object[]
            {
                "Incorrect",
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            },
            new object[]
            {
                "NotExisting",
                new[]
                {
                    new Tuple<string, object>("Name", "Admin")
                }
            },
            new object[]
            {
                "NotExisting",
                new Tuple<string, object>[] { }
            }
        };

        private static readonly object[] EmptyTableParameters =
        {
            new object[]
            {
                "EmptyTable",
                new[]
                {
                    new Tuple<string, object>("EmptyName", "Maksym"),
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
                    new Tuple<string, object>("EmptyName", "Maksym"),
                }
            },
            new object[]
            {
                "EmptyTable",
                null
            }
        };

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_FindAll_Has_Rows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.FindAll(tableName, conditions);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(EmptyTableParameters))]
        public void When_FindAll_HasNot_Rows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.FindAll(tableName, conditions);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_FindAll_Throws_ArgumentException(string tableName,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentException>(() => _easyAdoNet.FindAll(tableName, conditions));
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_FindAll_Throws_ArgumentNullException(string tableName,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.FindAll(tableName, conditions));
        }
    }
}
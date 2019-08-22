using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EasyAdoNetFindTests
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("Person")]
        [TestCase("Role")]
        public void When_FindAll_Expect_HasRows(string tableName)
        {
            var result = _easyAdoNet.Find(tableName);

            Assert.IsTrue(result.HasRows, "Expected and result readers must have the same rows count");
        }

        [TestCase("EmptyTable")]
        public void When_FindAll_ExpectNot_HasRows(string tableName)
        {
            var result = _easyAdoNet.Find(tableName);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase("NotExisting")]
        [TestCase("Another")]
        public void When_FindAll_Expect_ThrowArgumentException(string tableName)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName); });
        }

        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_FindByConditions_Expect_HasRows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, conditions);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(EmptyTableParameters))]
        public void When_FindByConditions_ExpectNot_HasRows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, conditions);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_FindByConditions_Expect_ThrowArgumentException(string tableName,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName, conditions); });
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_FindByCondition_Expect_ThrowArgumentNullException(string tableName,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentNullException>(() => { _easyAdoNet.Find(tableName, conditions); });
        }

        private EasyAdoNet _easyAdoNet;

        private const string ConnectionString = "Data Source=MAKS\\SQLEXPRESS;Initial Catalog=Test;" +
                                                "Integrated Security=True";

        #region FindParameters

        private static readonly object[] CorrectParameters =
        {
            new object[]
            {
                "Person",
                new[]
                {
                    new Tuple<string, object>("Name", "Maksym"),
                    new Tuple<string, object>("Surname", "Lemich")
                }
            },
            new object[]
            {
                "Role",
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
        
        #endregion
    }
}
using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.FindTests
{
    [TestFixture]
    public class FindAllByConditionsTests
    {
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }
        
        [Test, TestCaseSource(nameof(CorrectParameters))]
        public void When_Find_Has_Rows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, conditions);

            Assert.IsTrue(result.HasRows);
        }

        [Test, TestCaseSource(nameof(EmptyTableParameters))]
        public void When_Find_HasNot_Rows(string tableName,
            params Tuple<string, object>[] conditions)
        {
            var result = _easyAdoNet.Find(tableName, conditions);

            Assert.IsFalse(result.HasRows);
        }

        [Test, TestCaseSource(nameof(IncorrectParameters))]
        public void When_Find_Throws_ArgumentException(string tableName,
            params Tuple<string, object>[] conditions)
        {
            Assert.Throws<ArgumentException>(() => { _easyAdoNet.Find(tableName, conditions); });
        }

        [Test, TestCaseSource(nameof(NullParameters))]
        public void When_Find_Throws_ArgumentNullException(string tableName,
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
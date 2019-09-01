using System;
using System.Data.SqlClient;
using EasyADO.NET;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.ExecTests
{
    [TestFixture]
    public class ExecViewTests : BaseTestFixture
    {
        [OneTimeSetUp]
        public void OneTimeInit()
        {
            Context.Database.ExecuteSqlCommand("CREATE VIEW [PersonsNames] AS SELECT Name, Surname FROM Persons;");
            Context.Database.ExecuteSqlCommand("CREATE VIEW [EmptyView] AS SELECT EmptyName FROM EmptyTable;");
        }
        
        [SetUp]
        public void Init()
        {
            _easyAdoNet = new EasyAdoNet(ConnectionString);
        }

        [TestCase("PersonsNames")]
        public void When_ExecView_Has_Rows(string viewName)
        {
            var result = _easyAdoNet.ExecView(viewName);
            
            Assert.IsTrue(result.HasRows);
        }

        [TestCase("EmptyView")]
        public void When_ExecView_HaNot_Rows(string viewName)
        {
            var result = _easyAdoNet.ExecView(viewName);

            Assert.IsFalse(result.HasRows);
        }

        [TestCase(null)]
        public void When_ExecView_Throws_ArgumentNullException(string viewName)
        {
            Assert.Throws<ArgumentNullException>(() => _easyAdoNet.ExecView(viewName));
        }

        [TestCase("NotExistingView")]
        [TestCase("")]
        public void When_ExecView_Throws_SqlException(string viewName)
        {
            Assert.Throws<SqlException>(() => _easyAdoNet.ExecView(viewName));
        }

        private EasyAdoNet _easyAdoNet;
    }
}
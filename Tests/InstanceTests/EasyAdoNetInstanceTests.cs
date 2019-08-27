using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.InstanceTests
{
    [TestFixture]
    public class EasyAdoNetInstanceTests : BaseTestFixture
    {
        [TestCase("data source=(LocalDb)\\MSSQLLocalDB;" +
                  "initial catalog=EasyAdoNetTest;integrated security=True;" +
                  "MultipleActiveResultSets=True;App=EntityFramework")]
        public void When_EasyAdoNet_Expect_Initializes(string connectionString)
        {
            Assert.DoesNotThrow(() =>
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }

        [TestCase("data source=(LocalDb)\\MSSQLLocalDB;" +
                  "initial catalog=NotExistsDb;integrated security=True;" +
                  "MultipleActiveResultSets=True;App=EntityFramework")]
        [TestCase("")]
        public void When_EasyAdoNet_Throws_ArgumentException(string connectionString)
        {
            Assert.Throws<ArgumentException>(code: delegate
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }

        [TestCase(null)]
        public void When_EasyAdoNet_Throws_ArgumentNullException(string connectionString)
        {
            Assert.Throws<ArgumentNullException>(delegate
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }
    }
}
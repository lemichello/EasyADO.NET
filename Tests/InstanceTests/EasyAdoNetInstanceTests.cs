using System;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests.InstanceTests
{
    [TestFixture]
    public class EasyAdoNetInstanceTests
    {
        [TestCase(@"Data Source=MAKS\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True")]
        public void When_EasyAdoNet_Expect_Initializes(string connectionString)
        {
            Assert.DoesNotThrow(() =>
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }

        [TestCase(@"Data Source=MAKSS\SQLEXPRESS;Initial Catalog=Tesst;Integrated Security=True")]
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
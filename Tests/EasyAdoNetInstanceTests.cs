using System;
using System.Collections.Generic;
using EasyADO.NET;
using NUnit.Framework;

namespace Tests
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
        public void When_EasyAdoNet_Expect_ThrowsArgumentException(string connectionString)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }

        [TestCase(null)]
        public void When_EasyAdoNet_Expect_ThrowsArgumentNullException(string connectionString)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new EasyAdoNet(connectionString);
            });
        }
    }
}
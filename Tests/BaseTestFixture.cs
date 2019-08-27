using NUnit.Framework;
using Tests.EntityFramework;

namespace Tests
{
    public class BaseTestFixture
    {
        [OneTimeSetUp]
        public void BaseSetUp()
        {
            Context = new EfContext();

            Context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void BaseTearDown()
        {
            Context.Database.EnsureDeleted();
        }

        protected EfContext Context;
    }
}
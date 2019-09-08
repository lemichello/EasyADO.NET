using System.Data.SqlClient;
using Tests.EntityFramework.Entities;

namespace Tests.Integration_Tests.Utils
{
    public static class SqlDataReaderExtensions
    {
        public static Person ToPerson(this SqlDataReader reader)
        {
            return new Person
            {
                Id      = (int) reader[0],
                Name    = reader[1].ToString(),
                Surname = reader[2].ToString()
            };
        }
    }
}
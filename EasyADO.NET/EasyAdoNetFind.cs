using System;
using System.Data.SqlClient;
using System.Linq;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Retrieves all the data from a given table, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="columns"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="columns"/> are null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> has not existing column.</exception>
        public SqlDataReader Find(string tableName, params string[] columns)
        {
            if (columns.Any(i => i == null))
                throw new ArgumentNullException(nameof(columns));

            if (columns.Length == 0)
                throw new ArgumentException("Columns can't be blank", nameof(columns));

            CheckForTableExistent(tableName);

            var connection  = GetAndOpenConnection();
            var commandText = $"SELECT {string.Join(", ", columns)} FROM {tableName}";

            using (var command = new SqlCommand(commandText, connection))
            {
                return command.ExecuteReader();
            }
        }
    }
}
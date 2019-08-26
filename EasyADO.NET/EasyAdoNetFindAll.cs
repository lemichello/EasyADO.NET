using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Retrieves all the data from a given table name.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> is null.</exception>
        public SqlDataReader FindAll(string tableName)
        {
            CheckForTableExistent(tableName);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);

                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table name by conditions.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="conditions">Conditions, by which will be searching. First component - name of the column,
        /// second element - value of the column.
        /// </param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="conditions"/> are null.</exception>
        public SqlDataReader FindAll(string tableName, params Tuple<string, object>[] conditions)
        {
            if (conditions.Any(i => i == null))
                throw new ArgumentNullException(nameof(conditions));

            CheckForTableExistent(tableName);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildCommandQuery(tableName, conditions), connection))
            {
                foreach (var (column, value) in conditions)
                {
                    command.Parameters.AddWithValue($"@{column}", value);
                }

                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table name by predicate.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="predicate">Part of SQL query, which starts with 'WHERE' statement, e.g. 'WHERE ColumnName = Value AND AnotherColumnName = AnotherValue'.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database or <paramref name="predicate"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="predicate"/> is null. </exception>
        /// <exception cref="SqlException">Throws, when <paramref name="predicate"/> has incorrect SQL syntax.</exception>
        public SqlDataReader FindAll(string tableName, string predicate)
        {
            CheckForTableExistent(tableName);

            if (predicate != null && string.IsNullOrWhiteSpace(predicate))
                throw new ArgumentException("Condition string can't be blank", nameof(predicate));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"SELECT * FROM {tableName} {predicate}",
                connection))
            {
                return command.ExecuteReader();
            }
        }

        private static string BuildCommandQuery(string tableName, IEnumerable<Tuple<string, object>> conditions)
        {
            var builder = new StringBuilder($"SELECT * FROM {tableName} WHERE ");

            foreach (var (column, _) in conditions)
            {
                builder.Append($"{column} = @{column} AND ");
            }

            var resultString = builder.ToString();

            return resultString.Remove(resultString.LastIndexOf("AND", StringComparison.Ordinal));
        }
    }
}
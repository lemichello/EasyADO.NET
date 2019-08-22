using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Retrieves all the data from a given table name.
        /// </summary>
        /// <param name="tableName">Name of the table, from which you want to retrieve the data.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when given table name doesn't exist in the database.</exception>
        public SqlDataReader Find(string tableName)
        {
            if (!_tableNames.Contains(tableName))
                throw new ArgumentException("Passed nonexistent name of the table", nameof(tableName));

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
        /// <param name="tableName">Name of the table, from which you want to retrieve the data.</param>
        /// <param name="conditions">Conditions, by which will be searching. First component - name of the column,
        /// second element - value of the column.
        /// </param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when given table name doesn't exist in the database.</exception>
        public SqlDataReader Find(string tableName, params Tuple<string, object>[] conditions)
        {
            CheckParameters(tableName, conditions);
            
            if (!_tableNames.Contains(tableName))
                throw new ArgumentException("Passed nonexistent name of the table", nameof(tableName));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildCommandQuery(tableName, conditions), connection))
            {
                foreach (var (_, value) in conditions)
                {
                    command.Parameters.AddWithValue($"@{value}", value);
                }

                return command.ExecuteReader();
            }
        }

        private void CheckParameters(string tableName, Tuple<string, object>[] conditions)
        {
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));

            if (conditions == null)
                throw new ArgumentNullException(nameof(conditions));

            if (conditions.Length == 0)
                throw new ArgumentException("Conditions cannot be empty", nameof(conditions));
        }


        private static string BuildCommandQuery(string tableName, IEnumerable<Tuple<string, object>> conditions)
        {
            var builder = new StringBuilder($"SELECT * FROM {tableName} WHERE ");

            foreach (var (column, value) in conditions)
            {
                builder.Append($"{column} = @{value} AND ");
            }

            var resultString = builder.ToString();

            return resultString.Remove(resultString.LastIndexOf("AND", StringComparison.Ordinal));
        }
    }
}
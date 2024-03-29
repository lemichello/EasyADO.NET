using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EasyADO.NET.Utils;

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

            using (var command = new SqlCommand($"SELECT * FROM [{tableName}]", connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table name.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> is null.</exception>
        public IEnumerable<T> FindAll<T>(string tableName) where T : class, new()
        {
            var reader = FindAll(tableName);

            return reader.ConvertToClass<T>();
        }

        /// <summary>
        /// Retrieves all the data from a given table name by conditions.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="equalityConditions">Conditions, by which will be searching. First component - name of the column,
        /// second component - searching value of the column.
        /// </param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="equalityConditions"/> are null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="equalityConditions"/> have non-existing column.</exception>
        public SqlDataReader FindAll(string tableName, Dictionary<string, object> equalityConditions)
        {
            CheckConditions(equalityConditions);
            CheckForTableExistent(tableName);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(
                $"SELECT * FROM [{tableName}] WHERE {BuildConditionsQuery(equalityConditions)}",
                connection))
            {
                foreach (var pair in equalityConditions)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table name by conditions.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="equalityConditions">Conditions, by which will be searching. First component - name of the column,
        /// second component - searching value of the column.
        /// </param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="equalityConditions"/> are null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="equalityConditions"/> have non-existing column.</exception>
        public IEnumerable<T> FindAll<T>(string tableName,
            Dictionary<string, object> equalityConditions) where T : class, new()
        {
            var reader = FindAll(tableName, equalityConditions);

            return reader.ConvertToClass<T>();
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
            CheckPredicate(predicate);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"SELECT * FROM [{tableName}] {predicate}",
                connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table name by predicate.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="predicate">Part of SQL query, which starts with 'WHERE' statement, e.g. 'WHERE ColumnName = Value AND AnotherColumnName = AnotherValue'.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="tableName"/> doesn't exist in the database or <paramref name="predicate"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="predicate"/> is null. </exception>
        /// <exception cref="SqlException">Throws, when <paramref name="predicate"/> has incorrect SQL syntax.</exception>
        public IEnumerable<T> FindAll<T>(string tableName, string predicate) where T : class, new()
        {
            var reader = FindAll(tableName, predicate);

            return reader.ConvertToClass<T>();
        }

        private static void CheckPredicate(string predicate)
        {
            if (predicate != null && string.IsNullOrWhiteSpace(predicate))
                throw new ArgumentException("Condition string can't be blank", nameof(predicate));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EasyADO.NET.Utils;

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
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> have non-existing column.</exception>
        public SqlDataReader Find(string tableName, string[] columns)
        {
            CheckColumns(columns);
            CheckForTableExistent(tableName);

            var connection  = GetAndOpenConnection();
            var commandText = $"SELECT [{string.Join("], [", columns)}] FROM [{tableName}]";

            using (var command = new SqlCommand(commandText, connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="columns"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="tableName"/> or <paramref name="columns"/> are null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> have non-existing column.</exception>
        public IEnumerable<T> Find<T>(string tableName, string[] columns) where T : class, new()
        {
            var reader = Find(tableName, columns);

            return reader.ConvertToClass<T>();
        }

        /// <summary>
        /// Retrieves all the data from a given table by predicate, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="predicate">Part of SQL query, which starts with 'WHERE' statement, e.g. 'WHERE ColumnName = Value AND AnotherColumnName = AnotherValue'.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="predicate"/> or <paramref name="columns"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> have non-existing column or <paramref name="predicate"/> has incorrect SQL syntax.</exception>
        public SqlDataReader Find(string tableName, string predicate, string[] columns)
        {
            CheckColumns(columns);
            CheckForTableExistent(tableName);
            CheckPredicate(predicate);

            var connection  = GetAndOpenConnection();
            var commandText = $"SELECT [{string.Join("], [", columns)}] FROM [{tableName}] {predicate}";

            using (var command = new SqlCommand(commandText, connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table by predicate, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="predicate">Part of SQL query, which starts with 'WHERE' statement, e.g. 'WHERE ColumnName = Value AND AnotherColumnName = AnotherValue'.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="predicate"/> or <paramref name="columns"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> have non-existing column or <paramref name="predicate"/> has incorrect SQL syntax.</exception>
        public IEnumerable<T> Find<T>(string tableName, string predicate, string[] columns) where T : class, new()
        {
            var reader = Find(tableName, predicate, columns);

            return reader.ConvertToClass<T>();
        }

        /// <summary>
        /// Retrieves all the data from a given table by conditions, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <param name="equalityConditions">Conditions, by which will be searching. First component - name of the column,
        /// second element - value of the column.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="equalityConditions"/> or <paramref name="columns"/>  are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> or <paramref name="equalityConditions"/> have non-existing column.</exception>
        public SqlDataReader Find(string tableName, string[] columns, Dictionary<string, object> equalityConditions)
        {
            CheckColumns(columns);
            CheckForTableExistent(tableName);
            CheckConditions(equalityConditions);

            var connection = GetAndOpenConnection();
            var commandText = $"SELECT [{string.Join("], [", columns)}] FROM [{tableName}] " +
                              $"WHERE {BuildConditionsQuery(equalityConditions)}";

            using (var command = new SqlCommand(commandText, connection))
            {
                foreach (var pair in equalityConditions)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Retrieves all the data from a given table by conditions, selecting only given columns.
        /// </summary>
        /// <param name="tableName">Name of the table, from which will be retrieving the data.</param>
        /// <param name="columns">Names of the columns.</param>
        /// <param name="equalityConditions">Conditions, by which will be searching. First component - name of the column,
        /// second element - value of the column.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="equalityConditions"/> or <paramref name="columns"/>  are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="columns"/> or <paramref name="equalityConditions"/> have non-existing column.</exception>
        public IEnumerable<T> Find<T>(string tableName, string[] columns,
            Dictionary<string, object> equalityConditions) where T : class, new()
        {
            var reader = Find(tableName, columns, equalityConditions);

            return reader.ConvertToClass<T>();
        }

        private void CheckColumns(string[] columns)
        {
            if (columns.Any(i => i == null))
                throw new ArgumentNullException(nameof(columns));

            if (columns.Length == 0)
                throw new ArgumentException("Columns can't be empty", nameof(columns));
        }
    }
}
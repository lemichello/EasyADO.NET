using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Updates given values in the given table by the predicate.
        /// </summary>
        /// <param name="tableName">Name of the table, in which will be updating.</param>
        /// <param name="predicate">Part of the SQL query, which starts with 'WHERE' statement, e.g. 'WHERE ColumnName = Value AND AnotherColumnName = AnotherValue'.</param>
        /// <param name="updatingValues">Values, which will be replaced in the table. First component - name of the column, second component - value.</param>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="predicate"/> or <paramref name="updatingValues"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="predicate"/> or <paramref name="updatingValues"/> have non-existing column.</exception>
        public void Update(string tableName, string predicate, Dictionary<string, object> updatingValues)
        {
            CheckForTableExistent(tableName);
            CheckUpdatingValues(updatingValues);

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildUpdateQuery(tableName, predicate, updatingValues),
                connection))
            {
                foreach (var pair in updatingValues)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates given values in the given table by given conditions.
        /// </summary>
        /// <param name="tableName">Name of the table, in which will be updating.</param>
        /// <param name="equalityConditions">Conditions, by which will be comparing in 'WHERE' section. First component - name of the column,
        /// second element - value of the column.</param>
        /// <param name="updatingValues">Values, which will be replaced in the table. First component - name of the column, second component - value.</param>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="equalityConditions"/> or <paramref name="updatingValues"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="equalityConditions"/> or <paramref name="updatingValues"/> have non-existing column.</exception>
        public void Update(string tableName, Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            CheckForTableExistent(tableName);
            CheckUpdatingValues(updatingValues);
            CheckConditions(equalityConditions);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildUpdateQuery(tableName, equalityConditions, updatingValues),
                connection))
            {
                foreach (var pair in updatingValues)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                foreach (var pair in equalityConditions)
                {
                    command.Parameters.AddWithValue($"@eq{pair.Key}", pair.Value);
                }

                command.ExecuteNonQuery();
            }
        }

        private void CheckUpdatingValues(Dictionary<string, object> updatingValues)
        {
            try
            {
                CheckConditions(updatingValues);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(updatingValues));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Updating values can't be empty'", nameof(updatingValues));
            }
        }

        private static string BuildUpdateQuery(string tableName, string predicate,
            Dictionary<string, object> updatingValues)
        {
            var builder = new StringBuilder($"UPDATE [{tableName}] SET ");

            foreach (var pair in updatingValues)
            {
                builder.Append($"[{pair.Key}] = @{pair.Key}, ");
            }

            var res = builder.ToString().Substring(0, builder.Length - 2);

            return $"{res} {predicate}";
        }

        private static string BuildUpdateQuery(string tableName, Dictionary<string, object> equalityConditions,
            Dictionary<string, object> updatingValues)
        {
            var builder = new StringBuilder($"UPDATE [{tableName}] SET ");

            foreach (var pair in updatingValues)
            {
                builder.Append($"[{pair.Key}] = @{pair.Key}, ");
            }

            var res = builder.ToString().Substring(0, builder.Length - 2);
            builder = new StringBuilder($"{res} WHERE ");

            foreach (var pair in equalityConditions)
            {
                builder.Append($"[{pair.Key}] = @eq{pair.Key} AND ");
            }

            return builder.ToString().Substring(0, builder.Length - 5);
        }
    }
}
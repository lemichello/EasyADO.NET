using System;
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
        public void Update(string tableName, string predicate, params Tuple<string, object>[] updatingValues)
        {
            CheckForTableExistent(tableName);
            CheckUpdatingValues(updatingValues);

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildUpdateQuery(tableName, predicate, updatingValues),
                connection))
            {
                foreach (var (column, value) in updatingValues)
                {
                    command.Parameters.AddWithValue($"@{column}", value);
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
        public void Update(string tableName, Tuple<string, object>[] equalityConditions,
            params Tuple<string, object>[] updatingValues)
        {
            CheckForTableExistent(tableName);
            CheckUpdatingValues(updatingValues);
            CheckConditions(equalityConditions);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(BuildUpdateQuery(tableName, equalityConditions, updatingValues),
                connection))
            {
                foreach (var (column, value) in updatingValues)
                {
                    command.Parameters.AddWithValue($"@{column}", value);
                }

                foreach (var (column, value) in equalityConditions)
                {
                    command.Parameters.AddWithValue($"@eq{column}", value);
                }

                command.ExecuteNonQuery();
            }
        }

        private void CheckUpdatingValues(Tuple<string, object>[] updatingValues)
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
            params Tuple<string, object>[] updatingValues)
        {
            var builder = new StringBuilder($"UPDATE [{tableName}] SET ");

            foreach (var (column, _) in updatingValues)
            {
                builder.Append($"[{column}] = @{column}, ");
            }

            var res = builder.ToString().Substring(0, builder.Length - 2);

            return $"{res} {predicate}";
        }

        private static string BuildUpdateQuery(string tableName, Tuple<string, object>[] equalityConditions,
            Tuple<string, object>[] updatingValues)
        {
            var builder = new StringBuilder($"UPDATE [{tableName}] SET ");

            foreach (var (column, _) in updatingValues)
            {
                builder.Append($"[{column}] = @{column}, ");
            }

            var res = builder.ToString().Substring(0, builder.Length - 2);
            builder = new StringBuilder($"{res} WHERE ");

            foreach (var (column, _) in equalityConditions)
            {
                builder.Append($"[{column}] = @eq{column} AND ");
            }

            return builder.ToString().Substring(0, builder.Length - 5);
        }
    }
}
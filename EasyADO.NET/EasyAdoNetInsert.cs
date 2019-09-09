using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Inserts given values to the given table. 
        /// </summary>
        /// <param name="tableName">Name of the table, in which will be inserting the data.</param>
        /// <param name="values">Values, which will be inserting to the table. First component of the values - name of the column,
        /// second component - inserting value of the column.</param>
        /// <returns>Inserted ID.</returns>
        /// <exception cref="ArgumentException">Throws, when given <paramref name="tableName"/> doesn't exist in the database or <paramref name="values"/> are empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="values"/> has not existing column.</exception>
        public int Insert(string tableName, Dictionary<string, object> values)
        {
            CheckForTableExistent(tableName);
            CheckValues(values);

            var connection  = GetAndOpenConnection();
            var commandText = BuildInsertQuery(tableName, values);

            using (var command = new SqlCommand(commandText, connection))
            {
                foreach (var pair in values)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                return (int) command.ExecuteScalar();
            }
        }

        private void CheckValues(Dictionary<string, object> values)
        {
            try
            {
                CheckConditions(values);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(values));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Values can't be empty'");
            }
        }

        private static string BuildInsertQuery(string tableName, Dictionary<string, object> values)
        {
            var builder = new StringBuilder($"INSERT INTO [{tableName}] ( ");

            foreach (var pair in values)
            {
                builder.Append($"[{pair.Key}], ");
            }

            var resultStr = builder.ToString().Substring(0, builder.Length - 2);
            builder = new StringBuilder(resultStr + ") OUTPUT inserted.Id VALUES (");

            foreach (var pair in values)
            {
                builder.Append($"@{pair.Key}, ");
            }

            return builder.ToString().Substring(0, builder.Length - 2) + ")";
        }
    }
}
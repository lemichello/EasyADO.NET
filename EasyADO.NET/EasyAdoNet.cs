using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <param name="connectionString">Connection string to the Microsoft SQL Server database.</param>
        /// <exception cref="ArgumentException">Throws, when given connection string is empty or incorrect.</exception>
        /// <exception cref="ArgumentNullException">Throws, when given connection string is null.</exception>
        public EasyAdoNet(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _tableNames       = new List<string>();

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentException("Given empty connection string", nameof(connectionString));

            if (!CheckConnectionString())
                throw new ArgumentException("Given incorrect connection string");

            InitializeDbTablesNames();
        }

        private readonly string       _connectionString;
        private readonly List<string> _tableNames;

        private bool CheckConnectionString()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                }
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }

        private void InitializeDbTablesNames()
        {
            var dbName = Regex.Match(_connectionString, @"^.*;[Ii]nitial [Cc]atalog=([\w\d]+);.*$").Groups[1].Value;
            var reader = GetTableNamesReader(dbName);

            while (reader.Read())
            {
                _tableNames.Add(reader[0].ToString());
            }
        }

        private void CheckForTableExistent(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));

            if (!_tableNames.Contains(tableName))
                throw new ArgumentException("Passed nonexistent name of the table", nameof(tableName));
        }

        private SqlConnection GetAndOpenConnection()
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            return connection;
        }

        private SqlDataReader GetTableNamesReader(string dbName)
        {
            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
                                                          WHERE TABLE_TYPE = 'BASE TABLE' AND 
                                                                TABLE_CATALOG = @dbName", connection))
            {
                command.Parameters.AddWithValue("@dbName", dbName);

                return command.ExecuteReader();
            }
        }

        private static string BuildConditionsQuery(IEnumerable<Tuple<string, object>> conditions)
        {
            var builder = new StringBuilder();

            foreach (var (column, _) in conditions)
            {
                builder.Append($"[{column}] = @{column} AND ");
            }

            var resultString = builder.ToString();

            return resultString.Remove(resultString.LastIndexOf("AND", StringComparison.Ordinal));
        }

        private void CheckConditions(Tuple<string, object>[] conditions)
        {
            if (conditions == null || conditions.Any(i => i == null))
                throw new ArgumentNullException(nameof(conditions));

            if (conditions.Length == 0)
                throw new ArgumentException("Conditions can't be empty", nameof(conditions));
        }
    }
}
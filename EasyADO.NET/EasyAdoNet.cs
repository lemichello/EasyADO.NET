using System;
using System.Data.SqlClient;
using System.Data.Common;

namespace EasyADO.NET
{
    public class EasyAdoNet
    {
        /// <param name="connectionString">Connection string to the Microsoft SQL Server database.</param>
        /// <exception cref="ArgumentException">Throws, when given connection string is empty or incorrect.</exception>
        /// <exception cref="ArgumentNullException">Throws, when given connection string is null.</exception>
        public EasyAdoNet(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentException("Given empty connection string", nameof(connectionString));

            if (!CheckConnectionString())
                throw new ArgumentException("Given incorrect connection string");
        }

        /// <summary>
        /// Retrieves all the data from a given table name.
        /// </summary>
        /// <returns>A SqlDataReader.</returns>
        public SqlDataReader Find(string tableName)
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);

                return command.ExecuteReader();
            }
        }

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

        private readonly string _connectionString;
    }
}
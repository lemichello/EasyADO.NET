using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Executes given stored procedure with given parameters.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="parameters">Parameters for stored procedure. First element - name of the parameter,
        /// second element - value.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentException">Throws, when <paramref name="parameters"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="procedureName"/> or one of the <paramref name="parameters"/> doesn't exist in the database.</exception>
        public SqlDataReader ExecProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            CheckExecProcedureParameters(procedureName, parameters);

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"[{procedureName}]",
                connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                foreach (var pair in parameters)
                {
                    command.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
                }

                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Executes given stored procedure.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="procedureName"/> is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="procedureName"/> doesn't exist in the database.</exception>
        public SqlDataReader ExecProcedure(string procedureName)
        {
            if (procedureName == null)
                throw new ArgumentNullException(nameof(procedureName));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"[{procedureName}]", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                return command.ExecuteReader();
            }
        }

        private void CheckExecProcedureParameters(string procedureName, Dictionary<string, object> parameters)
        {
            if (procedureName == null)
                throw new ArgumentNullException(nameof(procedureName));

            if (parameters == null ||
                parameters.Any(i => i.Key == null || i.Value == null))
                throw new ArgumentNullException(nameof(parameters));

            if (parameters.Count == 0)
                throw new ArgumentException("You can't pass empty stored procedure parameters." +
                                            "If given stored procedure hasn't parameters, call 'ExecProcedure' method " +
                                            "without second parameter", nameof(parameters));
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Executes given procedure with given optional parameters.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">Parameters for stored procedure. First element - name of the parameter,
        /// second element - value.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws, when one of the parameters is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="procedureName"/> or one of the <paramref name="parameters"/> doesn't exist in the database.</exception>
        public SqlDataReader ExecProcedure(string procedureName, params Tuple<string, object>[] parameters)
        {
            if (procedureName == null)
                throw new ArgumentNullException(nameof(procedureName));

            if (parameters == null || parameters.Any(i => i == null))
                throw new ArgumentNullException(nameof(parameters));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand(procedureName,
                connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                foreach (var (name, value) in parameters)
                {
                    command.Parameters.AddWithValue($"@{name}", value);
                }

                return command.ExecuteReader();
            }
        }
    }
}
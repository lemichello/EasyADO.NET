using System;
using System.Data.SqlClient;

namespace EasyADO.NET
{
    public partial class EasyAdoNet
    {
        /// <summary>
        /// Executes given view and returns all the data from the result.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="viewName"/> is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="viewName"/> doesn't exist in the database.'</exception>
        public SqlDataReader ExecView(string viewName)
        {
            if (viewName == null)
                throw new ArgumentNullException(nameof(viewName));

            var connection = GetAndOpenConnection();

            using (var command = new SqlCommand($"SELECT * FROM [{viewName}]", connection))
            {
                return command.ExecuteReader();
            }
        }
    }
}
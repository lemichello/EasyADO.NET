using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EasyADO.NET.Utils;

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

        /// <summary>
        /// Executes given view and returns all the data from the result.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <typeparam name="T">Type of the class, to which will be converted result of query.</typeparam>
        /// <returns>Collection of <typeparamref name="T"/> instances.</returns>
        /// <exception cref="ArgumentNullException">Throws, when <paramref name="viewName"/> is null.</exception>
        /// <exception cref="SqlException">Throws, when <paramref name="viewName"/> doesn't exist in the database.'</exception>
        public IEnumerable<T> ExecView<T>(string viewName) where T : class, new()
        {
            var reader = ExecView(viewName);

            return reader.ConvertToClass<T>();
        }
    }
}
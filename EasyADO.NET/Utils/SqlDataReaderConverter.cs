using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace EasyADO.NET.Utils
{
    internal static class SqlDataReaderConverter
    {
        internal static IEnumerable<T> ConvertToClass<T>(this SqlDataReader reader) where T : class, new()
        {
            var collection = new List<T>();
            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                var instance = new T();

                FillInstanceProperties(reader, properties, instance);

                collection.Add(instance);
            }

            return collection;
        }

        private static void FillInstanceProperties<T>(IDataRecord reader, PropertyInfo[] properties, T instance)
            where T : class, new()
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                SetPropertyValue(reader, properties, instance, i);
            }
        }

        private static void SetPropertyValue<T>(IDataRecord reader, IEnumerable<PropertyInfo> properties, T instance,
            int i)
            where T : class, new()
        {
            foreach (var prop in properties)
            {
                if (prop.Name != reader.GetName(i))
                    continue;

                var value = reader[i];

                if (value == DBNull.Value)
                    value = null;

                prop.SetValue(instance, value);
                return;
            }
        }
    }
}
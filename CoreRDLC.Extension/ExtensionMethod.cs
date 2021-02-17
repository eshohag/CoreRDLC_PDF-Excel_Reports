using System;
using System.Data;
using System.Reflection;

namespace CoreRDLC.Extension
{
    public static class ExtensionMethod
    {
        public static DataTable ToDataTable<TSource>(this TSource data)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                                 prop.PropertyType);
            }

            var values = new object[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(data, null);
            }
            dataTable.Rows.Add(values);

            return dataTable;
        }
    }
}

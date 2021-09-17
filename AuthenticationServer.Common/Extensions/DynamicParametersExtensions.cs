using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace AuthenticationServer.Common.Extentions
{
    public static class DynamicParametersExtensions
    {
        public static DynamicParameters AddColumnParameters<T>(this DynamicParameters parameters, T entity, params string[] columnsToIgnore)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (columnsToIgnore is not null && Array.Exists(columnsToIgnore, column => column == property.Name))
                    continue;

                string prefix = "";
                try
                {
                    parameters.Get<string>(property.Name);
                    prefix = type.GetTableName();
                }
                catch { }
                    

                object[] attributes = property.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    ColumnAttribute columnAttribute = attribute as ColumnAttribute;
                    if (columnAttribute is not null)
                    {
                        string value = "";
                        if (property.PropertyType == typeof(DateTime))
                            value = ((DateTime)property.GetValue(entity)).ToString("yyyy-MM-dd");
                        else
                            value = property.GetValue(entity)?.ToString();

                        parameters.Add($"{prefix}{property.Name}", value);
                    }

                    

                    KeyAttribute keyAttribute = attribute as KeyAttribute;
                    if (keyAttribute is not null)
                        parameters.Add($"{type.GetTableName()}{property.Name}", property.GetValue(entity)?.ToString());
                }
            }

            return parameters;
        }
    }
}

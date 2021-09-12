using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace AuthenticationServer.Common.Extentions
{
    public static class DynamicParametersExtentions
    {
        public static DynamicParameters AddParametersFromProperties<T>(this DynamicParameters parameters, T entity, string prefix = "")
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    ColumnAttribute columnAttribute = attribute as ColumnAttribute;
                    if (columnAttribute != null)
                        parameters.Add($"{prefix}{property.Name}", property.GetValue(entity)?.ToString());
                    

                    KeyAttribute keyAttribute = attribute as KeyAttribute;
                    if (keyAttribute != null)
                        parameters.Add($"{prefix}{property.Name}", property.GetValue(entity)?.ToString());
                }
            }

            return parameters;
        }
    }
}

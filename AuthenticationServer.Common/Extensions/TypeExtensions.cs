using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace AuthenticationServer.Common.Extentions
{
    public static class TypeExtensions
    {

        public static string GetTableName(this Type type)
        {
            string tableName = type.Name;
            var customAttributes = type.GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if (customAttributes.Count() > 0)
                tableName = customAttributes.First().Name;


            return tableName;
        }
    }
}

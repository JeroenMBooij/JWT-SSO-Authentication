using Dapper;
using System;
using System.Reflection;

namespace Authentication.Persistance.Repositories
{
    public abstract class AbstractRepository
    {
        protected DynamicParameters GetEntityParamaters<T>(T entity, DynamicParameters parameters = null)
        {
            if (parameters == null)
                parameters = new DynamicParameters();

            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (!IsSimpleType(property.PropertyType))
                    continue;
                if (parameters.ParameterNames.AsList().Contains($"{property.Name}"))
                    continue;

                var test = property.GetValue(entity);
                parameters.Add(CreateEntityPropertyName(entity, property.Name), property.GetValue(entity));
            }

            return parameters;
        }

        private string CreateEntityPropertyName<T>(T entity, string propertyName)
        {
            string className = typeof(T).Name;
            string suffix = "Entity";
            string classNamePrefix = className.Substring(0, (className.Length - suffix.Length));

            string EntityPropertyName;
            switch (propertyName)
            {
                case "Created":
                case "LastModified":
                case "TenantId":
                case "UserId":
                    EntityPropertyName = $"@{propertyName}";
                    break;
                default:
                    EntityPropertyName = $"@{classNamePrefix}{propertyName}";
                    break;
            }

            return EntityPropertyName;
        }

        private bool IsSimpleType(Type propertyType)
        {
            return propertyType.IsPrimitive
                || propertyType.IsEnum
                || propertyType.Equals(typeof(string))
                || propertyType.Equals(typeof(decimal))
                || propertyType.Equals(typeof(Guid))
                || propertyType.Equals(typeof(DateTime?))
                || propertyType.Equals(typeof(DateTimeOffset?))
                || propertyType.Equals(typeof(DateTime))
                || propertyType.Equals(typeof(DateTimeOffset));
        }

    }
}

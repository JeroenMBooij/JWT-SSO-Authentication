using System.Collections.Generic;
using System.Reflection;

namespace Authentication.Persistance.Repositories
{
    public abstract class AbstractRepository
    {
        protected Dictionary<string, object> GetEntityParamaters<T>(T entity, Dictionary<string, object> tracker = null)
        {
            var parameters = new Dictionary<string, object>();
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (property.GetGetMethod().IsVirtual)
                    continue;
                if (tracker != null && tracker.ContainsKey($"@{property.Name}"))
                    continue;

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
    }
}

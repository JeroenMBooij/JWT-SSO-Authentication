using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Extentions
{
    public class GenericExtentions
    {
        public static T CloneAndMerge<T>(T baseObject, T overrideObject) where T : new()
        {
            var t = typeof(T);
            var publicProperties = t.GetProperties();

            var output = new T();

            foreach (var propInfo in publicProperties)
            {
                var overrideValue = propInfo.GetValue(overrideObject);
                var defaultValue = !propInfo.PropertyType.IsValueType
                    ? null
                    : Activator.CreateInstance(propInfo.PropertyType);
                if (overrideValue == defaultValue)
                {
                    propInfo.SetValue(output, propInfo.GetValue(baseObject));
                }
                else
                {
                    propInfo.SetValue(output, overrideValue);
                }
            }

            return output;
        }
        
    }
}

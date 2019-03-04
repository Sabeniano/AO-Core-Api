using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace AoApi.Helpers
{
    /// <summary>
    /// Extensions for entities
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Shapes object into a dynamic expando object with the given fields
        /// </summary>
        /// <typeparam name="T">The type of object to shape</typeparam>
        /// <param name="source">The Object to shape</param>
        /// <param name="fields">The fields to keep in the new expando object</param>
        /// <returns></returns>
        public static ExpandoObject ShapeData<T>(this T source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasnt found on {typeof(T)}");
                    }


                    propertyInfoList.Add(propertyInfo);

                }
            }


            var dataShapedObject = new ExpandoObject();


            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(source);

                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }
    }
}

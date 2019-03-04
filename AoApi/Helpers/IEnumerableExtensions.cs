using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace AoApi.Helpers
{
    /// <summary>
    /// Extensions for IEnumerable of T
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// shapes each item in the IEnumerable into a dynamic expando object with the given fields only
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the IEnumerable of T</typeparam>
        /// <param name="source">the iEnumerable to shape the items of</param>
        /// <param name="fields">The fields to keep in the new expando object</param>
        /// <returns></returns>
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {

                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {

                var fieldsAfterSplit = fields.Split(",");

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasnt found on {typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);

                }
            }

            foreach (TSource sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}

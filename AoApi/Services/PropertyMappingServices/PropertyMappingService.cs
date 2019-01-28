using System;
using System.Collections.Generic;
using System.Linq;

namespace AoApi.Services.PropertyMappingServices
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public void AddPropertyMapping<TSource, TDestination>(Dictionary<string, IEnumerable<string>> mapping)
        {
            propertyMappings.Add(new PropertyMapping<TSource, TDestination>(mapping));

        }

        public Dictionary<string, IEnumerable<string>> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Can't find property mapping instance for <{typeof(TSource)}>");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(",");

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

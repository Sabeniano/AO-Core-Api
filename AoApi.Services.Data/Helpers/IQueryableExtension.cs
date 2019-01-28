using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AoApi.Services.Data.Helpers
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Applysort<T>(this IQueryable<T> source, string orderBy, IDictionary<string, IEnumerable<string>> mapping)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            string[] orderByAfterSplit = orderBy.Split(",");

            foreach (string orderByClause in orderByAfterSplit.Reverse())
            {
                string trimmedOrderByClause = orderByClause.Trim();

                bool orderDescending = trimmedOrderByClause.EndsWith(" desc");

                int indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");

                string propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mapping.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mapping[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.Reverse())
                {

                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}

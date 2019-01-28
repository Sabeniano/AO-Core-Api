using System.Collections.Generic;

namespace AoApi.Services.PropertyMappingServices
{
    public class PropertyMapping<Tsource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, IEnumerable<string>> _mappingDictionary { get; private set; }

        public PropertyMapping(Dictionary<string, IEnumerable<string>> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}

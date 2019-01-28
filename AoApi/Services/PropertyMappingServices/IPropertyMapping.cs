using System.Collections.Generic;

namespace AoApi.Services.PropertyMappingServices
{
    interface IPropertyMapping
    {
        Dictionary<string, IEnumerable<string>> _mappingDictionary { get; }
    }
}

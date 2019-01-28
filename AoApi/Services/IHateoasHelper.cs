using AoApi.Helpers;
using System;
using System.Collections.Generic;

namespace AoApi.Services
{
    public interface IHateoasHelper
    {
        IEnumerable<LinksObject> CreateLinksForResource(Guid id, string fields, string resourceName);
        IEnumerable<LinksObject> CreateLinksForResources(RequestParameters requestParameters, bool hasNext, bool hasPrevious, string resourceName, object values = null);
        IEnumerable<LinksObject> CreateLinksForChildResources(string resourceName, object values);
    }
}

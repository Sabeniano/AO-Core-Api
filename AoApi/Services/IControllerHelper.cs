using AoApi.Helpers;
using AoApi.Services.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AoApi.Services
{
    public interface IControllerHelper
    {
        ExpandoObject CreateLinkedentityWithmetadataObject<T>(PaginationMetaDataObject paginationMetadata, IEnumerable<T> objs, IEnumerable<LinksObject> links);
        ExpandoObject ShapeAndAddLinkToObject<T>(T obj, string resourceName, string field);
        PaginationMetaDataObject CreatePaginationMetadataObject<T>(PagedList<T> pagedlist, RequestParameters requestParameters, string routeName);
        IEnumerable<IDictionary<string, object>> AddLinksToShapedObjects<T>(IEnumerable<T> objs, string resourceName, string fields);
        ExpandoObject AddLinksToCollection(IEnumerable<IDictionary<string, object>> collection, RequestParameters requestParameters, bool hasNext, bool hasPrevious, string resourceName);
        IEnumerable<LinksObject> CreateLinksForResource(Guid id, string fields, string resourceName);
    }
}

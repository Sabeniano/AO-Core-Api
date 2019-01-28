using AoApi.Helpers;
using AoApi.Services.Data.Helpers;
using System.Collections.Generic;
using System.Dynamic;

namespace AoApi.Services
{
    public interface IControllerHelper
    {
        ExpandoObject CreateLinkedentityWithmetadataObject<T>(PaginationMetaDataObject paginationMetadata, IEnumerable<T> objs, IEnumerable<LinksObject> links);
        ExpandoObject ShapeAndAddLinkToObject<T>(T obj, string resourceName, string field);
        PaginationMetaDataObject CreatePaginationMetadataObject<T>(PagedList<T> pagedlist, RequestParameters requestParameters, string routeName);
    }
}

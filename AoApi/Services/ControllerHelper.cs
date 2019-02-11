using AoApi.Helpers;
using AoApi.Services.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AoApi.Services
{
    public class ControllerHelper : IControllerHelper
    {
        private readonly IPaginationUrlHelper _paginationUrlHelper;
        private readonly IHateoasHelper _hateoasHelper;

        public ControllerHelper(IPaginationUrlHelper paginationUrlHelper, IHateoasHelper hateoasHelper)
        {
            _paginationUrlHelper = paginationUrlHelper;
            _hateoasHelper = hateoasHelper;
        }

        public ExpandoObject CreateLinkedentityWithmetadataObject<T>(
            PaginationMetaDataObject paginationMetadata,
            IEnumerable<T> objs, IEnumerable<LinksObject> links)
        {
            var objWithMetadata = new ExpandoObject();

            ((IDictionary<string, object>)objWithMetadata).Add("metadata", paginationMetadata);
            ((IDictionary<string, object>)objWithMetadata).Add("records", objs);
            ((IDictionary<string, object>)objWithMetadata).Add("links", links);
            return objWithMetadata;
        }

        public ExpandoObject ShapeAndAddLinkToObject<T>(T obj, string resourceName, string fields)
        {
            var shapedObj = obj.ShapeData(fields);
            var shapedObjAsDict = shapedObj as IDictionary<string, object>;
            var linksForObj = _hateoasHelper.CreateLinksForResource((Guid)shapedObjAsDict["Id"], fields, resourceName);
            ((IDictionary<string, object>)shapedObj).Add("links", linksForObj);
            return shapedObj;
        }

        public IEnumerable<IDictionary<string, object>> AddLinksToShapedObjects<T>(IEnumerable<T> objs, string resourceName, string fields)
        {
            var linkedObjs = objs.Select(obj =>
            {
                var objAsDict = obj as IDictionary<string, object>;
                objAsDict.Add("links", _hateoasHelper.CreateLinksForResource((Guid)objAsDict["Id"], fields, resourceName));
                return objAsDict;
            });

            return linkedObjs;
        }

        public ExpandoObject AddLinksToCollection(IEnumerable<IDictionary<string, object>> collection, RequestParameters requestParameters, bool hasNext, bool hasPrevious, string resourceName)
        {
            var linkedCollection = new ExpandoObject();
            var links = _hateoasHelper.CreateLinksForResources(requestParameters, hasNext, hasPrevious, resourceName);
            ((IDictionary<string, object>)linkedCollection).Add("records", collection);
            ((IDictionary<string, object>)linkedCollection).Add("links", links);
            return linkedCollection;
        }

        public ExpandoObject AddLinksToCollection(IEnumerable<IDictionary<string, object>> collection, string resourceName, object value)
        {
            var linkedCollection = new ExpandoObject();
            var links = _hateoasHelper.CreateLinksForChildResources(resourceName, value);
            ((IDictionary<string, object>)linkedCollection).Add("records", collection);
            ((IDictionary<string, object>)linkedCollection).Add("links", links);
            return linkedCollection;
        }

        public IEnumerable<LinksObject> CreateLinksForResource(Guid id, string fields, string resourceName)
        {
            return _hateoasHelper.CreateLinksForResource(id, fields, resourceName);
        }

        public PaginationMetaDataObject CreatePaginationMetadataObject<T>(PagedList<T> pagedlist, RequestParameters requestParameters, string routeName)
        {
            var previousPageLink = pagedlist.HasPrevious ? _paginationUrlHelper.CreateUrlForResource(requestParameters, PageType.PreviousPage, routeName) : null;
            var nextPageLink = pagedlist.HasNext ? _paginationUrlHelper.CreateUrlForResource(requestParameters, PageType.NextPage, routeName) : null;

            var paginationMetaData = new PaginationMetaDataObject()
            {
                TotalCount = pagedlist.TotalCount,
                PageSize = pagedlist.PageSize,
                CurrentPage = pagedlist.CurrentPage,
                TotalPages = pagedlist.TotalPages,
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
            };

            return paginationMetaData;
        }
    }
}

using AoApi.Helpers;
using AoApi.Services.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;

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

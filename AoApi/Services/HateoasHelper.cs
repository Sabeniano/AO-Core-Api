using AoApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AoApi.Services
{
    public class HateoasHelper : IHateoasHelper
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IPaginationUrlHelper _paginationUrlHelper;

        public HateoasHelper(IUrlHelper urlHelper, IPaginationUrlHelper paginationUrlHelper)
        {
            _urlHelper = urlHelper;
            _paginationUrlHelper = paginationUrlHelper;
        }
        public IEnumerable<LinksObject> CreateLinksForResource(Guid id, string fields, string resourceName)
        {
            var links = new List<LinksObject>();
            var obj = new ExpandoObject();

            ((IDictionary<string, object>)obj).Add($"{resourceName}Id", id);
            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinksObject(_urlHelper.Link($"Get{resourceName}", obj), "self", "GET"));
            }
            else
            {
                ((IDictionary<string, object>)obj).Add("fields", fields);
                links.Add(new LinksObject(_urlHelper.Link($"Get{resourceName}", obj), "self", "GET"));
            }
            links.Add(
                new LinksObject(_urlHelper.Link($"Delete{resourceName}", obj), $"delete_{resourceName}", "DELETE")
                );
            links.Add(
                new LinksObject(_urlHelper.Link($"Update{resourceName}", obj), $"update_{resourceName}", "PUT")
                );
            links.Add(
                new LinksObject(_urlHelper.Link($"PartiallyUpdate{resourceName}", obj), $"partially_update_{resourceName}", "PATCH")
                );
            return links;
        }

        public IEnumerable<LinksObject> CreateLinksForResources(RequestParameters requestParameters, bool hasNext, bool hasPrevious, string resourceName, object values = null)
        {
            var links = new List<LinksObject>
            {
                new LinksObject(_paginationUrlHelper.CreateUrlForResource(requestParameters, PageType.Current, $"Get{resourceName}s"), "self", "GET")
            };

            if (hasNext)
            {
                links.Add(new LinksObject(_paginationUrlHelper.CreateUrlForResource(requestParameters, PageType.NextPage, $"Get{resourceName}s"), "self_next_page", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(new LinksObject(_paginationUrlHelper.CreateUrlForResource(requestParameters, PageType.PreviousPage, $"Get{resourceName}s"), "self_previous_page", "GET"));
            }

            links.Add(new LinksObject(_urlHelper.Link($"Get{resourceName}s", values), $"create_{resourceName.ToLower()}", "POST"));

            return links;
        }

        public IEnumerable<LinksObject> CreateLinksForChildResources(string resourceName, object values)
        {
            var links = new List<LinksObject>
            {
                new LinksObject(_urlHelper.Link($"Get{resourceName}s", values), "self", "GET")
            };

            links.Add(new LinksObject(_urlHelper.Link($"Get{resourceName}s", values), $"create_{resourceName.ToLower()}", "POST"));

            return links;
        }
    }
}

using AoApi.Helpers;

namespace AoApi.Services
{
    public interface IPaginationUrlHelper
    {
        string CreateUrlForResource(RequestParameters requestParameter, PageType pageType, string routeName);
    }
}

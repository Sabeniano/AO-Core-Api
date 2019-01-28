namespace AoApi.Services.PropertyMappingServices
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}

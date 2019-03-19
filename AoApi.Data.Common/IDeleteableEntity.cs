using System;

namespace AoApi.Data.Common
{
    /// <summary>
    /// Interface for entity models to inherit soft delete information from
    /// </summary>
    public interface IDeleteableEntity
    {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedOn { get; set; }
    }
}

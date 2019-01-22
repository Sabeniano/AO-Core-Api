using System;

namespace AoApi.Data.Common
{
    public interface IDeleteableEntity
    {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedOn { get; set; }
    }
}

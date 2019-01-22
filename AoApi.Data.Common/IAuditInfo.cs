using System;

namespace AoApi.Data.Common
{
    public interface IAuditInfo
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset? ModifiedOn { get; set; }
    }
}

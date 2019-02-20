using System;

namespace AoApi.Data.Common
{
    /// <summary>
    /// Interface for entity models to inherit audit information from
    /// </summary>
    public interface IAuditInfo
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset? ModifiedOn { get; set; }
    }
}

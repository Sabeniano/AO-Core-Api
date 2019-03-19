using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.Common
{
    /// <summary>
    /// Main class for models to inherit from, to adhere to DRY practices.
    /// </summary>
    /// <typeparam name="TKey">type of ID for the entity</typeparam>
    public abstract class BaseEntityModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

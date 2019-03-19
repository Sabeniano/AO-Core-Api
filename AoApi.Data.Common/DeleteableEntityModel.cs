using System;

namespace AoApi.Data.Common
{
    /// <summary>
    /// Main
    /// </summary>
    /// <typeparam name="Tkey">type of ID for the entity</typeparam>
    public class DeleteableEntityModel<Tkey> : BaseEntityModel<Tkey>, IDeleteableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}

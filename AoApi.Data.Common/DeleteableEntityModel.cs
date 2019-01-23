using System;

namespace AoApi.Data.Common
{
    public class DeleteableEntityModel<Tkey> : BaseEntityModel<Tkey>, IDeleteableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}

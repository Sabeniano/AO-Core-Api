using System;

namespace AoApi.Data.Common
{
    class DeleteableEntityModel<Tkey> : BaseEntityModel<Tkey>, IDeleteableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}

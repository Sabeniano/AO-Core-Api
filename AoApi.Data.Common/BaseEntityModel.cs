﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.Common
{
    public abstract class BaseEntityModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

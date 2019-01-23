using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Jobs")]
    public class Job : DeleteableEntityModel<Guid> // decide if necessary? BaseEntityModel
    {
        public string JobTitle { get; set; }
        public string Description { get; set; }

        // necessary ?
        public Employee Employee { get; set; }
    }
}

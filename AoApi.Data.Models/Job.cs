using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Jobs")]
    public class Job : DeleteableEntityModel<Guid>
    {
        public string JobTitle { get; set; }
        public string Description { get; set; }

        // necessary ?
        public Employee Employee { get; set; }
    }
}

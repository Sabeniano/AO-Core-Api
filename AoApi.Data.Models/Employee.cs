using AoApi.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Employees")]
    public class Employee : DeleteableEntityModel<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }

        // relation til User
        public User User { get; set; }

        // relation to job
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }

        // one-to-many relation
        public ICollection<Schedule> Schedules { get; set; }
    }
}

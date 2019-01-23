using AoApi.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Employees")]
    public class Employee : DeleteableEntityModel<Guid>
    {
        [Required(ErrorMessage = "Employee must have a firstname")]
        [MaxLength(20, ErrorMessage = "Firstname must not be longer than 20 characters")]
        public string FirstName { get; set; }
        [MaxLength(30, ErrorMessage = "Lastname must not be longer than 30 characters")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset Birthday { get; set; }
        [MaxLength(20, ErrorMessage = "City must not be longer than 20 characters")]
        public string City { get; set; }
        [MaxLength(20, ErrorMessage = "Country must not be longer than 20 characters")]
        public string Country { get; set; }
        [MaxLength(20, ErrorMessage = "Street must not be longer than 20 characters")]
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

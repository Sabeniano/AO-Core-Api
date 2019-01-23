using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Users")]
    public class User : DeleteableEntityModel<Guid>
    {
        [Required(ErrorMessage = "User must have a username")]
        [MaxLength(20, ErrorMessage = "Username must not be longer than 20 characters")]
        public string Username { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must not be longer than 20 characters")]
        public string Password { get; set; }
        public string Email { get; set; }

        // employee relation
        [Required(ErrorMessage = "User must have an employee id")]
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        // employee relation
        [Required(ErrorMessage = "User must have an role id")]
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}

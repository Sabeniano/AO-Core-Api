using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Services.Data.DtoModels.UserDtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "User must have a username")]
        [MaxLength(20, ErrorMessage = "Username must not be longer than 20 characters")]
        public string Username { get; set; }
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must not be longer than 20 characters")]
        public string Password { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "User must have an employee id")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "User must have a role id")]
        public Guid RoleId { get; set; }
    }
}

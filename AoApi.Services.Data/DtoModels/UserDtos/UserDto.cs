using System;

namespace AoApi.Services.Data.DtoModels.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public Guid RoleId { get; set; }
        public string RoleTitle { get; set; }
    }
}

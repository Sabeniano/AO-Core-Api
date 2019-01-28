using AoApi.Data.Models;
using System;
using System.Collections.Generic;

namespace AoApi.Data.DtoModels.EmployeeDtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
    }
}

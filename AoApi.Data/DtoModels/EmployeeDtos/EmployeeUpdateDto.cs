using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.DtoModels.EmployeeDtos
{
    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20, ErrorMessage = "First Name must not be longer than 20 characters")]
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
        public Guid JobId { get; set; }
    }
}

using System;

namespace AoApi.Services.Data.DtoModels.EmployeeDtos
{
    public class EmployeeDtoForMultiple
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public string Address { get; set; }
        public string JobTitle { get; set; }
    }
}

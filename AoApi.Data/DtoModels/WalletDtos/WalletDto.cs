using AoApi.Data.Models;
using System;

namespace AoApi.Data.DtoModels.WalletDtos
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public int Wage { get; set; }
        public int Salary { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}

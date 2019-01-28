using AoApi.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.DtoModels.WalletDtos
{
    public class WalletUpdateDto
    {
        public int Wage { get; set; }
        public int Salary { get; set; }
        [Required(ErrorMessage = "Wallet must have payment method")]
        public EnumPaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Wallet must have a employee id")]
        public Guid EmployeeId { get; set; }
    }
}

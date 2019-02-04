using AoApi.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AoApi.Data.Models
{
    [Table("Wallets")]
    public class Wallet : BaseEntityModel<Guid>
    {
        public int Wage { get; set; }
        public int Salary { get; set; }
        [Required(ErrorMessage = "Wallet must have PaymentMethod")]
        public EnumPaymentMethod PaymentMethod { get; set; } // setup monthly/hourly payment method

        // owner(employee) relation
        [Required(ErrorMessage = "Wallet must have an employee id")]
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }

    public enum EnumPaymentMethod
    {
        Monthly,
        Hourly
    }
}

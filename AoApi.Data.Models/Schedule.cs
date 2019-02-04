using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Schedules")]
    public class Schedule : BaseEntityModel<Guid>
    {
        [Required(ErrorMessage = "Schedule must have WorkDate")]
        public DateTimeOffset WorkDate { get; set; }
        [Required(ErrorMessage = "Schedule must have StartHour")]
        public DateTimeOffset StartHour { get; set; }
        [Required(ErrorMessage = "Schedule must have EndHour")]
        public DateTimeOffset EndHour { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        // isDayOff for extra pay on days to get called on work when you're off

        // owner(employee) relation
        [Required(ErrorMessage = "Schedule must have an employee id")]
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}

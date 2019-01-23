using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Schedules")]
    public class Schedule : BaseEntityModel<Guid>
    {
        public DateTime WorkDate { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        // isDayOff for ekstra løn på dage hvor man er tilkaldt?

        // owner(employee) relation
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}

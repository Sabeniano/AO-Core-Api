using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Workhours")]
    public class Workhours : BaseEntityModel<Guid>
    {
        public int TotalHoursThisPaycheck { get; set; }
        public int TotalOvertimeHoursThisPaycheck { get; set; }

        // owner(employee) relation
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

    }
}

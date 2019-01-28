using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Services.Data.DtoModels.WorkhoursDtos
{
    public class WorkhoursUpdateDto
    {
        public int TotalHoursThisPaycheck { get; set; }
        public int TotalOvertimeHoursThisPaycheck { get; set; }

        [Required(ErrorMessage = "Workhour must have an employee id")]
        public Guid EmployeeId { get; set; }
    }
}

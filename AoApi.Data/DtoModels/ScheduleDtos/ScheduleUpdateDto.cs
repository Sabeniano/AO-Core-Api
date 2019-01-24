using System;
using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.DtoModels.ScheduleDtos
{
    public class ScheduleUpdateDto
    {
        [Required(ErrorMessage = "Schedule must have WorkDate")]
        public DateTimeOffset WorkDate { get; set; }
        [Required(ErrorMessage = "Schedule must have StartHour")]
        public DateTimeOffset StartHour { get; set; }
        [Required(ErrorMessage = "Schedule must have EndHour")]
        public DateTimeOffset EndHour { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        [Required(ErrorMessage = "Schedule must have an employee id")]
        public Guid EmployeeId { get; set; }
    }
}

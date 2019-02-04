using System;

namespace AoApi.Services.Data.DtoModels.ScheduleDtos
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset WorkDate { get; set; }
        public DateTimeOffset StartHour { get; set; }
        public DateTimeOffset EndHour { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}

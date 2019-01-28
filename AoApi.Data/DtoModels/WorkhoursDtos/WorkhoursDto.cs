using System;

namespace AoApi.Data.DtoModels.WorkhoursDtos
{
    public class WorkhoursDto
    {
        public Guid Id { get; set; }
        public int TotalHoursThisPaycheck { get; set; }
        public int TotalOvertimeHoursThisPaycheck { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}

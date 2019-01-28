using System;

namespace AoApi.Services.Data.DtoModels.JobDtos
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
    }
}

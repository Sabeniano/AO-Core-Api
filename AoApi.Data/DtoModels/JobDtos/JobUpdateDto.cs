using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.DtoModels.JobDtos
{
    public class JobUpdateDto
    {
        [Required(ErrorMessage = "Job must have a job title")]
        [MaxLength(20, ErrorMessage = "Job title must not be longer than 20 characters")]
        public string JobTitle { get; set; }
        [MaxLength(500, ErrorMessage = "Description must not be longer than 500 characters")]
        public string Description { get; set; }
    }
}

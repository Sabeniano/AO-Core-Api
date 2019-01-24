using AoApi.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AoApi.Data.Models
{
    [Table("Jobs")]
    public class Job : DeleteableEntityModel<Guid> // decide if necessary? BaseEntityModel
    {
        [Required(ErrorMessage = "Job must have a job title")]
        [MaxLength(20, ErrorMessage = "Job title must not be longer than 20 characters")]
        public string JobTitle { get; set; }
        [MaxLength(500, ErrorMessage = "Description must not be longer than 500 characters")]
        public string Description { get; set; }

        // necessary ?
        public Employee Employee { get; set; }
    }
}

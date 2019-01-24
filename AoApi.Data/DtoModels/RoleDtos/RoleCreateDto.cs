using System.ComponentModel.DataAnnotations;

namespace AoApi.Data.DtoModels.RoleDtos
{
    public class RoleCreateDto
    {
        [Required(ErrorMessage = "Role must have a job title")]
        [MaxLength(20, ErrorMessage = "Role title must not be longer than 20 characters")]
        public string RoleTitle { get; set; }
    }
}

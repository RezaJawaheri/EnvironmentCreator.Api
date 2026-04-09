using System.ComponentModel.DataAnnotations;

namespace EnvironmentCreator.Api.Dtos
{
    public class CreateEnvironmentDto
    {
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}
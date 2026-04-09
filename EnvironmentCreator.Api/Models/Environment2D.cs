using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnvironmentCreator.Api.Models
{
    public class Environment2D
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public List<Object2D> Objects { get; set; } = new List<Object2D>();
    }
}
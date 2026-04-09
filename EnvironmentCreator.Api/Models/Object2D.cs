using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EnvironmentCreator.Api.Models
{
    public class Object2D
    {
        public int Id { get; set; }

        [Required]
        public string PrefabId { get; set; } = string.Empty;

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public float ScaleX { get; set; } = 1;
        public float ScaleY { get; set; } = 1;

        public float RotationZ { get; set; }

        public int SortingLayer { get; set; }

        [Required]
        public int Environment2DId { get; set; }

        [ForeignKey("Environment2DId")]
        [JsonIgnore]
        public Environment2D? Environment2D { get; set; }
    }
}
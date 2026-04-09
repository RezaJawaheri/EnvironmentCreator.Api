using System.ComponentModel.DataAnnotations;

namespace EnvironmentCreator.Api.Dtos
{
    public class CreateObject2DDto
    {
        [Required]
        public string PrefabId { get; set; } = string.Empty;

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public float ScaleX { get; set; } = 1;
        public float ScaleY { get; set; } = 1;

        public float RotationZ { get; set; }

        public int SortingLayer { get; set; }
    }
}
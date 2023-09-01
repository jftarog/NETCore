using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class SongCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Singer { get; set; }
        public int Length { get; set; }
        public DateTime Created { get; set; }
    }
}

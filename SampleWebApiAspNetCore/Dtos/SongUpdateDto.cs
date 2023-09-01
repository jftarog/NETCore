
namespace SampleWebApiAspNetCore.Dtos
{
    public class SongUpdateDto
    {
        public string? Name { get; set; }
        public string? Singer { get; set; }
        public int Length { get; set; }
        public DateTime Created { get; set; }
    }
}

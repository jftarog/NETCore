namespace SampleWebApiAspNetCore.Dtos
{
    public class SongDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Singer { get; set; }
        public int Length { get; set; }
        public DateTime Created { get; set; }
    }
}

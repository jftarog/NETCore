using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class SongDbContext : DbContext
    {
        public SongDbContext(DbContextOptions<SongDbContext> options)
            : base(options)
        {
        }

        public DbSet<SongEntity> SongItems { get; set; } = null!;
    }
}

using Microsoft.EntityFrameworkCore;

namespace InMemorySyncTest.Model;

public class InMemoryTestDbContext : DbContext
{
    public InMemoryTestDbContext(DbContextOptions<InMemoryTestDbContext> options) : base(options)
    {
    }

    public DbSet<Mock>? Mock { get; set; }
}
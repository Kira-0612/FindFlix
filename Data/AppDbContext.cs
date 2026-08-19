using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public DbSet<MediaItem> MediaItems { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=FindFlix.db");
    }

}
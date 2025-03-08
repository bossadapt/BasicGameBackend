using Microsoft.EntityFrameworkCore;
public class PlayerContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Play> Plays { get; set; }
    public DbSet<Map> Maps { get; set; }
    public PlayerContext(DbContextOptions options) : base(options)
    {
    }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Play>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Map>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Map>().HasData(
            new Map("pk_pylons",
            authorTime:31.75,
            sPlusTime:32.1,
            sTime:33.5,
            aTime:40,
            bTime:50
            )
        );
    }
}
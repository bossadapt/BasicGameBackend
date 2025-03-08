using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class PlayerContextFactory : IDesignTimeDbContextFactory<PlayerContext>
{
    public PlayerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PlayerContext>();
        optionsBuilder.UseSqlite("Data Source=players.db");
        return new PlayerContext(optionsBuilder.Options);
    }
}

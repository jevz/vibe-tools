namespace VibeTools.Api.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class VibeToolsContext(DbContextOptions<VibeToolsContext> options) : DbContext(options)
{
    public DbSet<Tool> Tools => Set<Tool>();
    public DbSet<Review> Reviews => Set<Review>();
}

using Microsoft.EntityFrameworkCore;
using UpStock.Models;

namespace UpStock.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Asset> Assets => Set<Asset>();
}
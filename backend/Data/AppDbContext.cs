using Microsoft.EntityFrameworkCore;
using UpStock.Models;

namespace UpStock.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para el Soft Delete
        modelBuilder.Entity<Asset>().HasQueryFilter(a => !a.IsDeleted);

        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);

        modelBuilder.Entity<Client>().HasQueryFilter(c => c.IsActive);

        // Opcional: si EF genera el UUID en el cliente >:/
        modelBuilder.Entity<Asset>()
            .Property(a => a.AssetID)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}

// Faltan las tablas rentals, and rentalsitem
using Microsoft.EntityFrameworkCore;
using UpStock.Models;

namespace UpStock.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<RentalItem> RentalItems { get; set; }

    // public DbSet<AssetLog> AssetLogs { get; set; }
    // public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Filtros globales activos
        modelBuilder.Entity<Asset>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Client>().HasQueryFilter(c => c.IsActive);

        // 2. Configuración de IDs Autogenerados
        modelBuilder.Entity<Asset>().Property(a => a.AssetId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Status>().Property(s => s.StatusId).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(u => u.UserID).ValueGeneratedOnAdd();
        modelBuilder.Entity<Client>().Property(c => c.ClientID).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().Property(c => c.CategoryID).ValueGeneratedOnAdd();

    
        base.OnModelCreating(modelBuilder);
    }
}
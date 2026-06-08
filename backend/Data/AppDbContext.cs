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
    
    // public DbSet<RentalItem> RentalItems { get; set; }
    // public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
    // public DbSet<AssetLog> AssetLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Filtros globales activos
        modelBuilder.Entity<Asset>().HasQueryFilter(a => !a.isdeleted);
        // Las de User y Client quedan comentadas hasta que adaptemos sus modelos:
        // modelBuilder.Entity<User>().HasQueryFilter(u => u.isactive);   
        // modelBuilder.Entity<Client>().HasQueryFilter(c => c.isactive); 

        modelBuilder.Entity<Asset>().Property(a => a.assetid).ValueGeneratedOnAdd();
        modelBuilder.Entity<Status>().Property(s => s.statusid).ValueGeneratedOnAdd();

        // Las dem�s quedan comentadas para que no de error hasta que hagamos esos modelos:
        // modelBuilder.Entity<User>().Property(u => u.userid).ValueGeneratedOnAdd();
        // modelBuilder.Entity<Client>().Property(c => c.clientid).ValueGeneratedOnAdd();
        // modelBuilder.Entity<Category>().Property(c => c.categoryid).ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}
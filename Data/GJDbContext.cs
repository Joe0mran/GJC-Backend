using GJC.Models;
using Microsoft.EntityFrameworkCore;

namespace GJC.Data;

public class GJDbContext : DbContext
{
    public GJDbContext(DbContextOptions<GJDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship: Customer 1 -> 1 Address
        // Address.CustomerId is both PK and FK to Customer.CustomerId
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Customer)
            .HasForeignKey<Address>(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique: AdminUser Email
        modelBuilder.Entity<AdminUser>()
            .HasIndex(a => a.Email)
            .IsUnique();

        // Unique (recommended): Customer PhoneNumber
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique();

        // Optional: set max lengths (good practice)
        modelBuilder.Entity<Customer>()
            .Property(c => c.PhoneNumber)
            .HasMaxLength(30);

        modelBuilder.Entity<AdminUser>()
            .Property(a => a.Email)
            .HasMaxLength(256);

        modelBuilder.Entity<Address>()
            .Property(a => a.Country)
            .HasMaxLength(100);

        modelBuilder.Entity<Address>()
            .Property(a => a.City)
            .HasMaxLength(100);
    }

    // Auto CreatedAt / UpdatedAt
    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                    entry.Property("CreatedAt").CurrentValue = now;

                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                    entry.Property("UpdatedAt").CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                    entry.Property("UpdatedAt").CurrentValue = now;
            }
        }
    }
}

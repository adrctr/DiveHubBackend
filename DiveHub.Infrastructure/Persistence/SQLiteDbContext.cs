using DiveHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.Persistence;

public class SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Dive?> Dives { get; set; } = null!;
    public DbSet<Equipment?> Equipments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration de l'entité User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.HasIndex(u => u.Auth0UserId).IsUnique(); // Index unique sur Auth0UserId
            entity.Property(u => u.Auth0UserId).IsRequired();
            entity.Property(u => u.FirstName).IsRequired();
            entity.Property(u => u.LastName).IsRequired();
            entity.Property(u => u.Email).IsRequired();
        });

        // Configuration de l'entité Dive
        modelBuilder.Entity<Dive>(entity =>
        {
            entity.HasKey(d => d.DiveId);
            entity.HasOne<User>()
                .WithMany(u => u.Dives)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration de l'entité Equipment
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId);
        });

        // Configuration de la relation Many-to-Many entre Dive et Equipment
        modelBuilder.Entity<Dive>()
            .HasMany(d => d.Equipments)
            .WithMany(e => e.Dives);
    }
}
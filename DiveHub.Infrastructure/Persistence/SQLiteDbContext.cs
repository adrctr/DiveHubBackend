using DiveHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.Persistence;

public class SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Dive> Dives { get; set; } = null!;
    public DbSet<DivePhoto> DivePhotos { get; set; } = null!;
    public DbSet<DivePoint> DivePoints { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Dive>().HasKey(d => d.DiveId);
        modelBuilder.Entity<DivePhoto>().HasKey(dp => dp.DivePhotoId);
        modelBuilder.Entity<DivePoint>().HasKey(dp => dp.DivePointId);

        modelBuilder.Entity<Dive>()
            .HasOne<User>()
            .WithMany(u => u.Dives)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DivePhoto>()
            .HasOne<Dive>()
            .WithMany(d => d.DivePhotos)
            .HasForeignKey(dp => dp.DiveId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DivePoint>()
            .HasOne<Dive>()
            .WithMany(d => d.DivePoints)
            .HasForeignKey(dp => dp.DiveId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
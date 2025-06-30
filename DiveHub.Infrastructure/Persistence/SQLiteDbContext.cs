using DiveHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.Persistence;

public class SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Dive?> Dives { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Dive>().HasKey(d => d.DiveId);


        modelBuilder.Entity<Dive>()
            .HasOne<User>()
            .WithMany(u => u.Dives)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
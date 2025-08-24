using DiveHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.Persistence;

public class DiveHubDbContext(DbContextOptions<DiveHubDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Dive?> Dives { get; set; } = null!;
    public DbSet<Equipment?> Equipments { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Dive>().HasKey(d => d.DiveId);
        modelBuilder.Entity<Equipment>().HasKey(d => d.EquipmentId);
    }
}
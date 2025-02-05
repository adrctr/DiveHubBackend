using DiveHub.Core.Entities;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiveHub.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Ajoute le DbContext et assure la création de la base de données au démarrage.
    /// </summary>
    public static void AddDatabaseInitialization(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        // Ajoute le DbContext
        services.AddDbContext<SQLiteDbContext>(options =>
            options.UseSqlite(connectionString));

        // Ajoute un service d'initialisation de la base de données
        services.AddScoped<DatabaseInitializer>();
    }
}
public class DatabaseInitializer(SQLiteDbContext context)
{
    public void Initialize()
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated(); // Crée la base de données et les tables si elles n'existent pas
        context.Users.Add(new User()
        {
            UserId = 1,
            Email = "adr.couturier@gmail.com",
            CreatedAt = DateTime.Now,
            FirstName = "Adrien",
            LastName = "Couturier",
        });
        context.SaveChanges();
    }
}

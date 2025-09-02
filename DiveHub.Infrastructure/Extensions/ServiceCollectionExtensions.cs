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
        // Ajoute le DbContext SqlLite
        //services.AddDbContext<DiveHubDbContext>(options =>
        //    options.UseSqlite(connectionString));

        // Ajout des services nécessaires
        services.AddDbContext<DiveHubDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Ajoute un service d'initialisation de la base de données
        services.AddScoped<DatabaseInitializer>();
    }
}

public class DatabaseInitializer(DiveHubDbContext context)
{
    public void Initialize()
    {
        // Applique les migrations → crée les tables si elles n'existent pas
        context.Database.Migrate();

        if (!context.Equipments.Any())
        {
            var equipment1 = new Equipment { EquipmentName = "Bouteille 12L" };
            var equipment4 = new Equipment { EquipmentName = "Projecteur" };
            var equipment2 = new Equipment { EquipmentName = "Gilet stabilisateur" };
            var equipment3 = new Equipment { EquipmentName = "Go Pro" };

            context.Equipments.AddRange(equipment1, equipment2, equipment3);
            context.SaveChanges();
        }
    }
}
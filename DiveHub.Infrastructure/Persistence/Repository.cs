using System.Linq.Expressions;
using DiveHub.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.Persistence;

/// <summary>
/// // Implémentation générique d'un dépôt (Repository) pour la gestion des entités.
/// </summary>
/// <param name="context">SQLiteDbContext</param>
/// <typeparam name="T">Entity</typeparam>
public class Repository<T>(SQLiteDbContext context) : IRepository<T>
    where T : class
{
    /// <summary>
    /// Ajoute une nouvelle entité à la base de données.
    /// </summary>
    /// <param name="entity">L'entité à ajouter.</param>
    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Récupère une entité par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité.</param>
    /// <returns>L'entité correspondante ou null si elle n'existe pas.</returns>
    public async Task<T?> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

    /// <summary>
    /// Récupère toutes les entités de ce type dans la base de données.
    /// </summary>
    /// <returns>Une liste de toutes les entités.</returns>
    public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

    /// <summary>
    /// Met à jour une entité existante dans la base de données.
    /// </summary>
    /// <param name="entity">L'entité à mettre à jour.</param>
    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Supprime une entité par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité à supprimer.</param>
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
    
    /// <summary>
    /// Récupère les entités qui satisfont un prédicat donné.
    /// </summary>
    /// <param name="predicate">Une expression lambda qui définit les critères de recherche.</param>
    /// <returns>Une liste d'entités correspondant aux critères.</returns>
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }
}

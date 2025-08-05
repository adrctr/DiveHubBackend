using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.repositories;

public interface IDiveRepository : IRepository<Dive>
{
    Task<IEnumerable<Dive>> GetDivesWihDetails(string auth0UserId);
    Task<Dive?> GetDiveByIdAsync(int diveId);
}

public class DiveRepository(SQLiteDbContext context, IUserRepository userRepository) : GenericRepository<Dive>(context), IDiveRepository
{
    private readonly SQLiteDbContext _dbcontext = context;

    /// <summary>
    /// Retourne une liste de dives avec ses détails 
    /// </summary>
    /// <param name="auth0UserId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Dive>> GetDivesWihDetails(string auth0UserId)
    {
        // Vérifier si l'utilisateur existe déjà
        var existingUser = await userRepository.GetByAuth0UserIdAsync(auth0UserId);
        return await _dbcontext.Dives.Include(d => d.Equipments).Where(d => d.UserId == existingUser.UserId)
            .ToListAsync();
    }

    public async Task<Dive?> GetDiveByIdAsync(int diveId)
    {
        return await _dbcontext.Dives.Include(d => d.Equipments)
            .FirstOrDefaultAsync(d => d.DiveId == diveId);
    }
}
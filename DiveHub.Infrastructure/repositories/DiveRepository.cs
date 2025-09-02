using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace DiveHub.Infrastructure.repositories;

public interface IDiveRepository : IRepository<Dive>
{
    Task<IEnumerable<Dive>> GetDivesWihDetails(int userId);
    Task<Dive?> GetDiveByIdAsync(int diveId);
}

public class DiveRepository(DiveHubDbContext context) : GenericRepository<Dive>(context), IDiveRepository
{
    private readonly DiveHubDbContext _dbcontext = context;

    /// <summary>
    /// Retourne une liste de dives avec ses détails
    /// </summary>
    /// <param name="auth0UserId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Dive>> GetDivesWihDetails(int userId)
    {

        return await _dbcontext.Dives.Include(d => d.Equipments).Where(d => d.UserId == userId)
            .ToListAsync();
    }

    public async Task<Dive?> GetDiveByIdAsync(int diveId)
    {
        return await _dbcontext.Dives.Include(d => d.Equipments)
            .FirstOrDefaultAsync(d => d.DiveId == diveId);
    }
}
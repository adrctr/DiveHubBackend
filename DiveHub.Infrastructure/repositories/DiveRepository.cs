using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.repositories;

public interface IDiveRepository : IRepository<Dive>
{
    Task<IEnumerable<Dive>> GetDivesWihDetails();
    Task<Dive?> GetDiveByIdAsync(int diveId);
}

public class DiveRepository(SQLiteDbContext context) : GenericRepository<Dive>(context), IDiveRepository
{
    private readonly SQLiteDbContext _dbcontext = context;

    /// <summary>
    /// Retourne une liste de dives avec ses détails 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Dive>> GetDivesWihDetails()
    {
        return (await _dbcontext.Dives.Include(d => d.Equipments)
            .ToListAsync());
    }

    public async Task<Dive?> GetDiveByIdAsync(int diveId)
    {
        return await _dbcontext.Dives.Include(d => d.Equipments)
            .FirstOrDefaultAsync(d => d.DiveId == diveId);
    }
}
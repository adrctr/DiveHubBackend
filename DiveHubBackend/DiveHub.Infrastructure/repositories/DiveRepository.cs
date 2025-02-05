using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.repositories;

public interface IDiveRepository : IRepository<Dive>
{
    Task<IEnumerable<Dive>> GetDivesWihDetails();
}

public class DiveRepository(SQLiteDbContext context) : GenericRepository<Dive>(context), IDiveRepository
{
    /// <summary>
    /// Retourne une liste de dives avec ses détails 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Dive>> GetDivesWihDetails()
    {
        return await context.Dives
            .Include(d => d.DivePhotos)
            .Include(d => d.DivePoints)
            .ToListAsync();
    }
}
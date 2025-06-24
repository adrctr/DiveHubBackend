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
    private readonly SQLiteDbContext _context = context;

    /// <summary>
    /// Retourne une liste de dives avec ses détails 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Dive>> GetDivesWihDetails()
    {
        return await _context.Dives.Include(d => d.Equipments)
                   .ToListAsync();
    }
}
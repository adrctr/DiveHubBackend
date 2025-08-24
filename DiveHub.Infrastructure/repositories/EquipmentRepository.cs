using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.repositories;

public interface IEquipmentRepository : IRepository<Equipment>
{
    Task<List<Equipment>> GetEquipmentsByIdsAsync(List<int> equipmentIds);
}

public class EquipmentRepository(DiveHubDbContext context) : GenericRepository<Equipment>(context), IEquipmentRepository
{
    private readonly DiveHubDbContext _dbContext = context;

    public async Task<List<Equipment>> GetEquipmentsByIdsAsync(List<int> equipmentIds)
    {
        return await _dbContext.Set<Equipment>()
            .Where(e => equipmentIds.Contains(e.EquipmentId))
            .ToListAsync();
    }

}

using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IDivePointService
{
    Task<List<DivePoint>> GetAllDivePointsAsync();
    Task<DivePoint?> GetDivePointByIdAsync(int id);
    
    Task<List<DivePoint>> GetDivePointsByDiveIdAsync(int diveId);
    Task AddDivePointAsync(DivePoint divePoint);
    Task UpdateDivePointAsync(DivePoint divePoint);
    Task DeleteDivePointAsync(int id);
}
using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task<List<Dive>> GetAllDivesAsync();
    Task<Dive?> GetDiveByIdAsync(int id);
    Task<List<Dive>> GetDiveByUserIdAsync(int userid);

    Task AddDiveAsync(Dive dive);
    Task UpdateDiveAsync(Dive dive);
    Task DeleteDiveAsync(int id);

    Task AddDivePointAsync(int diveId, DivePoint divePoint);
    Task<DivePoint?> GetDivePointsAsync(int diveId);
}
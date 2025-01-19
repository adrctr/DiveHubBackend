using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDivePointService
{
    Task CreateDivePointAsync(DivePointDto divePointDto, int userId);
    Task<DivePointDto?> GetDivePointByIdAsync(int divePointId);
    Task<IEnumerable<DivePointDto>> GetAllDivePointsAsync();
    Task UpdateDivePointAsync(DivePointDto divePointDto);
    Task DeleteDivePointAsync(int divePointId);
}
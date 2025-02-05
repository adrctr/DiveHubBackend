using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDivePointService
{
    Task AddDivePointAsync(DivePointDto divePointDto, int userId);
    Task AddManyDivePointAsync(IEnumerable<DivePointDto> divePointDtos);
    Task<DivePointDto?> GetDivePointByIdAsync(int divePointId);
    Task<IEnumerable<DivePointDto>> GetAllDivePointsAsync();
    Task UpdateDivePointAsync(DivePointDto divePointDto);
    Task DeleteDivePointAsync(int divePointId);
}
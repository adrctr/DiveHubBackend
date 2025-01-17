using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DivePointService(IStorageService<DivePoint> divePointStorageService) : IDivePointService
{
    /// <summary>
    /// Récupérer tous les points GPS
    /// </summary>
    /// <returns></returns>
    public async Task<List<DivePoint>> GetAllDivePointsAsync()
    {
        return await divePointStorageService.GetAllAsync();
    }

    /// <summary>
    /// Récupérer un point GPS par ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DivePoint?> GetDivePointByIdAsync(int id)
    {
        return await divePointStorageService.GetByIdAsync(id);
    }
    /// <summary>
    /// Récupérer des point GPS par DiveId
    /// </summary>
    /// <param name="diveId"></param>
    /// <returns></returns>
    public async Task<List<DivePoint>> GetDivePointsByDiveIdAsync(int diveId)
    {
        var divePoints = await GetAllDivePointsAsync();
        return divePoints.Where(dp => dp.DiveId == diveId).ToList();
    }

    /// <summary>
    /// Ajouter un point GPS
    /// </summary>
    /// <param name="divePoint"></param>
    public async Task AddDivePointAsync(DivePoint divePoint)
    {
        await divePointStorageService.AddAsync(divePoint);
    }

    /// <summary>
    /// Mettre à jour un point GPS
    /// </summary>
    /// <param name="divePoint"></param>
    public async Task UpdateDivePointAsync(DivePoint divePoint)
    {
        await divePointStorageService.UpdateAsync(divePoint);
    }

    /// <summary>
    /// Supprimer un point GPS
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteDivePointAsync(int id)
    {
        await divePointStorageService.DeleteAsync(id);
    }
}
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivePhotoController(IDivePhotoService divePhotoService, IWebHostEnvironment environment) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPhotos([FromForm] string diveId, [FromForm] List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("Aucun fichier sélectionné.");

        var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "dive_photos");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        List<DivePhotoSaveDto> divePhotos = new();

        foreach (var file in files)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var divePhoto = new DivePhotoSaveDto
            {
                DiveId = int.Parse(diveId), // Correction ici
                FileName = fileName,
                Url = Path.Combine("uploads", "dive_photos", fileName).Replace("\\", "/"),
                CreatedAt = DateTime.UtcNow
            };

            divePhotos.Add(divePhoto);
        }

        await divePhotoService.AddManyDivePhotoAsync(divePhotos); // Ajout de await
        return Ok(new { message = "Photos enregistrées avec succès!", photoUrls = divePhotos.Select(p => p.Url).ToList() });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDivePhotoById(int id)
    {
        var divePhoto = await divePhotoService.GetDivePhotoByIdAsync(id);
        return divePhoto != null ? Ok(divePhoto) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDivePhotos()
    {
        var divePhotos = await divePhotoService.GetAllDivePhotosAsync();
        return Ok(divePhotos);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDivePhoto([FromBody] DivePhotoDto divePhotoDto)
    {
        await divePhotoService.UpdateDivePhotoAsync(divePhotoDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDivePhoto(int id)
    {
        await divePhotoService.DeleteDivePhotoAsync(id);
        return NoContent();
    }
}
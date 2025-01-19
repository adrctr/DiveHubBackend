using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivePhotoController(IDivePhotoService divePhotoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDivePhoto([FromBody] DivePhotoDto divePhotoDto)
    {
        await divePhotoService.CreateDivePhotoAsync(divePhotoDto, 1); //TODO : Change ressoruce iD
        return Ok();
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
using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivePhotoController(DivePhotoService divePhotoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DivePhoto>>> GetAll()
    {
        return Ok(await divePhotoService.GetAllDivePhotosAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DivePhoto>> GetById(int id)
    {
        var divePhoto = await divePhotoService.GetDivePhotoByIdAsync(id);
        if (divePhoto == null) return NotFound();
        return Ok(divePhoto);
    }

    [HttpPost]
    public async Task<ActionResult> Add(DivePhoto divePhoto)
    {
        await divePhotoService.AddDivePhotoAsync(divePhoto);
        return CreatedAtAction(nameof(GetById), new { id = divePhoto.DivePhotoId }, divePhoto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, DivePhoto divePhoto)
    {
        if (id != divePhoto.DivePhotoId) return BadRequest();
        await divePhotoService.UpdateDivePhotoAsync(divePhoto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await divePhotoService.DeleteDivePhotoAsync(id);
        return NoContent();
    }
}
using DiveHub.Application.Services;
using DiveHub.Core.Entities;

namespace DiveHub.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DiveController(DiveService diveService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Dive>>> GetAll()
    {
        return Ok(await diveService.GetAllDivesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Dive>> GetById(int id)
    {
        var dive = await diveService.GetDiveByIdAsync(id);
        if (dive == null) return NotFound();
        return Ok(dive);
    }

    [HttpPost]
    public async Task<ActionResult> Add(Dive dive)
    {
        await diveService.AddDiveAsync(dive);
        return CreatedAtAction(nameof(GetById), new { id = dive.DiveId }, dive);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, Dive dive)
    {
        if (id != dive.DiveId) return BadRequest();
        await diveService.UpdateDiveAsync(dive);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await diveService.DeleteDiveAsync(id);
        return NoContent();
    }
}
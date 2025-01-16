using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivePointController(DivePointService divePointService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DivePoint>>> GetAll()
    {
        return Ok(await divePointService.GetAllDivesPointAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DivePoint>> GetById(int id)
    {
        var divePoint = await divePointService.GetDivePointByIdAsync(id);
        if (divePoint == null) return NotFound();
        return Ok(divePoint);
    }

    [HttpPost]
    public async Task<ActionResult> Add(DivePoint divePoint)
    {
        await divePointService.AddDivePointAsync(divePoint);
        return CreatedAtAction(nameof(GetById), new { id = divePoint.DivePointId }, divePoint);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, DivePoint divePoint)
    {
        if (id != divePoint.DivePointId) return BadRequest();
        await divePointService.UpdateDivePointAsync(divePoint);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await divePointService.DeleteDivePointAsync(id);
        return NoContent();
    }
}
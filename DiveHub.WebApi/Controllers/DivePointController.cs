using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DivePointsController : ControllerBase
{
    private readonly IDivePointService _divePointService;

    public DivePointsController(IDivePointService divePointService)
    {
        _divePointService = divePointService;
    }

    // GET: api/divepoints
    [HttpGet]
    public async Task<ActionResult<List<DivePoint>>> GetAllDivePoints()
    {
        var divePoints = await _divePointService.GetAllDivePointsAsync();
        return Ok(divePoints);
    }

    // GET: api/divepoints/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DivePoint>> GetDivePointById(int id)
    {
        var divePoint = await _divePointService.GetDivePointByIdAsync(id);
        if (divePoint == null)
        {
            return NotFound();
        }

        return Ok(divePoint);
    }

    // POST: api/divepoints
    [HttpPost]
    public async Task<ActionResult<DivePoint>> AddDivePoint([FromBody] DivePoint divePoint)
    {
        if (divePoint == null)
        {
            return BadRequest("DivePoint cannot be null");
        }

        await _divePointService.AddDivePointAsync(divePoint);
        return CreatedAtAction(nameof(GetDivePointById), new { id = divePoint.DivePointId }, divePoint);
    }

    // PUT: api/divepoints/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateDivePoint(int id, [FromBody] DivePoint divePoint)
    {
        if (id != divePoint.DivePointId)
        {
            return BadRequest("DivePoint ID mismatch");
        }

        var existingDivePoint = await _divePointService.GetDivePointByIdAsync(id);
        if (existingDivePoint == null)
        {
            return NotFound();
        }

        await _divePointService.UpdateDivePointAsync(divePoint);
        return NoContent();
    }

    // DELETE: api/divepoints/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDivePoint(int id)
    {
        var divePoint = await _divePointService.GetDivePointByIdAsync(id);
        if (divePoint == null)
        {
            return NotFound();
        }

        await _divePointService.DeleteDivePointAsync(id);
        return NoContent();
    }
}
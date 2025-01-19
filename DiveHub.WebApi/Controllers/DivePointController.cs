using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivePointController(IDivePointService divePointService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDivePoint([FromBody] DivePointDto divePointDto)
    {
        await divePointService.CreateDivePointAsync(divePointDto, 1); //TODO: Change User ID 
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDivePointById(int id)
    {
        var divePoint = await divePointService.GetDivePointByIdAsync(id);
        return divePoint != null ? Ok(divePoint) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDivePoints()
    {
        var divePoints = await divePointService.GetAllDivePointsAsync();
        return Ok(divePoints);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDivePoint([FromBody] DivePointDto divePointDto)
    {
        await divePointService.UpdateDivePointAsync(divePointDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDivePoint(int id)
    {
        await divePointService.DeleteDivePointAsync(id);
        return NoContent();
    }
}
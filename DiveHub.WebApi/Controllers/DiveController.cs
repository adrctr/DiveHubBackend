using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DiveHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiveController(IDiveService diveService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDive([FromBody] DiveDto diveDto)
    {
        await diveService.CreateDiveAsync(diveDto, 1); //TODO: Change User ID 
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiveById(int id)
    {
        var dive = await diveService.GetDiveByIdAsync(id);
        return dive != null ? Ok(dive) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDives()
    {
        var dives = await diveService.GetAllDivesAsync();
        return Ok(dives);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDive([FromBody] DiveDto diveDto)
    {
        await diveService.UpdateDiveAsync(diveDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDive(int id)
    {
        await diveService.DeleteDiveAsync(id);
        return NoContent();
    }
}
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
    [ProducesResponseType<DiveDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDive([FromBody] DiveSaveDto diveDto)
    {
        DiveDto diveCreated = await diveService.CreateDiveAsync(diveDto, 1); //TODO: Change User ID 
        return Ok(diveCreated);
    }
   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiveById(int id)
    {
        var dive = await diveService.GetDiveByIdAsync(id);
        return dive != null ? Ok(dive) : NotFound();
    }

    [HttpGet("All")]
    [ProducesResponseType(typeof(IEnumerable<DiveDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDives()
    {
        var dives = await diveService.GetAllDivesAsync();
        return Ok(dives);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(DiveDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateDive([FromBody] DiveDto diveDto)
    {
        await diveService.UpdateDiveAsync(diveDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDive(int id)
    {
        await diveService.DeleteDiveAsync(id);
        return NoContent();
    }
}
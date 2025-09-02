using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiveHub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DiveController(IDiveService diveService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<DiveDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDive([FromBody] DiveSaveDto diveDto)
    {
        var useridauth0 = GetCurrentAuth0UserId();
        if (useridauth0 == null)
        {
            return Unauthorized("Utilisateur non authentifié");
        }
        DiveDto diveCreated = await diveService.CreateDiveAsync(diveDto, useridauth0);
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
        var userId = GetCurrentAuth0UserId();
        if (userId == null)
        {
            return Unauthorized("Utilisateur non authentifié");
        }
        var dives = await diveService.GetAllDivesAsync(userId);
        return Ok(dives);
    }

    [HttpPut]
    [ProducesResponseType(typeof(DiveDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateDive([FromBody] DiveDto diveDto)
    {
        var userId = GetCurrentAuth0UserId();
        if (userId == null)
        {
            return Unauthorized("Utilisateur non authentifié");
        }
        await diveService.UpdateDiveAsync(diveDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDive(int id)
    {
        await diveService.DeleteDiveAsync(id);
        return NoContent();
    }

    private string? GetCurrentAuth0UserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         User.FindFirst("sub")?.Value ??
                         User.FindFirst("user_id")?.Value;

        return userIdClaim;
    }
}
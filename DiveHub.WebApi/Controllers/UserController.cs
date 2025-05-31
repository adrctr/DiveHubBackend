using System.Security.Claims;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiveHub.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost("sync")]
    public async Task<IActionResult> SyncUserFromProviderAsync()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (userId == null) return Unauthorized(userId);

        UserDto user =  await userService.SyncUserFromProviderAsync(userId, email);
        return Ok(user);
    }
}
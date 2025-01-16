using DiveHub.Application.Services;
using DiveHub.Core.Entities;

namespace DiveHub.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return Ok(await userService.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> Add(User user)
    {
        await userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, User user)
    {
        if (id != user.UserId) return BadRequest();
        await userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }
}
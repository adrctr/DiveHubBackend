using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;

namespace DiveHub.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> AddUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("User cannot be null");
        }

        await userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.UserId)
        {
            return BadRequest("User ID mismatch");
        }

        var existingUser = await userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        await userService.UpdateUserAsync(user);
        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await userService.DeleteUserAsync(id);
        return NoContent();
    }

    // POST: api/users/{id}/add-dive
    [HttpPost("{id}/add-dive")]
    public async Task<ActionResult> AddDiveToUser(int id, [FromBody] Dive dive)
    {
        if (dive == null)
        {
            return BadRequest("Dive cannot be null");
        }

        await userService.AddDiveAsync(id, dive);
        return NoContent();
    }

    // GET: api/users/{id}/dives
    [HttpGet("{id}/dives")]
    public async Task<ActionResult<List<Dive>>> GetUserDives(int id)
    {
        var dives = await userService.GetDiveAsync(id);
        return Ok(dives);
    }
}
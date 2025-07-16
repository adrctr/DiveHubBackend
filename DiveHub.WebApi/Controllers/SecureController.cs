using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetSecret()
    {
        return Ok("You are authorized!");
    }
}
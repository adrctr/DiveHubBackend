
using Microsoft.AspNetCore.Mvc;

namespace DiveHub.WebApi.Controllers
{
    /// <summary>
    ///Dives Controller
    /// </summary>
    /// <returns></returns>
    [Route("[controller]")]
    [ApiController]
    public class DivesController() : ControllerBase
    {
        /// <summary>
        /// Get a list of Dives
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            return Ok();
        }

        /// <summary>
        /// Get a Dive by id
        /// </summary>
        /// <returns></returns>z
        /// 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok();
        }
    }
}

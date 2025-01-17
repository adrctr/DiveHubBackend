using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DiveHubWebApi.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class DivesController(IDiveService diveService) : ControllerBase
    {
        // GET: api/dives
        [HttpGet]
        public async Task<ActionResult<List<Dive>>> GetAllDives()
        {
            var dives = await diveService.GetAllDivesAsync();
            return Ok(dives);
        }

        // GET: api/dives/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dive>> GetDiveById(int id)
        {
            var dive = await diveService.GetDiveByIdAsync(id);
            if (dive == null)
            {
                return NotFound();
            }
            return Ok(dive);
        }

        // POST: api/dives
        [HttpPost]
        public async Task<ActionResult<Dive>> AddDive([FromBody] Dive dive)
        {
            if (dive == null)
            {
                return BadRequest("Dive cannot be null");
            }

            await diveService.AddDiveAsync(dive);
            return CreatedAtAction(nameof(GetDiveById), new { id = dive.DiveId }, dive);
        }

        // PUT: api/dives/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDive(int id, [FromBody] Dive dive)
        {
            if (id != dive.DiveId)
            {
                return BadRequest("Dive ID mismatch");
            }

            var existingDive = await diveService.GetDiveByIdAsync(id);
            if (existingDive == null)
            {
                return NotFound();
            }

            await diveService.UpdateDiveAsync(dive);
            return NoContent();
        }

        // DELETE: api/dives/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDive(int id)
        {
            var dive = await diveService.GetDiveByIdAsync(id);
            if (dive == null)
            {
                return NotFound();
            }

            await diveService.DeleteDiveAsync(id);
            return NoContent();
        }

        // POST: api/dives/{id}/add-dive-point
        [HttpPost("{id}/add-dive-point")]
        public async Task<ActionResult> AddDivePointToDive(int id, [FromBody] DivePoint divePoint)
        {
            if (divePoint == null)
            {
                return BadRequest("DivePoint cannot be null");
            }

            await diveService.AddDivePointAsync(id, divePoint);
            return NoContent();
        }

        // GET: api/dives/{id}/dive-points
        [HttpGet("{id}/dive-points")]
        public async Task<ActionResult<List<DivePoint>>> GetDivePoints(int id)
        {
            var divePoints = await diveService.GetDivePointsAsync(id);
            return Ok(divePoints);
        }
}
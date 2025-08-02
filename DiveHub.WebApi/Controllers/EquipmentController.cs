using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentHub.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EquipmentController(IEquipmentService EquipmentService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<EquipmentDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEquipment([FromBody] EquipmentSaveDto EquipmentDto)
    {
        EquipmentDto EquipmentCreated = await EquipmentService.CreateEquipmentAsync(EquipmentDto);
        return Ok(EquipmentCreated);
    }

    [HttpGet("All")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEquipments()
    {
        var Equipments = await EquipmentService.GetAllEquipmentsAsync();
        return Ok(Equipments);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEquipment([FromBody] EquipmentDto EquipmentDto)
    {
        await EquipmentService.UpdateEquipmentAsync(EquipmentDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        await EquipmentService.DeleteEquipmentAsync(id);
        return NoContent();
    }
}
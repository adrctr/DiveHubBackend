
namespace DiveHub.Application.Dto;

public partial class EquipmentDto
{
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } =string.Empty;

}

public partial class EquipmentSaveDto
{
    public string EquipmentName { get; set; } = string.Empty;
}
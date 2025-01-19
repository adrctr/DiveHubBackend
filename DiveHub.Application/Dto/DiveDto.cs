using DiveHub.Core.Entities;

namespace DiveHub.Application.Dto;

public partial class DiveDto
{
    public int DiveId { get; set; }
    public string DiveName { get; set; } = string.Empty;
    public DateTime DiveDate { get; set; }
    public string? Description { get; set; }
}

public partial class DiveSaveDto
{
    public int DiveId { get; set; }
    public string DiveName { get; set; } = string.Empty;
    public DateTime DiveDate { get; set; }
    public string? Description { get; set; }
    public List<DiveSavePointDto> Points { get; set; } = [];
    public List<DiveSavePhotoDto> Photos { get; set; } = [];
}
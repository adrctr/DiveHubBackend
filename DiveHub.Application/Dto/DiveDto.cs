using DiveHub.Core.Entities;

namespace DiveHub.Application.Dto;

public partial class DiveDto
{
    public int DiveId { get; set; }
    public string DiveName { get; set; } = string.Empty;
    public DateTime? DiveDate { get; set; }
    public string? Description { get; set; }
}

public partial class DiveSaveDto
{
    public string DiveName { get; set; } = string.Empty;
    public DateTime? DiveDate { get; set; }
    public string? Description { get; set; }
    public List<DiveSavePointDto> Points { get; set; } = [];
    public List<DivePhotoSaveDto> Photos { get; set; } = [];
}

public partial class DiveDetailDto
{
    public int DiveId { get; set; }
    public string DiveName { get; set; } = string.Empty;
    public DateTime? DiveDate { get; set; }
    public string? Description { get; set; }
    public List<DivePointDto> DivePoints { get; set; } = [];
    public List<DivePhotoDto> DivePhotos { get; set; } = [];
}
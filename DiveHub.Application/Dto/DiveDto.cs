namespace DiveHub.Application.Dto;

public partial class DiveDto
{
    public int DiveId { get; set; }
    public string DiveName { get; set; } = string.Empty;
    public DateTime DiveDate { get; set; }
    public string? Description { get; set; }
}
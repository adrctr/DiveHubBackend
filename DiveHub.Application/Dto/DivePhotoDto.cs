
namespace DiveHub.Core.Entities;
public class DivePhotoDto
{
    public int DivePhotoId { get; set; }

    public int DiveId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }

public class DiveSavePhotoDto
{
    public int DiveId { get; set; }
    public string FileName { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

}
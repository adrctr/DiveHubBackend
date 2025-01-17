namespace DiveHub.Core.Entities;
public class DivePhoto
{
    public int DivePhotoId { get; set; }

    public int DiveId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

}
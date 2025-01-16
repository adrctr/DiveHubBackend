namespace DiveHub.Core.Entities;
public class DivePhoto
{
    public int DivephotoId { get; set; }

    public int DiveId { get; set; }

    public string Filename { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

}

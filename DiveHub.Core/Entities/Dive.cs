namespace DiveHub.Core.Entities;

public class Dive
{
    public int DiveId { get; set; }
    public int UserId { get; set; }
    public string DiveName { get; set; } = null!;
    public DateTime? DiveDate { get; set; }
    public float Depth { get; set; }
    public int Duration { get; set; }

    public string? Description { get; set; }
    // public ICollection<DivePoint> DivePoints { get; set; } = new List<DivePoint>();
    // public ICollection<DivePhoto> DivePhotos { get; set; } = new List<DivePhoto>();

}
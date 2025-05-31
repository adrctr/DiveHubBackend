namespace DiveHub.Core.Entities;

public partial class Dive
{
    public Guid DiveId { get; set; }
    public Guid UserId { get; set; }
    public string DiveName { get; set; } = null!;
    public DateTime? DiveDate { get; set; }
    public string? Description { get; set; }
    // public ICollection<DivePoint> DivePoints { get; set; } = new List<DivePoint>();
    // public ICollection<DivePhoto> DivePhotos { get; set; } = new List<DivePhoto>();

}
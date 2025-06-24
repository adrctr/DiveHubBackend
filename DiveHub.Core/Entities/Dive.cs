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
   
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

    // public ICollection<DivePhoto> DivePhotos { get; set; } = new List<DivePhoto>();

}
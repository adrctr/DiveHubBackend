namespace DiveHub.Core.Entities;

public class DivePoint
{
    public int DivepointId { get; set; }

    public int DiveId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string? Description { get; set; }

}

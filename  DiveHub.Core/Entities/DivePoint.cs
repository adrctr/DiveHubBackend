namespace DiveHub.Core.Entities;

public class DivePoint
{
    public int DivePointId { get; set; }

    public int DiveId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? Description { get; set; }

}

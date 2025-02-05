namespace DiveHub.Application.Dto;

public class DivePointDto
{
    public int DivePointId { get; set; }

    public int DiveId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? Description { get; set; }

}

public class DiveSavePointDto
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? Description { get; set; }

}

namespace ParkingControl.Models;

public class ParkingZone
{
    public int ZoneId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Description { get; set; }
}
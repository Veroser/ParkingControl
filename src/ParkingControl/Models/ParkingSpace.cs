namespace ParkingControl.Models;

public class ParkingSpace
{
    public int SpaceId { get; set; }
    public int ZoneId { get; set; }
    public string SpaceNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public bool IsOccupied { get; set; }
}
namespace ParkingControl.Models;

public class ParkingSession
{
    public int SessionId { get; set; }
    public int VehicleId { get; set; }
    public int SpaceId { get; set; }
    public int TariffId { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal? TotalCost { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string SpaceNumber { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public bool IsActive => ExitTime is null;
}
namespace ParkingControl.Models;

public class Client
{
    public int ClientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int VehicleCount { get; set; }
}
namespace ParkingControl.Models;

public class Vehicle
{
    public int VehicleId { get; set; }
    public int ClientId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
}
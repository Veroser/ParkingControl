namespace ParkingControl.Models;

public class Tariff
{
    public int TariffId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
}
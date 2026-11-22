using ParkingControl.Data;
using ParkingControl.Models;

namespace ParkingControl.Services;

public class ParkingService
{
    private readonly ClientRepository _clients = new();
    private readonly VehicleRepository _vehicles = new();
    private readonly SpaceRepository _spaces = new();
    private readonly SessionRepository _sessions = new();
    private readonly ReportRepository _reports = new();

    public List<Client> SearchClients(string? text) => _clients.Search(text);
    public List<Vehicle> SearchVehicles(string? text) => _vehicles.Search(text);
    public List<ParkingSpace> GetFreeSpaces() => _spaces.GetFree();
    public List<ParkingSession> SearchSessions(string? text, bool onlyActive) => _sessions.Search(text, onlyActive);
    public List<ReportRow> GetReport(DateTime from, DateTime to) => _reports.GetRevenueByZone(from, to);
    public List<Tariff> GetTariffs() => _reports.GetTariffs();

    public void SaveClient(Client c)
    {
        ValidationService.RequireNotEmpty(c.FullName, "ФИО");
        ValidationService.RequirePhone(c.Phone);
        if (_clients.ExistsPhone(c.Phone, c.ClientId == 0 ? null : c.ClientId))
            throw new InvalidOperationException("Клиент с таким телефоном уже существует.");
        if (c.ClientId == 0) _clients.Insert(c); else _clients.Update(c);
    }

    public void DeleteClient(int id) => _clients.Delete(id);

    public void SaveVehicle(Vehicle v)
    {
        ValidationService.RequirePlate(v.PlateNumber);
        ValidationService.RequireNotEmpty(v.Model, "Модель");
        if (v.VehicleId == 0) _vehicles.Insert(v); else _vehicles.Update(v);
    }

    public void DeleteVehicle(int id) => _vehicles.Delete(id);

    public int OpenSession(int vehicleId, int spaceId, int tariffId)
    {
        if (vehicleId == 0 || spaceId == 0 || tariffId == 0)
            throw new ArgumentException("Выберите автомобиль, место и тариф.");
        return _sessions.OpenSessionWithLock(vehicleId, spaceId, tariffId);
    }

    public decimal CloseSession(int sessionId) => _sessions.CloseSession(sessionId);
}
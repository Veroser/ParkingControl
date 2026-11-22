using Microsoft.Data.SqlClient;
using ParkingControl.Models;

namespace ParkingControl.Data;

public class SessionRepository
{
    public List<ParkingSession> Search(string? text, bool onlyActive)
    {
        var list = new List<ParkingSession>();
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT ps.SessionId, ps.VehicleId, ps.SpaceId, ps.TariffId,
                   ps.EntryTime, ps.ExitTime, ps.TotalCost,
                   v.PlateNumber, s.SpaceNumber, t.Name
            FROM ParkingSessions ps
            JOIN Vehicles v      ON v.VehicleId = ps.VehicleId
            JOIN ParkingSpaces s ON s.SpaceId   = ps.SpaceId
            JOIN Tariffs t       ON t.TariffId  = ps.TariffId
            WHERE (@OnlyActive = 0 OR ps.ExitTime IS NULL)
              AND (@Text IS NULL OR v.PlateNumber LIKE @Pattern OR s.SpaceNumber LIKE @Pattern)
            ORDER BY ps.EntryTime DESC;", conn);
        cmd.Parameters.AddWithValue("@OnlyActive", onlyActive ? 1 : 0);
        cmd.Parameters.AddWithValue("@Text", (object?)text ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Pattern", $"%{text ?? ""}%");
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new ParkingSession
            {
                SessionId = r.GetInt32(0),
                VehicleId = r.GetInt32(1),
                SpaceId = r.GetInt32(2),
                TariffId = r.GetInt32(3),
                EntryTime = r.GetDateTime(4),
                ExitTime = r.IsDBNull(5) ? null : r.GetDateTime(5),
                TotalCost = r.IsDBNull(6) ? null : r.GetDecimal(6),
                PlateNumber = r.GetString(7),
                SpaceNumber = r.GetString(8),
                TariffName = r.GetString(9)
            });
        }
        return list;
    }
}
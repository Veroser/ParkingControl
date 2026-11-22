using Microsoft.Data.SqlClient;
using ParkingControl.Models;

namespace ParkingControl.Data;

public class SpaceRepository
{
    public List<ParkingSpace> GetFree()
    {
        var result = new List<ParkingSpace>();
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT s.SpaceId, s.ZoneId, s.SpaceNumber, s.IsActive, z.Name
            FROM ParkingSpaces s
            JOIN ParkingZones z ON z.ZoneId = s.ZoneId
            WHERE s.IsActive = 1
              AND NOT EXISTS (SELECT 1 FROM ParkingSessions ps
                              WHERE ps.SpaceId = s.SpaceId AND ps.ExitTime IS NULL)
            ORDER BY z.Name, s.SpaceNumber;", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            result.Add(new ParkingSpace
            {
                SpaceId = r.GetInt32(0),
                ZoneId = r.GetInt32(1),
                SpaceNumber = r.GetString(2),
                IsActive = r.GetBoolean(3),
                ZoneName = r.GetString(4),
                IsOccupied = false
            });
        }
        return result;
    }
}
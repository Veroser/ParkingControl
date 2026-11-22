using Microsoft.Data.SqlClient;
using ParkingControl.Models;

namespace ParkingControl.Data;

public class VehicleRepository
{
    public List<Vehicle> Search(string? text)
    {
        var result = new List<Vehicle>();
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT v.VehicleId, v.ClientId, v.PlateNumber, v.Model, c.FullName
            FROM Vehicles v
            JOIN Clients c ON c.ClientId = v.ClientId
            WHERE (@Text IS NULL OR v.PlateNumber LIKE @Pattern OR v.Model LIKE @Pattern)
            ORDER BY v.PlateNumber;", conn);
        cmd.Parameters.AddWithValue("@Text", (object?)text ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Pattern", $"%{text ?? ""}%");
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            result.Add(new Vehicle
            {
                VehicleId = r.GetInt32(0),
                ClientId = r.GetInt32(1),
                PlateNumber = r.GetString(2),
                Model = r.GetString(3),
                ClientName = r.GetString(4)
            });
        }
        return result;
    }

    public void Insert(Vehicle v)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand(@"
            INSERT INTO Vehicles (ClientId, PlateNumber, Model)
            VALUES (@ClientId, @PlateNumber, @Model);", conn);
        cmd.Parameters.AddWithValue("@ClientId", v.ClientId);
        cmd.Parameters.AddWithValue("@PlateNumber", v.PlateNumber);
        cmd.Parameters.AddWithValue("@Model", v.Model);
        cmd.ExecuteNonQuery();
    }

    public void Update(Vehicle v)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand(@"
            UPDATE Vehicles SET ClientId=@ClientId, PlateNumber=@PlateNumber, Model=@Model
            WHERE VehicleId=@Id;", conn);
        cmd.Parameters.AddWithValue("@Id", v.VehicleId);
        cmd.Parameters.AddWithValue("@ClientId", v.ClientId);
        cmd.Parameters.AddWithValue("@PlateNumber", v.PlateNumber);
        cmd.Parameters.AddWithValue("@Model", v.Model);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        var cmd = new SqlCommand("DELETE FROM Vehicles WHERE VehicleId=@Id;", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}
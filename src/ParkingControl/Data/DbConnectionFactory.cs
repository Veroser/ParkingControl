using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ParkingControl.Data;

public static class DbConnectionFactory
{
    private static readonly string ConnectionString = LoadConnectionString();

    private static string LoadConnectionString()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
            throw new InvalidOperationException("Файл appsettings.json не найден.");
        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        return doc.RootElement
            .GetProperty("ConnectionStrings")
            .GetProperty("ParkingDb")
            .GetString()!;
    }

    public static SqlConnection Create() => new SqlConnection(ConnectionString);
}
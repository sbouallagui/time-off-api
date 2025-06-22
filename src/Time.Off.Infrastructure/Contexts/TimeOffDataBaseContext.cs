using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Time.Off.Infrastructure.Contexts;

public class TimeOffDataBaseContext
{
    private readonly string _connectionString;
    private const string ConnectionStringKey = "TimeOff";

    public TimeOffDataBaseContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringKey);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Missing or empty connection string for key '{ConnectionStringKey}'.");
        }

        _connectionString = connectionString;
    }

    public NpgsqlConnection CreateConnection()
        => new(_connectionString);
}

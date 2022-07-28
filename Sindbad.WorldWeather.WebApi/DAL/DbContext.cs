using System.Data;
using Microsoft.Data.SqlClient;

namespace Sindbad.WorldWeather.WebApi.DAL;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection");
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
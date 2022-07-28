using Dapper;

namespace Sindbad.WorldWeather.WebApi.DAL;

public class WeatherRepository : IWeatherRepository
{
    public WeatherRepository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public DbContext DbContext { get; }

    public async Task<Weather> GetLatestOrDefaultAsync(string city)
    {
        var script = "SELECT * FROM [Weather] WHERE [City]= @City   ORDER BY [CalculatedOn] DESC";
        var parameters = new DynamicParameters();
        parameters.Add("City", city);

        using var connection = DbContext.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Weather>(script, parameters);
    }

    public async Task Add(Weather weather)
    {
        var script = @"INSERT INTO [dbo].[Weather]   ([City],[CalculatedOn],[Summary],[Description],[Temp],[TempMin],[TempMax],[FeelsLike],[Visibility],[WindSpeed],[WindDegree],[Cloudiness])
                         VALUES  (@City,@CalculatedOn,@Summary, @Description, @Temp, @TempMin, @TempMax, @FeelsLike, @Visibility,@WindSpeed,@WindDegree, @Cloudiness )";

        var parameters = new DynamicParameters();
        parameters.Add("City", weather.City);
        parameters.Add("CalculatedOn", weather.CalculatedOn);
        parameters.Add("Summary", weather.Summary);
        parameters.Add("Description", weather.Description);
        parameters.Add("Temp", weather.Temp, System.Data.DbType.Decimal);
        parameters.Add("TempMin", weather.TempMin, System.Data.DbType.Decimal);
        parameters.Add("TempMax", weather.TempMax, System.Data.DbType.Decimal);
        parameters.Add("FeelsLike", weather.FeelsLike, System.Data.DbType.Decimal);
        parameters.Add("Visibility", weather.Visibility, System.Data.DbType.Decimal);
        parameters.Add("WindSpeed", weather.WindSpeed, System.Data.DbType.Decimal);
        parameters.Add("WindDegree", weather.WindDegree, System.Data.DbType.Decimal);
        parameters.Add("Cloudiness", weather.Cloudiness, System.Data.DbType.Decimal);

        using var connection = DbContext.CreateConnection();
        await connection.ExecuteAsync(script, parameters);
    }
}
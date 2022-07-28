namespace Sindbad.WorldWeather.WebApi.DAL;

public interface IWeatherRepository
{
    Task<Weather> GetLatestOrDefaultAsync(string city);

    Task Add(Weather weather);
}
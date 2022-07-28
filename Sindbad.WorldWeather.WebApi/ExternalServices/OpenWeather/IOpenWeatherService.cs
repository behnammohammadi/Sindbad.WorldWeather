namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather;

public interface IOpenWeatherService
{
    Task<OpenWeatherResponse> GetByCityAsync(string city);
}
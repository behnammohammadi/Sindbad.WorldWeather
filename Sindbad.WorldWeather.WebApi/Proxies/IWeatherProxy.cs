namespace Sindbad.WorldWeather.WebApi.Proxies;

public interface IWeatherProxy
{
    Task<ProxyResponse> GetByCity(string city);
}
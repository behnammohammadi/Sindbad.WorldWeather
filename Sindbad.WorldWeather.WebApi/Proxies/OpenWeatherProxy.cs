using Sindbad.WorldWeather.WebApi.Mappers;

namespace Sindbad.WorldWeather.WebApi.Proxies;

public class OpenWeatherProxy : IWeatherProxy
{
    public OpenWeatherProxy(ILogger<OpenWeatherProxy> logger, IOpenWeatherService realService)
    {
        Logger = logger;
        RealService = realService;
    }

    public ILogger<OpenWeatherProxy> Logger { get; }

    public IOpenWeatherService RealService { get; }

    public async Task<ProxyResponse> GetByCity(string city)
    {
        var result = await RealService.GetByCityAsync(city);

        return new ProxyResponse
        {
            Content = WeatherMapper.ToEntity(result.Content),
            Code = result.Code,
            ErrorMessage = result.ErrorMessage
        };
    }
}
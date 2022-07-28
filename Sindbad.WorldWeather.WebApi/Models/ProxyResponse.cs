namespace Sindbad.WorldWeather.WebApi.Models;

public class ProxyResponse
{
    public Entities.Weather Content { get; init; }

    public bool IsSuccess => Code.HasValue && Code == 200;

    public int? Code { get; init; }

    public string ErrorMessage { get; init; }
}
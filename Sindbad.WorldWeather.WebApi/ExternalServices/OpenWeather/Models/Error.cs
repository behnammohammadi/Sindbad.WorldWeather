namespace Sindbad.WorldWeather.WebApi.ExternalServices.OpenWeather.Models;

public class Error
{
    [JsonPropertyName("cod")]
    public string Code { get; set; }

    public string Message { get; set; }
}